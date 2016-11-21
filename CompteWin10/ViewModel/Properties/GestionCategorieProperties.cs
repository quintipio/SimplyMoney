
using System.Collections.Generic;
using CompteWin10.Enum;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// propriétés de la page de gestion des catégories
    /// </summary>
    public partial class GestionCategorieViewModel
    {
        private ModeOuvertureGestionCategorieEnum _modeOuverture;

        public ModeOuvertureGestionCategorieEnum ModeOuverture
        {
            get { return _modeOuverture; }

            set
            {
                _modeOuverture = value;
                OnPropertyChanged();
            }
        }


        private List<Categorie> _listeCateg;

        public List<Categorie> ListeCateg
        {
            get { return _listeCateg; }

            set
            {
                _listeCateg = value;
                OnPropertyChanged();
            }
        }

        private bool _visible;

        public bool Visible
        {
            get { return _visible; }

            set
            {
                _visible = value;
                OnPropertyChanged();
            }
        }

        private string _libelleSelected;

        public string LibelleSelected
        {
            get { return _libelleSelected; }

            set
            {
                _libelleSelected = value;
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

        private SousCategorie _selectedSousCategorie;

        public SousCategorie SelectedSousCategorie
        {
            get { return _selectedSousCategorie; }

            set
            {
                _selectedSousCategorie = value;
                OnPropertyChanged();
            }
        }
    }
}
