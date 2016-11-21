using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de démarrage
    /// </summary>
    public partial class DemarrageViewModel : AbstractViewModel
    {
        /// <summary>
        /// Lien vers la base de donnée pour la partie application
        /// </summary>
        private ApplicationBusiness _applicationBusiness;

        /// <summary>
        /// Lien vers la base de donnée pour la partie banque
        /// </summary>
        private BanqueBusiness _banqueBusiness;

        /// <summary>
        /// Lien vers la base de donnée pour la création de comptes
        /// </summary>
        private CompteBusiness _compteBusiness;
        
        /// <summary>
        /// La lsite des comptes à enregistrer
        /// </summary>
        private Dictionary<string, double?> _listeComptes;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DemarrageViewModel()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// Vérifie si des comptes existent déjà
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifCompteExistant()
        {
            return (await RoamingCompteBusiness.GetListeBanques()).Count > 0;
        }

        public override sealed async Task InitializeAsync()
        {
            _applicationBusiness = new ApplicationBusiness();
            await _applicationBusiness.Initialization;

            _banqueBusiness = new BanqueBusiness();
            await _banqueBusiness.Initialization;

            _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            _listeComptes = new Dictionary<string, double?>();

            ListePays = new ObservableCollection<Pays>(DeviseUtils.ListePays);
            ListeDevise = new ObservableCollection<Devise>(DeviseUtils.ListeDevises);
        }

        /// <summary>
        /// Méthode pour enregistrer l'appareil en tant qu'appareil principal
        /// </summary>
        /// <param name="creerCompte"></param>
        /// <returns></returns>
        public async Task CreerAppareilPrincipal(bool creerCompte)
        {
            await _applicationBusiness.CreerBase(AppareilEnum.ModeAppareilPrincipal);

            if (creerCompte)
            {
                var banqueEnBase = await _banqueBusiness.AjouterBanque(new Banque {Nom = NomBanque, IdDevise = SelectedDevise.Id, IdPays = SelectedPays.Id});

                var listeComptes =
                    _listeComptes.Select(
                        compte => new Compte {IdBanque = banqueEnBase.Id, Nom = compte.Key, Solde = compte.Value ?? 0, IdDevise = SelectedDevise.Id })
                        .ToList();
                await _compteBusiness.AjouterCompte(listeComptes);

                foreach (var compte in listeComptes)
                {
                    await _compteBusiness.AjouterSoldeInitial(compte.Solde, compte.Id);
                }
            }
            else
            {
                await _applicationBusiness.CreerBaseDeDonneeFromRoaming(AppareilEnum.ModeAppareilPrincipal);
            }
        }

        /// <summary>
        /// Ajoute un compte saisie à la liste à ajouter en base puis efface les données
        /// </summary>
        public void AjouterNouveauCompte()
        {
            if (!string.IsNullOrWhiteSpace(NomBanque) && SoldeInitialCompte != null)
            {
               
                _listeComptes.Add(NomCompte, SoldeInitialCompte);
                NomCompte = "";
                SoldeInitialCompte = null;
            }
            
        }

        /// <summary>
        /// Méthode pour enregistrer l'appareil en tant qu'appareil secondaire
        /// </summary>
        public async Task CreerAppareilSecondaire()
        {
                await _applicationBusiness.CreerBase(AppareilEnum.ModeAppareilSecondaire);
        }

        /// <summary>
        /// valide une banque ou non
        /// </summary>
        /// <returns>les erreurs sinon null</returns>
        public string ValiderBanque()
        {
            var retour = "";
            if (string.IsNullOrWhiteSpace(NomBanque) || SelectedDevise == null)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("ChampsVide")+"\r\n";
            }
            
            if (!string.IsNullOrWhiteSpace(NomBanque) && NomBanque.Length > 100)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("NomTropLong") + "\r\n";
            }
            return retour;
        }

        /// <summary>
        /// Vérifie les données d'un comte
        /// </summary>
        /// <param name="isSaisieFinie">si un autre compte sera ajouter après cette validation</param>
        /// <returns>les erreurs sinon null</returns>
        public string ValideCompte(bool isSaisieFinie)
        {
            var retour = "";
            if ((string.IsNullOrWhiteSpace(NomCompte) || SoldeInitialCompte == null) && !isSaisieFinie)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("ChampsVide") + "\r\n";
            }

            if (isSaisieFinie && _listeComptes.Count == 0 && (string.IsNullOrWhiteSpace(NomCompte) || SoldeInitialCompte == null))
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("ListeCompteVide") + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(NomCompte) && _listeComptes.ContainsKey(NomCompte))
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("CompteDejaPresent") + "\r\n";
            }

                if (!string.IsNullOrWhiteSpace(NomCompte) && NomCompte.Length > 100)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("NomTropLong") + "\r\n";
            }

            return retour;
        }
    }
}
