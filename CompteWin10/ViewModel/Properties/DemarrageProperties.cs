
using System.Collections.ObjectModel;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// Properties de la page de démarrage
    /// </summary>
    public partial class DemarrageViewModel
    {

        private string _nomBanque;
        private string _nomCompte;
        private double? _soldeInitial;


        public string NomBanque
        {
            get
            {
                return _nomBanque;
            }
            set
            {
                _nomBanque = value;
                OnPropertyChanged();
            }
        }

        public string NomCompte
        {
            get
            {
                return _nomCompte;
            }
            set
            {
                _nomCompte = value;
                OnPropertyChanged();
            }
        }

        public double? SoldeInitialCompte
        {
            get
            {
                return _soldeInitial;
            }
            set
            {
                _soldeInitial = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pays> _listePays;

        public ObservableCollection<Pays> ListePays
        {
            get { return _listePays; }

            set
            {
                _listePays = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Devise> _listeDevise;

        public ObservableCollection<Devise> ListeDevise
        {
            get { return _listeDevise; }

            set
            {
                _listeDevise = value;
                OnPropertyChanged();
            }
        }

        private Pays _selectedPays;

        public Pays SelectedPays
        {
            get { return _selectedPays; }

            set
            {
                _selectedPays = value;
                if (value != null)
                {
                    ListeDevise = new ObservableCollection<Devise>(value.Devises);
                }
                OnPropertyChanged();
            }
        }

        private Devise _selectedDevise;

        public Devise SelectedDevise
        {
            get { return _selectedDevise; }

            set
            {
                _selectedDevise = value;
                OnPropertyChanged();
            }
        }
    }
}
