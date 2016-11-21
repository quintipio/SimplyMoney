
using Windows.Storage;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// propriétés pour l'import et export
    /// </summary>
    public partial class SauvegardeRestaurationViewModel
    {
        private StorageFile _fichier;

        public StorageFile Fichier
        {
            get { return _fichier; }

            set
            {
                _fichier = value;
                OnPropertyChanged();
            }
        }

        private bool _isMdp;

        public bool IsMdp
        {
            get { return _isMdp; }

            set
            {
                _isMdp = value;
                OnPropertyChanged();
            }
        }

        private string _motDePasse;

        public string MotDePasse
        {
            get { return _motDePasse; }

            set
            {
                _motDePasse = value;
                OnPropertyChanged();
            }
        }

    }
}
