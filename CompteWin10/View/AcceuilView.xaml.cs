
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CompteWin10.Enum;
using CompteWin10.Interface;
using CompteWin10.Model;
using CompteWin10.Utils;
using CompteWin10.ViewModel;

namespace CompteWin10.View
{
    /// <summary>
    /// Vue de la page principale
    /// </summary>
    public sealed partial class AcceuilView : IView<AcceuilViewModel>
    {
        public AcceuilViewModel ViewModel { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        public AcceuilView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new AcceuilViewModel();
            await ViewModel.Initialization;
            

            TitreGrid.Background = App.AppShell.GetCouleur();

            if (App.ModeApp == AppareilEnum.ModeAppareilSecondaire && ViewModel.ListeBanque.Count == 0)
            {
                AcceuilScroll.Visibility = Visibility.Collapsed;
                ErrorTextBlock.Visibility = Visibility.Visible;
                ErrorTextBlock.Text = ResourceLoader.GetForCurrentView("Errors").GetString("AucunCompteSecondaire");
            }
        }


        #region GestionBanque
        private void AddBanque_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            App.AppShell.NavigateFrame(typeof(GestionCompteBanque), new PassageInfoGestionCompteBanque(ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterBanque, null, null));
            ((AppBarButton)sender).IsEnabled = true;
        }

        private void EditBanque_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var banque = (sender as AppBarButton).Tag as Banque;
            App.AppShell.NavigateFrame(typeof(GestionCompteBanque), new PassageInfoGestionCompteBanque(ModeOuvertureGestionCompteBanqueEnum.OuvertureModifierBanque, null, banque));
            ((AppBarButton)sender).IsEnabled = true;
        }

        private async void DeleteBanque_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var reponse = await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("SupprimerBanqueText"), ResourceLoader.GetForCurrentView().GetString("AttentionText"), MessageBoxButton.YesNo);

            if (reponse == MessageBoxResult.Yes)
            {
                var banque = (sender as AppBarButton).Tag as Banque;
                await ViewModel.SupprimerBanque(banque);
            }
            ((AppBarButton)sender).IsEnabled = true;
        }

        #endregion

        #region GestionCompte

        private void ConsultCompte_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var compte = (sender as AppBarButton).Tag as Compte;
            App.AppShell.NavigateFrame(typeof(MouvementView),compte);
            ((AppBarButton)sender).IsEnabled = true;
        }

        private void EditCompte_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var compte = (sender as AppBarButton).Tag as Compte;
            App.AppShell.NavigateFrame(typeof(GestionCompteBanque), new PassageInfoGestionCompteBanque(ModeOuvertureGestionCompteBanqueEnum.OuvertureModifierCompte, compte, null));
            ((AppBarButton)sender).IsEnabled = true;
        }


        private async void DeleteCompte_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var reponse = await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("SupprimerCompteText"), ResourceLoader.GetForCurrentView().GetString("AttentionText"), MessageBoxButton.YesNo);

            if (reponse == MessageBoxResult.Yes)
            {
                var compte = (sender as AppBarButton).Tag as Compte;
                await ViewModel.SupprimerCompte(compte);
            }
            ((AppBarButton)sender).IsEnabled = true;
        }
        
        private void AddCompte_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var banque = (sender as AppBarButton).Tag as Banque;
            App.AppShell.NavigateFrame(typeof(GestionCompteBanque), new PassageInfoGestionCompteBanque(ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterCompte, null, banque));
            ((AppBarButton)sender).IsEnabled = true;
        }

        #endregion
    }
}
