using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using CompteWin10.Strings;

namespace CompteWin10.ViewModel
{
 
    /// <summary>
    /// Properties des paramètres
    /// </summary>
    public partial class ParametresViewModel
    {
        private ObservableCollection<ListeLangues.LanguesStruct> _listeLangues;

        /// <summary>
        /// la liste des langues disponibles
        /// </summary>
        public ObservableCollection<ListeLangues.LanguesStruct> ListeLangues
        {
            get { return _listeLangues; }

            set
            {
                _listeLangues = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SolidColorBrush> _listeCouleurs;

        /// <summary>
        /// la liste des couleurs disponibles
        /// </summary>
        public ObservableCollection<SolidColorBrush> ListeCouleurs
        {
            get { return _listeCouleurs; }

            set
            {
                _listeCouleurs = value;
                OnPropertyChanged();
            }
        }

        private bool _isRoamingCategorieActive;

        public bool IsRoamingCategorieActive
        {
            get { return _isRoamingCategorieActive; }

            set
            {
                _isRoamingCategorieActive = value;
                OnPropertyChanged();
            }
        }

        private bool _isRoamingEcheancierActive;

        public bool IsRoamingEcheancierActive
        {
            get { return _isRoamingEcheancierActive; }

            set
            {
                _isRoamingEcheancierActive = value;
                OnPropertyChanged();
            }
        }
    }
}
