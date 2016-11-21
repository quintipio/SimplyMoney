using System.Collections.ObjectModel;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// properties du viewModel de la gestion des comptes et des banques
    /// </summary>
    public partial class GestionCompteBanqueViewModel
    {
        private int _idElement;

        public int IdElement
        {
            get { return _idElement; }

            set
            {
                _idElement = value;
                OnPropertyChanged();
            }
        }


        private string _nom;
        
        public string Nom
        {
            get { return _nom; }

            set
            {
                _nom = value;
                OnPropertyChanged();
            }
        }

        
        private int _idParent;

        public int IdParent
        {
            get { return _idParent; }

            set
            {
                _idParent = value;
                OnPropertyChanged();
            }
        }

        private double? _solde;

        public double? Solde
        {
            get { return _solde; }

            set
            {
                _solde = value;
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
