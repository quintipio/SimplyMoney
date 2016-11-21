using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.View;

namespace CompteWin10
{
    /// <summary>
    /// CS du shell de 'app (contient la frame affichant toute les pages)
    /// </summary>
    public sealed partial class Shell
    {
        
        
        /// <summary>
        /// Constructeur
        /// </summary>
        public Shell()
        {
            InitializeComponent();

            if (App.ModeApp == AppareilEnum.ModeAppareilSecondaire)
            {
                EcheancierRadioButton.Visibility = Visibility.Collapsed;
                CategRadioButton.Visibility = Visibility.Collapsed;
                StatsRadioButton.Visibility = Visibility.Collapsed;
            }
            ChangeCouleurShell();
        }


        #region Navigation
        /// <summary>
        /// Ouvre une frame dans la page
        /// </summary>
        /// <param name="type">le type de page à afficher</param>
        public void NavigateFrame(Type type)
        {
            Frame.Navigate(type);
        }

        /// <summary>
        /// ouvre une frame en passant un paramètre
        /// </summary>
        /// <param name="type">le type de page à ouvrir</param>
        /// <param name="parameter">le paramètre  à apssé</param>
        public void NavigateFrame(Type type, Object parameter)
        {
            Frame.Navigate(type,parameter);
        }
        #endregion

        #region évènements
        private void MainMenuRadioButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
            CheckSecButtonVisible();
        }

        private void HomeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(AcceuilView));
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?PFN=" + Package.Current.Id.FamilyName));
        }

        private async void BugsButton_Click(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=" + ContexteStatic.Support + "&subject=Bugs ou suggestions pour " + ContexteStatic.NomAppli);
            await Launcher.LaunchUriAsync(mailto);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(AboutView));
        }

        private void EcheancierRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(EcheancierView));
        }

        private void ParamsRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(ParametresView));
        }
        
        private void CategRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(GestionCategorieView));
        }
        
        private void StatsRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(StatistiquesView));
        }


        private void OutilsRadioButton_OnClickRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(OutilsView));
        }
        
        #endregion

        #region gestion des paramètres (couleur)

        /// <summary>
        /// Change la couleur du shell à partir de l'id de la couleur
        /// </summary>
        public void ChangeCouleurShell()
        {
            MainSplitView.PaneBackground = ContexteAppli.SetColorTheme(ContexteStatic.ListeCouleur[App.IdCouleurBackground]);
        }

        /// <summary>
        /// Retourne la couleur de thème à appliquer
        /// </summary>
        /// <returns></returns>
        public SolidColorBrush GetCouleur()
        {
            return ContexteAppli.SetColorTheme(ContexteStatic.ListeCouleur[App.IdCouleurBackground]);
        }
        #endregion

        private void CheckSecButtonVisible()
        {
            if (!MainSplitView.IsPaneOpen || (MainSplitView.IsPaneOpen && Width < 750))
            {
                RateButton.Visibility = Visibility.Collapsed;
                BugsButton.Visibility = Visibility.Collapsed;
                AboutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                RateButton.Visibility = Visibility.Visible;
                BugsButton.Visibility = Visibility.Visible;
                AboutButton.Visibility = Visibility.Visible;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckSecButtonVisible();
        }
    }
}
