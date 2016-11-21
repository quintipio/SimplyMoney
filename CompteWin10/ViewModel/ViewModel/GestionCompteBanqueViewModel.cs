using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de la gestion des comptes et des banque
    /// </summary>
    public partial class GestionCompteBanqueViewModel : AbstractViewModel
    {
        /// <summary>
        /// mode d'utilisation de la page
        /// </summary>
        private readonly ModeOuvertureGestionCompteBanqueEnum _modeGestion;

        /// <summary>
        /// lien vers les données des comptes
        /// </summary>
        private CompteBusiness _compteBusiness;

        /// <summary>
        /// lien vers les données des banques
        /// </summary>
        private BanqueBusiness _banqueBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public GestionCompteBanqueViewModel(ModeOuvertureGestionCompteBanqueEnum modeGestion)
        {
            Initialization = InitializeAsync();
            _modeGestion = modeGestion;
        }

        public override sealed async Task InitializeAsync()
        {
            _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            _banqueBusiness = new BanqueBusiness();
            await _banqueBusiness.Initialization;

            ListeDevise = new ObservableCollection<Devise>(DeviseUtils.ListeDevises);
            ListePays = new ObservableCollection<Pays>(DeviseUtils.ListePays);
            SelectedPays = DeviseUtils.GetPays("FR");
            SelectedDevise = DeviseUtils.GetDevise(DeviseUtils.IdEuro);
        }

        /// <summary>
        /// valide les données  à ajouter ou à modifier
        /// </summary>
        /// <returns>les erreurs éventuelles ou null si aucune</returns>
        public async Task<string> Valider()
        {
            var retour = Validate();

            if (string.IsNullOrWhiteSpace(retour))
            {
                switch (_modeGestion)
                {
                        case ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterBanque:
                        await _banqueBusiness.AjouterBanque(new Banque { Nom = Nom, IdDevise = SelectedDevise.Id, IdPays = SelectedPays.Id});
                        break;

                        case ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterCompte:
                        var idCompte = await _compteBusiness.AjouterCompte(new Compte {Nom = Nom, IdDevise = SelectedDevise.Id, IdBanque = IdParent, Solde = Solde ?? 0});
                        await _compteBusiness.AjouterSoldeInitial(Solde ?? 0, idCompte);
                        break;

                        case ModeOuvertureGestionCompteBanqueEnum.OuvertureModifierBanque:
                        await _banqueBusiness.ModifierBanque(new Banque { Id = IdElement, Nom = Nom, IdDevise = SelectedDevise.Id, IdPays = SelectedPays.Id });
                        break;

                        case ModeOuvertureGestionCompteBanqueEnum.OuvertureModifierCompte:
                        await _compteBusiness.ModifierCompte(new Compte { Id = IdElement, Nom = Nom, IdDevise = SelectedDevise.Id, IdBanque = IdParent });
                        break;
                }
            }
            return retour;
        }

        private string Validate()
        {
            var retour = "";
            if (string.IsNullOrWhiteSpace(Nom) ||
                (SelectedDevise == null && _modeGestion.Equals(ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterBanque)))
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("ChampsVide") + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(Nom) && Nom.Length > 100)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("NomTropLong") + "\r\n";
            }
            return retour;
        }
    }
}
