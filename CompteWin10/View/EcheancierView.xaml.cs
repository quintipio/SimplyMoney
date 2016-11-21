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
    /// View de l'échancier
    /// </summary>
    public sealed partial class EcheancierView : IView<EcheancierViewModel>
    {
        public EcheancierViewModel ViewModel { get; set; }

        private bool _isLoad;

        /// <summary>
        /// Constructeur
        /// </summary>
        public EcheancierView()
        {
            InitializeComponent();
            _isLoad = false;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();
            AddEcheancierButton.Visibility = Visibility.Visible;
            GestionEcheancierGrid.Visibility = Visibility.Collapsed;
            DateLimiteEcheancierDatePicker.Visibility = Visibility.Collapsed;
            GridNbJours.Visibility = Visibility.Collapsed;
            CheckDateLimite.IsChecked = false;
            
            ViewModel = new EcheancierViewModel();
            await ViewModel.Initialization;

            MouvementCombo.SelectedIndex = ViewModel.IndexSelectedItemTypeMouvementEcheancier();
            PeriodiciteCombo.SelectedIndex = ViewModel.IndexSelectedItemPeriodiciteEcheancier();
            DateEcheancierDatePicker.Date = ViewModel.DateEcheancier;
            DateLimiteEcheancierDatePicker.Date = ViewModel.DateLimiteEcheancier;
            _isLoad = true;
        }

        private async void OpenEcheancier_Click(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var mouv = ((AppBarButton)sender).Tag as Echeancier;
            await ViewModel.ModifierEcheancier(mouv);
            AddEcheancierButton.Visibility = Visibility.Collapsed;
            GestionEcheancierGrid.Visibility = Visibility.Visible;

            var indexA = 0;
            foreach (var item in CompteEcheancierList.Items)
            {
                if (((Compte) item).Id == ViewModel.SelectedCompte.Id)
                {
                    CompteEcheancierList.SelectedIndex = indexA;
                    break;
                }
                indexA++;
            }
            
            DateEcheancierDatePicker.Date = ViewModel.DateEcheancier;
            DateLimiteEcheancierDatePicker.Date = ViewModel.DateLimiteEcheancier;
            CheckDateLimite.IsChecked = ViewModel.IsDateLimite;

            MouvementCombo.SelectedIndex = ViewModel.IndexSelectedItemTypeMouvementEcheancier();
            PeriodiciteCombo.SelectedIndex = ViewModel.IndexSelectedItemPeriodiciteEcheancier();
            CategorieList.SelectedItem = ViewModel.SelectedCategorieFmList;

            if (ViewModel.SelectedCompteVirement != null)
            {
                var indexB = 0;
                foreach (var item in VirementList.Items)
                {
                    if (((Compte)item).Id == ViewModel.SelectedCompteVirement.Id)
                    {
                        VirementList.SelectedIndex = indexB;
                        break;
                    }
                    indexB++;
                }
            }
            DateLimiteEcheancierDatePicker.Date = ViewModel.DateLimiteEcheancier;
            ((AppBarButton)sender).IsEnabled = true;
        }

        private void AddEcheancier_Click(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            AddEcheancierButton.Visibility = Visibility.Collapsed;
            GestionEcheancierGrid.Visibility = Visibility.Visible;
            MouvementCombo.SelectedIndex = ViewModel.IndexSelectedItemTypeMouvementEcheancier();
            PeriodiciteCombo.SelectedIndex = ViewModel.IndexSelectedItemPeriodiciteEcheancier();
            DateEcheancierDatePicker.Date = ViewModel.DateEcheancier;
            ((AppBarButton)sender).IsEnabled = true;
        }

        #region évènements pour gérer le databind


        private void PeriodiciteCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var selectedItem = PeriodiciteCombo.SelectedItem as EnumModel;

                if (selectedItem != null)
                {
                    ViewModel.SelectedPeriodicite = selectedItem;

                    GridNbJours.Visibility = selectedItem.Id == 7 ? Visibility.Visible : Visibility.Collapsed;
                }
            }

        }
        
        private void CompteEcheancierList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var selectedItem = CompteEcheancierList.SelectedItem as Compte;

                if (selectedItem != null)
                {
                    ViewModel.SelectedCompte = selectedItem;
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
        
        private void MouvementCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var selectedItem = MouvementCombo.SelectedItem as TypeMouvement;

                if (selectedItem != null)
                {
                    ViewModel.SelectedTypeMouvement = selectedItem;
                }
            }
        }

        #endregion


        private async void SupprimerMouvementClick(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await ViewModel.SupprimerEcheancier();
            CleanChamps();
            ((Button)sender).IsEnabled = true;
        }

        private void AnnulerMouvementClick(object sender, RoutedEventArgs e)
        {
            CleanChamps();
        }
        private void CleanChamps()
        {
            ViewModel.AnnulerEcheancier();
            AddEcheancierButton.Visibility = Visibility.Visible;
            GestionEcheancierGrid.Visibility = Visibility.Collapsed;
            CompteEcheancierList.SelectedItem = null;
            PeriodiciteCombo.SelectedIndex = 0;
            MouvementCombo.SelectedIndex = 0;
            VirementList.SelectedItem = null;
            CategorieList.SelectedItem = null;

        }

        private async void ValiderMouvement_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            if (DateEcheancierDatePicker.Date != null)
            {
                ViewModel.DateEcheancier = DateEcheancierDatePicker.Date.Value.DateTime;
            }
            if (DateLimiteEcheancierDatePicker.Date != null)
            {
                ViewModel.DateLimiteEcheancier = DateLimiteEcheancierDatePicker.Date.Value.DateTime;
            }
            var retour =  await ViewModel.SaveEcheancier();

            if (string.IsNullOrWhiteSpace(retour))
            {
                CleanChamps();
            }
            else
            {
                await MessageBox.ShowAsync(retour);
            }
            ((Button)sender).IsEnabled = true;
        }

        private void CheckDateLimite_OnChecked(object sender, RoutedEventArgs e)
        {
            DateLimiteEcheancierDatePicker.Visibility = (CheckDateLimite.IsChecked != null &&
                                                         CheckDateLimite.IsChecked.Value)
                ? Visibility.Visible
                : Visibility.Collapsed;

            ViewModel.IsDateLimite = CheckDateLimite.IsChecked != null && CheckDateLimite.IsChecked.Value;
        }
    }
}
