using System.Collections.Generic;
using CompteWin10.Model;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// Properties de la page principale
    /// </summary>
    public partial class AcceuilViewModel
    {
        private List<Banque> _listeBanque;

        /// <summary>
        /// La liste des banques avec pour chacune les comptes et les soldes
        /// </summary>
        public List<Banque> ListeBanque
        {
            get { return _listeBanque; }
            set
            {
                _listeBanque = value;
                OnPropertyChanged();
            }
        }

        private bool _visible;

        /// <summary>
        /// Visibilité de certains boutons
        /// </summary>
        public bool Visible
        {
            get { return _visible; }

            set
            {
                _visible = value;
                OnPropertyChanged();
            }
        }

        private string _soldeTotal;

        /// <summary>
        /// Le solde de toute les banques
        /// </summary>
        public string SoldeTotal
        {
            get { return _soldeTotal; }

            set
            {
                _soldeTotal = value;
                OnPropertyChanged();
            }
        }
    }
}
