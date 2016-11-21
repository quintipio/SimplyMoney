using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Context;
using CompteWin10.Model;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de l'échancier
    /// </summary>
    public partial class EcheancierViewModel : AbstractViewModel
    {
        private EcheancierBusiness _echeancierBusiness;
        private BanqueBusiness _banqueBusiness;
        private CompteBusiness _compteBusiness;


        private List<Banque> _listeCompte;

        /// <summary>
        /// Constructeur
        /// </summary>
        public EcheancierViewModel()
        {
            Initialization = InitializeAsync();
        }

        public sealed async override Task InitializeAsync()
        {
            _echeancierBusiness = new EcheancierBusiness();
            await _echeancierBusiness.Initialization;
            _banqueBusiness = new BanqueBusiness();
            await _banqueBusiness.Initialization;
            _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            await ChargerEcheancier();

            ListeCategorie = new CollectionViewSource { IsSourceGrouped = true };
            ListeCompteVirement = new CollectionViewSource { IsSourceGrouped = true };
            ListeCompteEcheancier = new CollectionViewSource {IsSourceGrouped = true };
            GenerateCategories();
            GenereMouvement();
            GenerePeriodicite();
            await GenereComptes();
            
            GridVirementVisible = false;
            DateEcheancier = DateUtils.GetMaintenant();
            DateLimiteEcheancier = DateUtils.GetMaintenant();
            SelectedTypeMouvement = TypeMouvementListe[0];
            SelectedPeriodicite = ListePeriodicite[0];
        }

        #region Chargement

        /// <summary>
        /// Charge la liste des échéancier
        /// </summary>
        /// <returns>la task</returns>
        private async Task ChargerEcheancier()
        {
            ListeEcheancier = new ObservableCollection<Echeancier>(await _echeancierBusiness.GetEcheancier());
        }

        /// <summary>
        /// Genere une liste de categories
        /// </summary>
        private void GenerateCategories()
        {
            ListeCategorie.Source = ContexteAppli.GenerateCategoriesGroup();
        }

        /// <summary>
        /// Genere les mouvement de compte à afficher
        /// </summary>
        private void GenereMouvement()
        {
            TypeMouvementListe = new ObservableCollection<TypeMouvement>
            {
                new TypeMouvement(1, ResourceLoader.GetForCurrentView().GetString("CarteBleue")),
                new TypeMouvement(2, ResourceLoader.GetForCurrentView().GetString("Cheque")),
                new TypeMouvement(3, ResourceLoader.GetForCurrentView().GetString("Virement")),
                new TypeMouvement(4, ResourceLoader.GetForCurrentView().GetString("Prelevement"))
            };
        }

        /// <summary>
        /// Génère la liste des périodicités
        /// </summary>
        private void GenerePeriodicite()
        {
            ListePeriodicite = new ObservableCollection<EnumModel>
            {
                new EnumModel(1, ResourceLoader.GetForCurrentView().GetString("AnnuelText")),
                new EnumModel(2, ResourceLoader.GetForCurrentView().GetString("BimestrielText")),
                new EnumModel(3, ResourceLoader.GetForCurrentView().GetString("TrimestrielText")),
                new EnumModel(4, ResourceLoader.GetForCurrentView().GetString("MensuelText")),
                new EnumModel(5, ResourceLoader.GetForCurrentView().GetString("HebdomadaireText")),
                new EnumModel(6, ResourceLoader.GetForCurrentView().GetString("QuotidienText")),
                new EnumModel(7, ResourceLoader.GetForCurrentView().GetString("PersonaliseText")),
            };
        }


        /// <summary>
        /// Carge la liste des compte pour un virement
        /// </summary>
        /// <returns>la task</returns>
        private async Task GenereComptes()
        {
            var tmp = new List<GroupInfoList<Compte>>();
            _listeCompte = await _banqueBusiness.GetListeBanqueCompteVirement(0);

            var groupeb = new GroupInfoList<Compte> { Key = ResourceLoader.GetForCurrentView().GetString("AucunText") };
            groupeb.Add(new Compte { Id = 0, Nom = ResourceLoader.GetForCurrentView().GetString("AucunText") });
            tmp.Add(groupeb);

            foreach (var banque in _listeCompte)
            {
                var groupe = new GroupInfoList<Compte> { Key = banque.Nom };
                groupe.AddRange(banque.ListeCompte);
                if (groupe.Count > 0)
                {
                    tmp.Add(groupe);
                }
            }

            ListeCompteVirement.Source = tmp;
            ListeCompteEcheancier.Source = tmp;
        }

        /// <summary>
        /// Retourne la position de l'item sélectionné dans la combobox des périodes
        /// </summary>
        /// <returns>l'index</returns>
        public int IndexSelectedItemPeriodiciteEcheancier()
        {
            var index = 0;
            foreach (var mouv in ListePeriodicite)
            {
                if (mouv.Id == SelectedPeriodicite.Id)
                {
                    return index;
                }
                index++;
            }
            return index--;
        }

        /// <summary>
        /// Retourne la position de l'item sélectionné dans la combobox des mouvements
        /// </summary>
        /// <returns>l'index</returns>
        public int IndexSelectedItemTypeMouvementEcheancier()
        {
            var index = 0;
            foreach (var mouv in TypeMouvementListe)
            {
                if (mouv.Id == SelectedTypeMouvement.Id)
                {
                    return index;
                }
                index++;
            }
            return index;
        }

        #endregion


        #region Gestion des mouvements


        /// <summary>
        /// Modidie un mouvement (prépare les champs
        /// </summary>
        /// <param name="echeancier">l'échancier à modifié </param>
        public async Task ModifierEcheancier(Echeancier echeancier)
        {
            IsModif = true;
            IdEcheancierSelect = echeancier.Id;
            
            //Info des dates
            SelectedCompte = await _compteBusiness.GetCompte(echeancier.IdCompte);
            DateLimiteEcheancier = echeancier.DateLimite;
            IsDateLimite = echeancier.IsDateLimite;
            DateEcheancier = echeancier.Date;
            NbJours = echeancier.NbJours;
            SelectedPeriodicite = ListePeriodicite[echeancier.IdPeriodicite - 1];

            //Info du mouvement
            Credit = echeancier.Credit;
            if (echeancier.Debit != 0)
            {
                Debit = echeancier.Debit;
            }
            else
            {
                Debit = null;
            }

            SelectedTypeMouvement = TypeMouvementListe[echeancier.ModeMouvement - 1];
            SelectedCategorieFmList = ContexteAppli.ListeCategoriesMouvement.SelectMany(varA => varA.SousCategorieList.Where(varB => varB.Id == echeancier.IdType && varB.IsSousCategPerso == echeancier.IsTypePerso)).FirstOrDefault();
            SelectedCompteVirement = (echeancier.IdCompteVirement > 0)
                ? await _compteBusiness.GetCompte(echeancier.IdCompteVirement):null;
            Commentaire = echeancier.Commentaire;
        }


        /// <summary>
        /// Efface les champs lors du clic sur le bouton Annuler
        /// </summary>
        public void AnnulerEcheancier()
        {
            IsModif = false;
            IdEcheancierSelect = null;
            SelectedCompte = null;
            DateEcheancier = DateUtils.GetMaintenant();
            IsDateLimite = false;
            DateLimiteEcheancier = DateUtils.GetMaintenant();
            NbJours = 0;


            Credit = null;
            Debit = null;
            Commentaire = null;
            SelectedCategorieFmList = null;
            SelectedTypeMouvement = TypeMouvementListe[0];
            SelectedCompteVirement = null;
        }


        /// <summary>
        /// Sauvegarde en base un nouveau mouvement
        /// </summary>
        public async Task<string> SaveEcheancier()
        {
            var retour = Validate();

            if (string.IsNullOrEmpty(retour))
            {
                //on remet en positif si un nombre est négatif
                if (Credit != null && Credit != 0 && Credit < 0)
                {
                    Credit *= -1;
                }

                if (Debit != null && Debit != 0 && Debit < 0)
                {
                    Debit *= -1;
                }

                //ajout du mouvement
                var echeancier = new Echeancier
                {
                    Id = IdEcheancierSelect ?? 0,
                    IdCompte = SelectedCompte.Id,
                    Date = DateEcheancier,
                    DateLimite = DateLimiteEcheancier,
                    IsDateLimite = IsDateLimite,
                    NbJours = NbJours,
                    IdPeriodicite = SelectedPeriodicite.Id,
                    Commentaire = Commentaire,
                    Credit = Credit ?? 0,
                    Debit = Debit ?? 0,
                    ModeMouvement = SelectedTypeMouvement.Id,
                    IdType = SelectedCategorieFmList?.Id ?? 0,
                    IsTypePerso = SelectedCategorieFmList?.IsSousCategPerso ?? false,
                    IdCompteVirement = SelectedCompteVirement?.Id ?? 0
                };
                await _echeancierBusiness.SaveEcheancier(echeancier);

                await ChargerEcheancier();
                AnnulerEcheancier();
            }
            return retour;
        }


        private string Validate()
        {
            var retour = "";
            if (SelectedPeriodicite.Id == 7 && NbJours <= 0)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("PeriodicitePerso")+"\r\n";
            }

            if (IsDateLimite && DateLimiteEcheancier <= DateEcheancier)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("DateLimieteEcheancier") + "\r\n";
            }

            if (SelectedCompte == null || (SelectedCompte != null && SelectedCompte.Id == 0))
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("AucunCompteSelect");
            }
            return retour;
        }

        /// <summary>
        /// Supprime un échéancier de la base
        /// </summary>
        /// <returns>la task</returns>
        public async Task SupprimerEcheancier()
        {
            if (IdEcheancierSelect != null)
            {
                    await _echeancierBusiness.SupprimerEcheancier(IdEcheancierSelect.Value);
                
                await ChargerEcheancier();
                AnnulerEcheancier();
            }
        }

        #endregion


        /// <summary>
        /// Indique si la grid des virements doit être visible
        /// </summary>
        /// <param name="categorie">la catégorie séletionné</param>
        /// <returns>true si à afficher</returns>
        private bool IsGridVirementVisible(TypeMouvement categorie)
        {
            return categorie.Id == 3;
        }
    }
}
