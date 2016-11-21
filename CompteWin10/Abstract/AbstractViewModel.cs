using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CompteWin10.Interface;

namespace CompteWin10.Abstract
{
    /// <summary>
    /// Classe abstraite des ViewModel
    /// </summary>
    public abstract class AbstractViewModel: INotifyPropertyChanged ,IAsyncInitialization
    {

        /// <summary>
        /// delegate pour l'init du business
        /// </summary>
        public Task Initialization { get; protected set; }

        /// <summary>
        /// pour le InotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Méthode à éxécuter pour initialiser le controleur
        /// </summary>
        /// <returns></returns>
        public abstract Task InitializeAsync();

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
