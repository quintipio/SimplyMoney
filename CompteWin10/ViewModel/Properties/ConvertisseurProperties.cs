using System.Collections.ObjectModel;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// Properties du convertisseur
    /// </summary>
    public partial class ConvertisseurViewModel
    {

        private ObservableCollection<Devise> _listeDeviseA;

        public ObservableCollection<Devise> ListeDeviseA
        {
            get { return _listeDeviseA; }

            set
            {
                _listeDeviseA = value;
                OnPropertyChanged();
            }
        }

        private Devise _selectedDeviseA;

        public Devise SelectedDeviseA
        {
            get { return _selectedDeviseA; }

            set
            {
                _selectedDeviseA = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Devise> _listeDeviseB;

        public ObservableCollection<Devise> ListeDeviseB
        {
            get { return _listeDeviseB; }

            set
            {
                _listeDeviseB = value;
                OnPropertyChanged();
            }
        }

        private Devise _selectedDeviseB;

        public Devise SelectedDeviseB
        {
            get { return _selectedDeviseB; }

            set
            {
                _selectedDeviseB = value;
                OnPropertyChanged();
            }
        }

        private double? _monnaieA;

        public double? MonnaieA
        {
            get { return _monnaieA; }

            set
            {
                _monnaieA = value;
                OnPropertyChanged();
            }
        }

        private double? _monnaieB;

        public double? MonnaieB
        {
            get { return _monnaieB; }

            set
            {
                _monnaieB = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pays> _listePaysA;

        public ObservableCollection<Pays> ListePaysA
        {
            get { return _listePaysA; }

            set
            {
                _listePaysA = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pays> _listePaysB;

        public ObservableCollection<Pays> ListePaysB
        {
            get { return _listePaysB; }

            set
            {
                _listePaysB = value;
                OnPropertyChanged();
            }
        }

        private Pays _selectedPaysA;

        public Pays SelectedPaysA
        {
            get { return _selectedPaysA; }

            set
            {
                _selectedPaysA = value;
                OnPropertyChanged();
            }
        }

        private Pays _selectedPaysB;

        public Pays SelectedPaysB
        {
            get { return _selectedPaysB; }

            set
            {
                _selectedPaysB = value;
                OnPropertyChanged();
            }
        }
    }
}
