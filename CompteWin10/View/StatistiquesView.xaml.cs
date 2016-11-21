using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CompteWin10.Interface;
using CompteWin10.Model;
using CompteWin10.ViewModel;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace CompteWin10.View
{
    /// <summary>
    /// View pour les stats
    /// </summary>
    public sealed partial class StatistiquesView : IView<StatsViewModel>
    {
        private bool _isLoad;

        private int _selectedChart;

        /// <summary>
        /// ViewModel
        /// </summary>
        public StatsViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public StatistiquesView()
        {
            InitializeComponent();
            _isLoad = false;
            _selectedChart = 0;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitreGrid.Background = App.AppShell.GetCouleur();
            ViewModel = new StatsViewModel();
            await ViewModel.Initialization;
            _isLoad = true;
        }

        private void ButtonCourbeDepense_Click(object sender, RoutedEventArgs e)
        {
            ChoixStatsGrid.Visibility = Visibility.Collapsed;
            DepenseCourbeGrid.Visibility = Visibility.Visible;
            CategDepenseGrid.Visibility = Visibility.Collapsed;

            ViewModel.SelectedCompte = ViewModel.ListeCompte[0];
            ViewModel.SelectedPeriode = ViewModel.PeriodeListe[0];
            ViewModel.SelectedCategorie = ViewModel.CategorieListe[0];

            _selectedChart = 1;

           // await GenererGraphiqueCourbeDepense();
        }

        private void ButtonDepenseCateg_Click(object sender, RoutedEventArgs e)
        {
            ChoixStatsGrid.Visibility = Visibility.Collapsed;
            DepenseCourbeGrid.Visibility = Visibility.Collapsed;
            CategDepenseGrid.Visibility = Visibility.Visible;

            ViewModel.SelectedCompte = ViewModel.ListeCompte[0];
            ViewModel.SelectedPeriode = ViewModel.PeriodeListe[0];
            ViewModel.SelectedCategorie = ViewModel.CategorieListe[0];

            _selectedChart = 2;

            //await GenererStatsCategDepense();
        }

        private async Task GenererGraphiqueCourbeDepense()
        {
            LineChart.Series.Clear();
            if (ViewModel.SelectedCompte.Id == 0)
            {
                ViewModel.Titre = "";

                var data = new Dictionary<string, List<Mouvement>>();
                var i = 0;
                double max = 0;
                double min = 0;
                foreach (var compte in ViewModel.ListeCompte.Where(compte => compte.Id > 0))
                {
                    var liste = await ViewModel.GetStatsDepenseCourbeCompte(compte.Id);
                    data.Clear();
                    if (liste.Count > 0)
                    {
                        data.Add(i + " - " + compte.Nom, liste);
                        var maxTmp = liste.Max(x => x.Chiffre) + 10;
                        if (maxTmp > max)
                        {
                            max = maxTmp;
                        }

                        var minTmp = liste.Min(x => x.Chiffre) - 10;
                        if (minTmp < min)
                        {
                            min = minTmp;
                        }

                        i++;
                    }

                    foreach (var item in data)
                    {
                        LineChart.Series.Add(new LineSeries
                        {
                            ItemsSource = item.Value,
                            Title = item.Key,
                            IndependentValuePath = "Date",
                            DependentValuePath = "Chiffre",
                            DependentRangeAxis = new LinearAxis { Minimum = min, Maximum = max, Orientation = AxisOrientation.Y },
                            IsSelectionEnabled = true
                        });
                    }
                }
            }
            else
            {
                ViewModel.Titre = ViewModel.SelectedCompte.Nom;
                var item = await ViewModel.GetStatsDepenseCourbeCompte(ViewModel.SelectedCompte.Id);
                if (item.Count > 0)
                {
                    var max = item.Max(x => x.Chiffre) + 10;
                    var min = item.Min(x => x.Chiffre) + 10;
                    LineChart.Series.Add(new LineSeries
                    {
                        ItemsSource = item,
                        Title = ViewModel.SelectedCompte.Nom,
                        IndependentValuePath = "Date",
                        DependentValuePath = "Chiffre",
                        DependentRangeAxis = new LinearAxis { Minimum = min, Maximum = max, Orientation = AxisOrientation.Y },
                        IsSelectionEnabled = true
                    });
                }
            }
        }


        private async Task GenererStatsCategDepense()
        {
            PieChart.Series.Clear();
            var liste = await ViewModel.GetStatsDepenseCategorie();
            if (liste.Count > 0)
            {
                PieChart.Series.Add(new PieSeries
                {
                    ItemsSource = liste,
                    Title = ViewModel.SelectedCategorie.Libelle,
                    IndependentValuePath = "Categ",
                    DependentValuePath = "Value",
                });
            }
        }


        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var compte = ((ComboBox) sender).SelectedItem as Compte;
                ViewModel.SelectedCompte = compte;

                switch (_selectedChart)
                {
                    case 1:
                        await GenererGraphiqueCourbeDepense();
                        break;

                    case 2:
                        await GenererStatsCategDepense();
                        break;

                }
            }

            
        }

        private async void SelectorPeriode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var enumData = ((ComboBox)sender).SelectedItem as EnumModel;
                ViewModel.SelectedPeriode = enumData;

                switch (_selectedChart)
                {
                    case 1:
                        await GenererGraphiqueCourbeDepense();
                        break;

                    case 2:
                        await GenererStatsCategDepense();
                        break;

                }
            }
        }

        private async void SelectorCateg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoad)
            {
                var enumData = ((ComboBox)sender).SelectedItem as Categorie;
                ViewModel.SelectedCategorie = enumData;

                await GenererStatsCategDepense();
            }
        }
    }
}
