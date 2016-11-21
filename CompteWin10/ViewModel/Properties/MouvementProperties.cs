using System;
using System.Collections.ObjectModel;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml.Data;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// Properties de la vue des mouvements
    /// </summary>
    public partial class MouvementViewModel
    {
        #region Liste des mouvements

        private int NombrePages { get; set; }

        private int PageEnCours { get; set; }




        private bool _isPreviousEnabled;

        public bool IsPreviousEnabled
        {
            get { return _isPreviousEnabled; }

            set
            {
                _isPreviousEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isNextEnabled;

        public bool IsNextEnabled
        {
            get { return _isNextEnabled; }

            set
            {
                _isNextEnabled = value;
                OnPropertyChanged();
            }
        }

        private Compte _compte;

        public Compte Compte
        {
            get { return _compte; }

            private set
            {
                _compte = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Mouvement> _listeMouvements;

        public ObservableCollection<Mouvement> ListeMouvements
        {
            get { return _listeMouvements; }

            set
            {
                _listeMouvements = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Edition du mouvement
        private int? IdMouvementSelect { get; set; }

        private int? IdMouvementVirement { get; set; }

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

        public DateTime DateMouvement { get; set; }


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

        private int _numero;

        public int Numero
        {
            get { return _numero; }

            set
            {
                _numero = value;
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
                SelectedSousCategorieString = (value != null)?value.ToString():"";
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
                GridNumeroVisible = IsGridNumeroVisible(_selectedTypeMouvement);
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

        private bool _gridNumeroVisible;

        public bool GridNumeroVisible
        {
            get { return _gridNumeroVisible; }

            set
            {
                _gridNumeroVisible = value;
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
