
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// Propriété du viewmodel des échéanciers
    /// </summary>
    public partial class EcheancierViewModel
    {
        #region liste des échéanciers

        private ObservableCollection<Echeancier> _listeEcheancier;

        public ObservableCollection<Echeancier> ListeEcheancier
        {
            get { return _listeEcheancier; }

            set
            {
                _listeEcheancier = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Edition de l'échéancier

        //Hidden
        private int? IdEcheancierSelect { get; set; }
        
        private bool _isModif;

        public bool IsModif
        {
            get { return _isModif; }

            set
            {
                _isModif = value;
                OnPropertyChanged();
            }
        }

        //PARTIE ECHEANCIER
        public DateTime DateEcheancier { get; set; }

        public DateTime DateLimiteEcheancier { get; set; }

        public bool IsDateLimite { get; set; }


        private EnumModel _selectedPeriodicite;

        public EnumModel SelectedPeriodicite
        {
            get { return _selectedPeriodicite; }

            set
            {
                _selectedPeriodicite = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EnumModel> _listePeriodicite;

        public ObservableCollection<EnumModel> ListePeriodicite
        {
            get { return _listePeriodicite; }

            set
            {
                _listePeriodicite = value;
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

        private CollectionViewSource _listeCompteEcheancier;

        public CollectionViewSource ListeCompteEcheancier
        {
            get { return _listeCompteEcheancier; }

            set
            {
                _listeCompteEcheancier = value;
                OnPropertyChanged();
            }
        }

        private int _nbJours;

        public int NbJours
        {
            get { return _nbJours; }

            set
            {
                _nbJours = value;
                OnPropertyChanged();
            }
        }

        //PARTIE MOUVEMENT
        private double? _debit;

        public double? Debit
        {
            get { return _debit; }

            set
            {
                _debit = value;
                if (value != null)
                {
                    Credit = null;
                }
                OnPropertyChanged();
            }
        }

        private double? _credit;

        public double? Credit
        {
            get { return _credit; }

            set
            {
                _credit = value;
                if (value != null)
                {
                    Debit = null;
                }
                OnPropertyChanged();
            }
        }
        
        private string _commentaire;

        public string Commentaire
        {
            get { return _commentaire; }

            set
            {
                _commentaire = value;
                OnPropertyChanged();
            }
        }

        private string _selectedVirementCompteString;

        public string SelectedVirementCompteString
        {
            get { return _selectedVirementCompteString; }

            set
            {
                _selectedVirementCompteString = value;
                OnPropertyChanged();
            }
        }

        private Compte _selectedCompteVirement;

        public Compte SelectedCompteVirement
        {
            get { return _selectedCompteVirement; }

            set
            {
                _selectedCompteVirement = value;
                SelectedVirementCompteString = (value != null) ? value.Nom : "";
            }
        }

        private CollectionViewSource _listeCompteVirement;

        public CollectionViewSource ListeCompteVirement
        {
            get { return _listeCompteVirement; }

            set
            {
                _listeCompteVirement = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSousCategorieString;

        public string SelectedSousCategorieString
        {
            get { return _selectedSousCategorieString; }

            set
            {
                _selectedSousCategorieString = value;
                OnPropertyChanged();
            }
        }

        private SousCategorie _selectedCategorieFmList;

        public SousCategorie SelectedCategorieFmList
        {
            get { return _selectedCategorieFmList; }

            set
            {
                _selectedCategorieFmList = value;
                SelectedSousCategorieString = (value != null) ? value.ToString() : "";
            }
        }


        private CollectionViewSource _listeCategorie;

        public CollectionViewSource ListeCategorie
        {
            get { return _listeCategorie; }

            set
            {
                _listeCategorie = value;
                OnPropertyChanged();
            }
        }

        private TypeMouvement _selectedTypeMouvement;

        public TypeMouvement SelectedTypeMouvement
        {
            get { return _selectedTypeMouvement; }

            set
            {
                _selectedTypeMouvement = value;
                GridVirementVisible = IsGridVirementVisible(_selectedTypeMouvement);
            }
        }

        private ObservableCollection<TypeMouvement> _typeMouvementListe;

        public ObservableCollection<TypeMouvement> TypeMouvementListe
        {
            get { return _typeMouvementListe; }

            set
            {
                _typeMouvementListe = value;
                OnPropertyChanged();
            }
        }
        

        private bool _gridVirementVisible;

        public bool GridVirementVisible
        {
            get { return _gridVirementVisible; }

            set
            {
                _gridVirementVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
