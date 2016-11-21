using Windows.UI.Xaml.Controls;
using CompteWin10.Context;

namespace CompteWin10.View
{
    /// <summary>
    /// Code Behind de la vue APPD
    /// </summary>
    public sealed partial class AboutView : Page
    {
        public AboutView()
        {
            InitializeComponent();

            TitreText.Text = ContexteStatic.NomAppli;
            Developpeur.Text = ContexteStatic.Developpeur;
            Version.Text = ContexteStatic.Version;
            TitreGrid.Background = App.AppShell.GetCouleur();
        }
    }
}
