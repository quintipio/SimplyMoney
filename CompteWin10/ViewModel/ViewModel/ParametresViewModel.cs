using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Context;
using CompteWin10.Roaming.Business;
using CompteWin10.Strings;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de la page des paramètres
    /// </summary>
    public partial class ParametresViewModel : AbstractViewModel
    {
        /// <summary>
        /// Lien vers la couche de la base application
        /// </summary>
        private ApplicationBusiness _applicationBusiness;

        /// <summary>
        /// Constructeurs
        /// </summary>
        public ParametresViewModel()
        {
            Initialization = InitializeAsync();
        }

        public override sealed async Task InitializeAsync()
        {
            _applicationBusiness = new ApplicationBusiness();
            await _applicationBusiness.Initialization;

            ListeLangues = new ObservableCollection<ListeLangues.LanguesStruct>(Strings.ListeLangues.GetListesLangues());

            ListeCouleurs = new ObservableCollection<SolidColorBrush>();
            foreach (var couleur in ContexteStatic.ListeCouleur)
                ListeCouleurs.Add(GetColor(couleur));

            IsRoamingCategorieActive = App.IsRoamingCategorieActive;
            IsRoamingEcheancierActive = App.IsRoamingEcheancierActive;

        }

        /// <summary>
        ///  Retourne le solidColorBrush à appliquer à un rectangle à partir de son code couleur
        /// </summary>
        /// <param name="color">la couleur</param>
        /// <returns></returns>
        private SolidColorBrush GetColor(uint color)
        {
            var hex = string.Format("{0:X}", color);
            return new SolidColorBrush(Color.FromArgb(byte.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                byte.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                byte.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier),
                byte.Parse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier)));
        }

        /// <summary>
        /// Change la couleur de l'application en base de donnée
        /// </summary>
        /// <param name="solidColor">la nouvelle couleur sélectionnée</param>
        /// <returns>l'id de la couleur</returns>
        public async Task<int> ChangeColorApplication(SolidColorBrush solidColor)
        {
            var color =
               (uint)
                   ((solidColor.Color.A << 24) | (solidColor.Color.R << 16) | (solidColor.Color.G << 8) |
                    (solidColor.Color.B << 0));
            var id = ContexteStatic.ListeCouleur.IndexOf(color);
            await _applicationBusiness.ChangeIdCouleurBackground(id);
            return id;
        }

        /// <summary>
        /// Change la langue de l'application
        /// </summary>
        /// <param name="langue">la nouvelle langue</param>
        public async Task ChangeLangueApplication(ListeLangues.LanguesStruct langue)
        {
            Strings.ListeLangues.ChangeLangueAppli(langue);
            await _applicationBusiness.ChangeIdLangue(langue);
            DeviseUtils.GeneratePays();
            DeviseUtils.GenerateDevise();
            await ContexteAppli.GenerateCategorieMouvement();
        }

        /// <summary>
        /// Réinitialise l'application et fait retourner sur la page d'acceuil
        /// </summary>
        /// <param name="reinitRoaming">indique si les données roaming doivent être effacé</param>
        public async Task ReinitAppli(bool reinitRoaming)
        { 
            await _applicationBusiness.DeleteDatabase();
            if (reinitRoaming)
            {
                await RoamingCompteBusiness.DeleteRoaming();
                await RoamingMouvementBusiness.DeleteRoaming();
            }
            App.OpenDemarrageView();
        }

        /// <summary>
        /// active ou désactive la synchro du roaming des catégories
        /// </summary>
        /// <returns>la task</returns>
        public async Task ChangeSynchroCategorie()
        {
            await _applicationBusiness.SetRoamingCategorieActive(IsRoamingCategorieActive);
            App.IsRoamingCategorieActive = IsRoamingCategorieActive;

            if (!IsRoamingCategorieActive)
            {
                await RoamingCategorieBusiness.DeleteRoaming();
            }
            else
            {
                await RoamingCategorieBusiness.ReCreerFichierRoaming();
            }
        }

        /// <summary>
        /// active ou désactive la synchro du roaming des catégories
        /// </summary>
        /// <returns>la task</returns>
        public async Task ChangeSynchroEcheancier()
        {
            await _applicationBusiness.SetRoamingEcheancierActive(IsRoamingEcheancierActive);
            App.IsRoamingEcheancierActive = IsRoamingEcheancierActive;
            if (!IsRoamingEcheancierActive)
            {
                await RoamingEcheancierBusiness.DeleteRoaming();
            }
            else
            {
                await RoamingEcheancierBusiness.ReCreerFichierRoaming();
            }
        }
    }
}
