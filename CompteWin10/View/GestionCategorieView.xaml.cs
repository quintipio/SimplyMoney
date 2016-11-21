using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.System;
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
    /// View de la gestion des catégories
    /// </summary>
    public sealed partial class GestionCategorieView : IView<GestionCategorieViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public GestionCategorieViewModel ViewModel { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        public GestionCategorieView()
        {
            InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();
            ViewModel = new GestionCategorieViewModel();
            await ViewModel.Initialization;
        }

        private void AddCategorie_Click(object sender, RoutedEventArgs e)
        {
            AddCategorieButton.Visibility = Visibility.Collapsed;
            ViewModel.ModeOuverture = ModeOuvertureGestionCategorieEnum.OuvertureAjouterCategorie;
            GridChangeLibelle.Visibility = Visibility.Visible;
            GridListe.Visibility = Visibility.Collapsed;

            LibelleTextBox.Focus(FocusState.Keyboard);
        }

        private async void DeleteSousCategorie_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton) sender).IsEnabled = false;
            var sousCategorie = ((AppBarButton)sender).Tag as SousCategorie;
            if (sousCategorie != null)
            {
                ViewModel.SelectedSousCategorie = sousCategorie;
                ViewModel.LibelleSelected = sousCategorie.Libelle;
            }

            if (!await ViewModel.CheckSuppressionSousCategorie())
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("SupressionSousCategImpossible"));
            }
            else
            {
                await ViewModel.SuppressionSousCategorie();
            }
            ViewModel.AnnulerModifAjout();
            ((AppBarButton)sender).IsEnabled = true;
        }

        private void EditSousCategorie_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            ViewModel.ModeOuverture = ModeOuvertureGestionCategorieEnum.OuvertureModifierSousCategorie;

            var sousCategorie = ((AppBarButton)sender).Tag as SousCategorie;
            if (sousCategorie != null)
            {
                ViewModel.SelectedSousCategorie = sousCategorie;
                ViewModel.LibelleSelected = sousCategorie.Libelle;
            }

            GridChangeLibelle.Visibility = Visibility.Visible;
            GridListe.Visibility = Visibility.Collapsed;
            AddCategorieButton.Visibility = Visibility.Collapsed;
            LibelleTextBox.Focus(FocusState.Keyboard);
            ((AppBarButton)sender).IsEnabled = true;
        }

        private void EditCategorie_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            ViewModel.ModeOuverture = ModeOuvertureGestionCategorieEnum.OuvertureModifierCategorie;

            var categorie = ((AppBarButton)sender).Tag as Categorie;
            if (categorie != null)
            {
                ViewModel.SelectedCategorie =categorie;
                ViewModel.LibelleSelected = categorie.Libelle;
            }

            GridChangeLibelle.Visibility = Visibility.Visible;
            GridListe.Visibility = Visibility.Collapsed;
            AddCategorieButton.Visibility = Visibility.Collapsed;
            LibelleTextBox.Focus(FocusState.Keyboard);
            ((AppBarButton)sender).IsEnabled = true;
        }

        private void AddSousCategorie_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            ViewModel.ModeOuverture = ModeOuvertureGestionCategorieEnum.OuvertureAjouterSousCategorie;

            var categorie = ((AppBarButton)sender).Tag as Categorie;
            if (categorie != null)
            {
                ViewModel.SelectedCategorie = categorie;
            }


            GridChangeLibelle.Visibility = Visibility.Visible;
            GridListe.Visibility = Visibility.Collapsed;
            AddCategorieButton.Visibility = Visibility.Collapsed;
            LibelleTextBox.Focus(FocusState.Keyboard);
            ((AppBarButton)sender).IsEnabled = true;
        }

        private async void DeleteCategorie_Clic(object sender, RoutedEventArgs e)
        {
            ((AppBarButton)sender).IsEnabled = false;
            var categorie = ((AppBarButton)sender).Tag as Categorie;
            if (categorie != null)
            {
                ViewModel.SelectedCategorie = categorie;
                ViewModel.LibelleSelected = categorie.Libelle;
            }

            if (!await ViewModel.CheckSuppressionCategorie())
            {
               await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView("Errors").GetString("SupressionCategImpossible"));
            }
            else
            {
               await ViewModel.SuppressionCategorie();
            }
            ViewModel.AnnulerModifAjout();
            ((AppBarButton)sender).IsEnabled = true;
        }

        private async void ValiderLibelle_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await Valide();
            ((Button)sender).IsEnabled = true;
        }

        private void AnnulerLibelle_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AnnulerModifAjout();
            GridChangeLibelle.Visibility = Visibility.Collapsed;
            GridListe.Visibility = Visibility.Visible;
            AddCategorieButton.Visibility = Visibility.Visible;
        }

        private async void Page_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && GridChangeLibelle.Visibility == Visibility.Visible)
            {
                await Valide();
            }
        }

        /// <summary>
        /// Valide un libelle
        /// </summary>
        /// <returns>la task</returns>
        private async Task Valide()
        {
            var retour = await ViewModel.Save();
            if (string.IsNullOrWhiteSpace(retour))
            {
                GridChangeLibelle.Visibility = Visibility.Collapsed;
                GridListe.Visibility = Visibility.Visible;
                AddCategorieButton.Visibility = Visibility.Visible;
            }
            else
            {
                await MessageBox.ShowAsync(retour);
            }
        }
    }
}
