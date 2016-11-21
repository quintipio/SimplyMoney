using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de la page principale
    /// </summary>
    public partial class AcceuilViewModel : AbstractViewModel
    {
        /// <summary>
        /// Communication vers la couche donnée pour les comptes
        /// </summary>
        private CompteBusiness _compteBusiness;

        /// <summary>
        /// Communication vers la couche donnée pour les banques
        /// </summary>
        private BanqueBusiness _banqueBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public AcceuilViewModel()
        {
            Initialization = InitializeAsync();
        }

        public sealed override async Task InitializeAsync()
        {
            _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            _banqueBusiness = new BanqueBusiness();
            await _banqueBusiness.Initialization;
            
            ListeBanque = (App.ModeApp.Equals(AppareilEnum.ModeAppareilPrincipal))? await _compteBusiness.GetResumeCompte(): await RoamingCompteBusiness.GetListeBanques();
            
            //calcul des sommes de chaque banque et du total
            foreach (var banque in ListeBanque)
            {
                var somme = banque.ListeCompte.Sum(compte => compte.Solde);
                banque.SoldeBanque =StringUtils.SeparerNombreEspace(somme.ToString(CultureInfo.InvariantCulture))+ " "+DeviseUtils.GetDevise(banque.IdDevise).Signe;
            }
            Visible = App.ModeApp.Equals(AppareilEnum.ModeAppareilPrincipal);
        }

        /// <summary>
        /// supprime un compte de la base et du roaming
        /// </summary>
        /// <param name="compte">le compte à supprimer</param>
        public async Task SupprimerCompte(Compte compte)
        {
            await _compteBusiness.SupprimerCompte(compte);
            await _compteBusiness.SupprimerSoldeInitial(compte.Id);
            ListeBanque = await _compteBusiness.GetResumeCompte();
        }

        /// <summary>
        /// Supprime une banque de la base et du roaming
        /// </summary>
        /// <param name="banque">la banque à supprimer</param>
        public async Task SupprimerBanque(Banque banque)
        {
            var listeCompte =await _banqueBusiness.GetCompteFmBanque(banque.Id);
           await _banqueBusiness.SupprimerBanque(banque);
            foreach (var compte in listeCompte)
            {
                await _compteBusiness.SupprimerSoldeInitial(compte.Id);
            }
            ListeBanque = await _compteBusiness.GetResumeCompte();
        }
    }
}
