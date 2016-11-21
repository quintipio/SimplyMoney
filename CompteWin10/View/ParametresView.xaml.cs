using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Interface;
using CompteWin10.Roaming.Business;
using CompteWin10.Strings;
using CompteWin10.Utils;
using CompteWin10.ViewModel;

namespace CompteWin10.View
{
    /// <summary>
    /// Code behind de la vue des paramètres
    /// </summary>
    public sealed partial class ParametresView : IView<ParametresViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public ParametresViewModel ViewModel { get; set; }

        private bool _canChangeLangue;


        /// <summary>
        /// Constructeur
        /// </summary>
        public ParametresView()
        {
            InitializeComponent();
            _canChangeLangue = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = new ParametresViewModel();
            await ViewModel.InitializeAsync();
           TitreGrid.Background = App.AppShell.GetCouleur();

            if (App.ModeApp == AppareilEnum.ModeAppareilSecondaire)
            {
                GestionRoaming.Visibility = Visibility.Collapsed;
            }

            //mise en place des langues
            ComboListeLangue.ItemsSource = ViewModel.ListeLangues;
            ComboListeLangue.SelectedItem = ListeLangues.GetLangueEnCours();

            //mise en place des couleurs
            ListeColorGridView.ItemsSource = ViewModel.ListeCouleurs;

            //Calcul pour le roaming
            await CalculerEspaceDispo();

            CategorieCheckBox.IsChecked = ViewModel.IsRoamingCategorieActive;
            EcheancierCheckBox.IsChecked = ViewModel.IsRoamingEcheancierActive;

        }

        /// <summary>
        /// Recalcul les infos d'espace pour le romaing
        /// </summary>
        private async Task CalculerEspaceDispo()
        {
            var espaceTotal = await RoamingUtils.GetEspaceRoamingOccupePourcent();
            EspaceDispoTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("EspaceDispo") +" "+ espaceTotal + " %";
            EspaceDispoTextBlock.Foreground = espaceTotal >= ContexteStatic.EspaceMaxAutoriseRoaming ? new SolidColorBrush(Color.FromArgb(255,255,0,0)) : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            EspaceCompteOccupe.Text = await RoamingCompteBusiness.GetEspaceFichierOccupePourcent() + " %";
            EspaceMouvementOccupe.Text = await RoamingMouvementBusiness.GetEspaceFichierOccupePourcent() + " %";
            EcheancierCheckBox.Content = await RoamingEcheancierBusiness.GetEspaceFichierOccupePourcent() + " %";
            CategorieCheckBox.Content = await RoamingCategorieBusiness.GetEspaceFichierOccupePourcent() + " %";
        }

        private async void comboListeLangue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_canChangeLangue && ComboListeLangue.SelectedItem is ListeLangues.LanguesStruct)
            {
                _canChangeLangue = false;
                await ViewModel.ChangeLangueApplication((ListeLangues.LanguesStruct) ComboListeLangue.SelectedItem);
                _canChangeLangue = true;
            }

        }

        private async void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var solidColor = ((SolidColorBrush)((Rectangle)sender).Tag);
            App.IdCouleurBackground = await ViewModel.ChangeColorApplication(solidColor);
            App.AppShell.ChangeCouleurShell();
            TitreGrid.Background = App.AppShell.GetCouleur();

        }

        private void ReinitA_Click(object sender, RoutedEventArgs e)
        {
            InfoReinitText.Visibility = Visibility.Visible;
            ReinitAllCheck.Visibility = Visibility.Visible;
            ReinitButton.Visibility = Visibility.Visible;
        }

        private async void ReinitB_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await ViewModel.ReinitAppli(ReinitAllCheck.IsChecked??false);
            ((Button)sender).IsEnabled = true;
        }

        private async void EcheancierCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (EcheancierCheckBox.IsChecked != null)
            {
                ViewModel.IsRoamingEcheancierActive = EcheancierCheckBox.IsChecked.Value;
                await ViewModel.ChangeSynchroEcheancier();
                await CalculerEspaceDispo();
            }
        }

        private async void CategorieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CategorieCheckBox.IsChecked != null)
            {
                ViewModel.IsRoamingCategorieActive = CategorieCheckBox.IsChecked.Value;
                await ViewModel.ChangeSynchroCategorie();
                await CalculerEspaceDispo();
            }
        }
    }
}
