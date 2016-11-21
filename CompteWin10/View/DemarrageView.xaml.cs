using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CompteWin10.Interface;
using CompteWin10.Model;
using CompteWin10.Utils;
using CompteWin10.ViewModel;

namespace CompteWin10.View
{
    /// <summary>
    /// Vue pour démarrer l'application
    /// </summary>
    public sealed partial class DemarrageView : IView<DemarrageViewModel>
    {
        private bool _isLoad;

        /// <summary>
        /// ViewModel
        /// </summary>
        public DemarrageViewModel ViewModel { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        public DemarrageView()
        {
            _isLoad = false;
           InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new DemarrageViewModel();
            await ViewModel.Initialization;

            _isLoad = true;

        }


        private async void ButtonAppPrincipal_Click(object sender, RoutedEventArgs e)
        {
            ButtonAppPrincipal.IsEnabled = false;
            ButtonAppSecondaire.IsEnabled = false;
            if (!await ViewModel.VerifCompteExistant())
            {

                PaysComboBox.SelectedItem = DeviseUtils.GetPays("FR");
                DeviseComboBox.SelectedIndex = 0;
                ChoixAppareilGrid.Visibility = Visibility.Collapsed;
                DonneesBanques.Visibility = Visibility.Visible;
            }
            else
            {
                await ViewModel.CreerAppareilPrincipal(false);
                App.OpenShell();
            }

        }

        private async void ButtonAppSecondaire_Click(object sender, RoutedEventArgs e)
        {
            ButtonAppPrincipal.IsEnabled = false;
            ButtonAppSecondaire.IsEnabled = false;
            await ViewModel.CreerAppareilSecondaire();
            App.OpenShell();
        }

        private async void ValiderBanqueButton_Click(object sender, RoutedEventArgs e)
        {
            var retour = ViewModel.ValiderBanque();
            if (!string.IsNullOrWhiteSpace(retour))
            {
                await MessageBox.ShowAsync(retour);
            }
            else
            {
                DonneesBanques.Visibility = Visibility.Collapsed;
                DonneesCompte.Visibility = Visibility.Visible;
            }
        }


        private async void ValiderCompteButton_Click(object sender, RoutedEventArgs e)
        {
            var retour = ViewModel.ValideCompte(true);
            if (!string.IsNullOrWhiteSpace(retour))
            {
                await MessageBox.ShowAsync(retour);
            }
            else
            {
                ValiderTerminerCompteButton.IsEnabled = false;
                ValiderAjouterCompteButton.IsEnabled = false;
                ViewModel.AjouterNouveauCompte();
                await ViewModel.CreerAppareilPrincipal(true);
                App.OpenShell();
            }
        }

        private async void ValiderAjouterCompteButton_Click(object sender, RoutedEventArgs e)
        {
            var retour = ViewModel.ValideCompte(false);
            if (!string.IsNullOrWhiteSpace(retour))
            {
                await MessageBox.ShowAsync(retour);
            }
            else
            {
                ViewModel.AjouterNouveauCompte();
            }
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
    }
}
