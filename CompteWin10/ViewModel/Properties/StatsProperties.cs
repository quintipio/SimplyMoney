using System.Collections.ObjectModel;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{

    /// <summary>
    /// Properties pour les stats
    /// </summary>
    public partial class StatsViewModel
    {
        private ObservableCollection<Compte> _listeCompte;

        public ObservableCollection<Compte> ListeCompte
        {
            get { return _listeCompte; }

            set
            {
                _listeCompte = value;
                OnPropertyChanged();
            }
        }

        private EnumModel _selectedPeriode;

        public EnumModel SelectedPeriode
        {
            get { return _selectedPeriode; }

            set
            {
                _selectedPeriode = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EnumModel> _periodeListe;

        public ObservableCollection<EnumModel> PeriodeListe
        {
            get { return _periodeListe; }

            set
            {
                _periodeListe = value;
                OnPropertyChanged();
            }
        }

        private Compte _selectedCompte;

        public Compte SelectedCompte
        {
            get { return _selectedCompte; }

            set
            {
                _selectedCompte = value;
                OnPropertyChanged();
            }
        }

        private Categorie _selectedCategorie;

        public Categorie SelectedCategorie
        {
            get { return _selectedCategorie; }

            set
            {
                _selectedCategorie = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Categorie> _categorieListe;

        public ObservableCollection<Categorie> CategorieListe
        {
            get { return _categorieListe; }

            set
            {
                _categorieListe = value;
                OnPropertyChanged();
            }
        }

        private string _titre;

        public string Titre
        {
            get { return _titre; }

            set
            {
                _titre = value;
                OnPropertyChanged();
            }
        }
    }
}
