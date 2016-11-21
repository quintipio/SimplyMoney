using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CompteWin10.Interface;
using CompteWin10.Model;
using CompteWin10.ViewModel;

namespace CompteWin10.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConvertisseurView : IView<ConvertisseurViewModel>
    {
        public ConvertisseurViewModel ViewModel { get; set; }

        private bool isLoad;

        /// <summary>
        /// Constucteur
        /// </summary>
        public ConvertisseurView()
        {
            InitializeComponent();
            isLoad = false;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();
            ViewModel = new ConvertisseurViewModel();
            await ViewModel.Initialization;
            isLoad = true;
        }

        private void PaysA_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (isLoad)
            {
                ViewModel.ChangePaysA(ComboBoxPaysA.SelectedItem as Pays);
                ComboBoxDeviseA.SelectedIndex = 0;
            }
        }

        private void DeviseA_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (isLoad)
            {
                ViewModel.SelectedDeviseA = ComboBoxDeviseA.SelectedItem as Devise;
            }
        }


        private void PaysB_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (isLoad)
            {
                ViewModel.ChangePaysB(ComboBoxPaysB.SelectedItem as Pays);
                ComboBoxDeviseB.SelectedIndex = 0;
            }
        }

        private void DeviseB_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (isLoad)
            {
                ViewModel.SelectedDeviseB = ComboBoxDeviseB.SelectedItem as Devise;
            }
        }
        

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ComboBoxPaysA.SelectedValue = ViewModel.SelectedPaysA;
            ComboBoxPaysB.SelectedValue = ViewModel.SelectedPaysB;
            ComboBoxDeviseA.SelectedIndex = 0;
            ComboBoxDeviseB.SelectedIndex = 0;
        }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            ViewModel.Convert();
            ((Button)sender).IsEnabled = true;
        }

    }
}
