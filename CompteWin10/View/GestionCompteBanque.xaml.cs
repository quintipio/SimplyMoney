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
    /// Petite structure pour passer les informations de modifications par e.parameter
    /// </summary>
    public struct PassageInfoGestionCompteBanque
    {
        /// <summary>
        /// Mode d'ouverture de la fenetre
        /// </summary>
        public ModeOuvertureGestionCompteBanqueEnum Mode { get; set; }

        /// <summary>
        /// Le compte à modifier ou null si aucun
        /// </summary>
        public Compte CompteInfo { get; set; }

        //La banque à modifier ou null si aucun
        public Banque BanqueInfo { get; set; }

        public PassageInfoGestionCompteBanque(ModeOuvertureGestionCompteBanqueEnum mode, Compte compte, Banque banque)
        {
            Mode = mode;
            CompteInfo = compte;
            BanqueInfo = banque;
        }
    }

    /// <summary>
    /// Code behind de la page de gestion des banques et des comptes
    /// </summary>
    public sealed partial class GestionCompteBanque : IView<GestionCompteBanqueViewModel>
    {
        private bool _isLoad;

        private PassageInfoGestionCompteBanque _info;

        /// <summary>
        /// ViewModel
        /// </summary>
        public GestionCompteBanqueViewModel ViewModel { get; set; }

        public GestionCompteBanque()
        {
            _isLoad = false;
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();

            var param = (PassageInfoGestionCompteBanque)e.Parameter;

            ViewModel = new GestionCompteBanqueViewModel(param.Mode);
            await ViewModel.Initialization;

            
            if (param.Mode == ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterBanque)
            {
                InfoPageTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("AjouterBanqueText");
                ValiderButton.Content = ResourceLoader.GetForCurrentView().GetString("AjouterText");
                TitreTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("AjouterBanqueText");
                SoldeTextBlock.Visibility = Visibility.Collapsed;
                SoldeGrid.Visibility = Visibility.Collapsed;
            }

            if (param.Mode == ModeOuvertureGestionCompteBanqueEnum.OuvertureModifierBanque)
            {
                InfoPageTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("ModifierBanqueText");
                TitreTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("ModifierBanqueText");
                ValiderButton.Content = ResourceLoader.GetForCurrentView().GetString("ModifierText");
                ViewModel.Nom = param.BanqueInfo.Nom;
                ViewModel.IdElement = param.BanqueInfo.Id;
                SoldeTextBlock.Visibility = Visibility.Collapsed;
                SoldeGrid.Visibility = Visibility.Collapsed;
                DeviseTextBlock.Visibility = Visibility.Collapsed;
                DeviseComboBox.Visibility = Visibility.Collapsed;
                PaysComboBox.Visibility = Visibility.Collapsed;
                PaysTextBlock.Visibility = Visibility.Collapsed;
                ViewModel.SelectedDevise = DeviseUtils.GetDevise(param.BanqueInfo.IdDevise);
            }

            if (param.Mode == ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterCompte)
            {
                InfoPageTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("AjouterCompteText");
                TitreTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("AjouterCompteText");
                ValiderButton.Content = ResourceLoader.GetForCurrentView().GetString("AjouterText");
                DeviseTextBlock.Visibility = Visibility.Collapsed;
                DeviseComboBox.Visibility = Visibility.Collapsed;
                PaysComboBox.Visibility = Visibility.Collapsed;
                PaysTextBlock.Visibility = Visibility.Collapsed;
                ViewModel.IdParent = param.BanqueInfo.Id;
                ViewModel.SelectedDevise = DeviseUtils.GetDevise(param.BanqueInfo.IdDevise);
            }

            if (param.Mode == ModeOuvertureGestionCompteBanqueEnum.OuvertureModifierCompte)
            {
                InfoPageTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("ModifierCompteText");
                TitreTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("ModifierCompteText");
                ValiderButton.Content = ResourceLoader.GetForCurrentView().GetString("ModifierText");
                DeviseTextBlock.Visibility = Visibility.Collapsed;
                DeviseComboBox.Visibility = Visibility.Collapsed;
                PaysComboBox.Visibility = Visibility.Collapsed;
                PaysTextBlock.Visibility = Visibility.Collapsed;
                SoldeTextBlock.Visibility = Visibility.Collapsed;
                SoldeGrid.Visibility = Visibility.Collapsed;
                ViewModel.Nom = param.CompteInfo.Nom;
                ViewModel.IdElement = param.CompteInfo.Id;
                ViewModel.IdParent = param.CompteInfo.IdBanque;
                ViewModel.SelectedDevise = DeviseUtils.GetDevise(param.CompteInfo.IdDevise);
            }

            _info = param;
            _isLoad = true;

        }

        private async void ValiderButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            var retour = await ViewModel.Valider();
            if (!string.IsNullOrWhiteSpace(retour))
            {
                await MessageBox.ShowAsync(retour);
            }
            else
            {
                App.AppShell.NavigateFrame(typeof(AcceuilView));
            }
            ((Button)sender).IsEnabled = true;
        }

        private void AnnulerButton_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(AcceuilView));
        }

        private void PaysComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                ViewModel.SelectedPays = PaysComboBox.SelectedItem as Pays;
                DeviseComboBox.SelectedIndex = 0;
            }
        }

        private void DeviseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                ViewModel.SelectedDevise = DeviseComboBox.SelectedItem as Devise;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_info.Mode == ModeOuvertureGestionCompteBanqueEnum.OuvertureAjouterBanque)
            {

                PaysComboBox.SelectedItem = DeviseUtils.GetPays("FR");
                DeviseComboBox.SelectedIndex = 0;
            }
        }
    }
}
