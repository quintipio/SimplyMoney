using Windows.UI.Xaml;
using CompteWin10.Enum;

namespace CompteWin10.View
{
    /// <summary>
    /// View pour les outils de l'appli
    /// </summary>
    public sealed partial class OutilsView
    {
        private Visibility IsVisible { get; set; }

        public OutilsView()
        {
            InitializeComponent();
            IsVisible = App.ModeApp == AppareilEnum.ModeAppareilSecondaire ? Visibility.Collapsed : Visibility.Visible;
            TitreGrid.Background = App.AppShell.GetCouleur();
        }

        private void OpenExport_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(SauvegardeRestaurationView),2);
        }

        private void OpenImport_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(SauvegardeRestaurationView), 1);
        }

        private void OpenConvertisseur_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(ConvertisseurView));
        }
    }
}
