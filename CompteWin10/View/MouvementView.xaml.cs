using System;
using System.Threading.Tasks;
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
    /// Vue pour la gestion des mouvements
    /// </summary>
    public sealed partial class MouvementView : IView<MouvementViewModel>
    {

        public MouvementViewModel ViewModel { get; set; }

        private bool _isLoad;

        /// <summary>
        /// Constructeur
        /// </summary>
        public MouvementView()
        {
            InitializeComponent();
            _isLoad = false;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();
            AddMouvementButton.Visibility = Visibility.Visible;
            GestionMouvementGrid.Visibility = Visibility.Collapsed;

            //Création du viewModel
            ViewModel = new MouvementViewModel(e.Parameter as Compte);
            await ViewModel.Initialization;

            //Affichage
            ScrollToBottom();
            await CheckEspaceDispo();
            MouvementCombo.SelectedIndex = ViewModel.IndexSelectedItemMouvement();
            DateMouvementDatePicker.Date = ViewModel.DateMouvement;
            await ViewModel.UpdateSoldeCompte();
            _isLoad = true;
        }


        #region liste Mouvements
        

        private async void Previous_Click(object sender, RoutedEventArgs e)
        {
            _isLoad = false;
            PreviousButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            await ViewModel.ChangePage(null, true, false,ViewModel.ListeRajoutMouvement);
            PreviousButton.IsEnabled = true;
            NextButton.IsEnabled = true;
            _isLoad = true;
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            _isLoad = false;
            PreviousButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            await ViewModel.ChangePage(null, false, true, ViewModel.ListeRajoutMouvement);
            PreviousButton.IsEnabled = true;
            NextButton.IsEnabled = true;
            _isLoad = true;
        }

        private async void OpenMouvement_Click(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var mouv = ((AppBarButton) sender).Tag as Mouvement;
            await ViewModel.ModifierMouvement(mouv);
            AddMouvementButton.Visibility = Visibility.Collapsed;
            GestionMouvementGrid.Visibility = Visibility.Visible;
            MouvementCombo.SelectedIndex = ViewModel.IndexSelectedItemMouvement();
            CategorieList.SelectedItem = ViewModel.SelectedCategorieFmList;
            VirementList.SelectedItem = ViewModel.SelectedCompteVirement;
            DateMouvementDatePicker.Date = ViewModel.DateMouvement;
            ((AppBarButton)sender).IsEnabled = true;
        }


        private void ScrollToBottom()
        {
            var selectedIndex = ListViewMouvements.Items.Count - 1;
            if (selectedIndex < 0)
                return;
            ListViewMouvements.SelectedIndex = selectedIndex;
            ListViewMouvements.UpdateLayout();
            ListViewMouvements.ScrollIntoView(ListViewMouvements.SelectedItem);
        }


        private void AddMouvementClick(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            AddMouvementButton.Visibility = Visibility.Collapsed;
            GestionMouvementGrid.Visibility = Visibility.Visible;
            MouvementCombo.SelectedIndex = ViewModel.IndexSelectedItemMouvement();
            DateMouvementDatePicker.Date = ViewModel.DateMouvement;
            ((AppBarButton)sender).IsEnabled = true;
        }

        /// <summary>
        /// Vérifie si il est possible d'écrire ou non dans l'espace de partage
        /// </summary>
        private async Task CheckEspaceDispo()
        {
            if (App.ModeApp == AppareilEnum.ModeAppareilSecondaire)
            {
                if (await RoamingUtils.IsEcritureRoamingAutorise())
                {
                    ErreurSynchroText.Visibility = Visibility.Collapsed;
                    AddMouvementButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ErreurSynchroText.Text =
                        ResourceLoader.GetForCurrentView("Errors").GetString("RoamingMouvementPlein");
                    ErreurSynchroText.Visibility = Visibility.Visible;
                    AddMouvementButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion


        #region gestion des mouvements
       
        private async void ValiderMouvement_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            if (DateMouvementDatePicker.Date != null)
            {
                ViewModel.DateMouvement = DateMouvementDatePicker.Date.Value.DateTime;
            }
            await ViewModel.SaveMouvement();

            CleanChamps();
            await CheckEspaceDispo();
            ((Button)sender).IsEnabled = true;
        }
        #endregion

        private void AnnulerMouvementClick(object sender, RoutedEventArgs e)
        {
            CleanChamps();
        }


        private void CleanChamps()
        {
            ViewModel.AnnulerMouvement();
            AddMouvementButton.Visibility = Visibility.Visible;
            GestionMouvementGrid.Visibility = Visibility.Collapsed;
            DateMouvementDatePicker.Date = DateTimeOffset.Now;
            MouvementCombo.SelectedIndex = 0;
            VirementList.SelectedItem = null;
            CategorieList.SelectedItem = null;
            ScrollToBottom();
        }

        private async void SupprimerMouvementClick(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await ViewModel.SupprimerMouvement();
            CleanChamps();
            ((Button)sender).IsEnabled = true;
        }
        
        private async void MouvementPasseCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoad)
            {
                var mouv = ((CheckBox)sender).Tag as Mouvement;
                if (mouv != null && ((CheckBox)sender).IsChecked != null)
                {
                    await ViewModel.ChangePasseMouvement(mouv, ((CheckBox)sender).IsChecked.Value);
                }
            }
        }


        private void VirementList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var selectedItem = VirementList.SelectedItem as Compte;

                if (selectedItem != null)
                {
                    ViewModel.SelectedCompteVirement = selectedItem;
                }
            }
        }

        private async void DateSoldeDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
                if (_isLoad)
                {
                    await ViewModel.UpdateDateSoldeCompte();
                    ScrollToBottom();
                }
           
        }
    }
}
