using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using CompteWin10.Abstract;
using CompteWin10.Model;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel du covnertisseur
    /// </summary>
    public partial class ConvertisseurViewModel : AbstractViewModel
    {

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConvertisseurViewModel()
        {
            Initialization = InitializeAsync();
        }
        public override async sealed Task InitializeAsync()
        {
            ListePaysA = new ObservableCollection<Pays>(DeviseUtils.ListePays);
            ListePaysB = new ObservableCollection<Pays>(DeviseUtils.ListePays);
            SelectedPaysA = DeviseUtils.GetPays("FR");
            SelectedPaysB = DeviseUtils.GetPays("FR");
            ListeDeviseA = new ObservableCollection<Devise>(SelectedPaysA.Devises);
            ListeDeviseB = new ObservableCollection<Devise>(SelectedPaysB.Devises);
            SelectedDeviseA = SelectedPaysA.Devises[0];
            SelectedDeviseB = SelectedPaysA.Devises[0];
            MonnaieA = 0.0;
            MonnaieB = 0.0;
        }

        /// <summary>
        /// Change les devises lors de la sélection du pays A
        /// </summary>
        /// <param name="pays">le pays sélectionné</param>
        public void ChangePaysA(Pays pays)
        {
            ListeDeviseA = new ObservableCollection<Devise>(pays.Devises);
            SelectedDeviseA = ListeDeviseA[0];
        }

        /// <summary>
        /// Change les devises lors de la sélection du pays B
        /// </summary>
        /// <param name="pays">le pays sélectionné</param>
        public void ChangePaysB(Pays pays)
        {
            ListeDeviseB = new ObservableCollection<Devise>(pays.Devises);
            SelectedDeviseB = ListeDeviseB[0];
        }

        /// <summary>
        /// Converti les propriétés mmonaieA en monnaieB avec les devises de sélectionnées
        /// </summary>
        public void Convert()
        {
            if (MonnaieA != null)
            {
                MonnaieB = DeviseUtils.ConvertisseurMonnaie(SelectedDeviseA.Id, SelectedDeviseB.Id, MonnaieA.Value);
            }
        }
    }
}
