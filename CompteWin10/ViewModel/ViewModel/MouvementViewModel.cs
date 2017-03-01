using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de la vues des mouvements
    /// </summary>
    public partial class MouvementViewModel : AbstractViewModel
    {
        

        private MouvementBusiness _mouvementBusiness;
        private CompteBusiness _compteBusiness;
        private BanqueBusiness _banqueBusiness;
        private EcheancierBusiness _echeancierBusiness;

        private List<Banque> _listeCompte;
        public List<Mouvement> ListeRajoutMouvement;

        private const int NbOccurencesMax = 100;
        public DateTime dateEnCours;

        #region init
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="compte">le compte ouvert</param>
        public MouvementViewModel(Compte compte)
        {
            Compte = compte;
            Initialization = InitializeAsync();
            ListeRajoutMouvement = new List<Mouvement>();
        }


        public sealed override async Task InitializeAsync()
        {
            _mouvementBusiness = new MouvementBusiness();
            await _mouvementBusiness.Initialization;

            _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            _banqueBusiness = new BanqueBusiness();
            await _banqueBusiness.Initialization;

            _echeancierBusiness = new EcheancierBusiness();
            await _echeancierBusiness.Initialization;

            //PARTIE LISTE MOUVEMENT
            IsDateSoldeCompteVisible = (App.ModeApp == AppareilEnum.ModeAppareilPrincipal);
            DateSoldeCompte = DateUtils.GetMaintenant();
            await RecompterPage(null);

            //PARTIE GESTION MOUVEMENT
            //préparation des données
            ListeCategorie = new CollectionViewSource { IsSourceGrouped = true };
            ListeCompteVirement = new CollectionViewSource {IsSourceGrouped = true};
            GenerateCategories();
            GenereMouvement();
            await GenereComptesVirement();

            GridNumeroVisible = false;
            GridVirementVisible = false;
            DateMouvement =DateUtils.GetMaintenant();
            SelectedTypeMouvement = TypeMouvementListe[0];
        }

        #endregion


        #region pour remplacer le databinding (bugs)

        /// <summary>
        /// Retourne la position de l'item sélectionné dans la combobox des mouvements
        /// </summary>
        /// <returns>l'index</returns>
        public int IndexSelectedItemMouvement()
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


        /// <summary>
        /// indique si la grid de numéro doit être visible
        /// </summary>
        /// <param name="categorie">la catégorie sélectionné dans le mouvement</param>
        /// <returns>true si visible</returns>
        private bool IsGridNumeroVisible(TypeMouvement categorie)
        {
            return (categorie.Id == 2 || categorie.Id == 3);
        }

        /// <summary>
        /// Indique si la grid des virements doit être visible
        /// </summary>
        /// <param name="categorie">la catégorie séletionné</param>
        /// <returns>true si à afficher</returns>
        private bool IsGridVirementVisible(TypeMouvement categorie)
        {
            return categorie.Id == 3;
        }

        #endregion


        #region Chargement
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
        /// Carge la liste des compte pour un virement
        /// </summary>
        /// <returns>la task</returns>
        private async Task GenereComptesVirement()
        {
            var tmp = new List<GroupInfoList<Compte>>();
            _listeCompte = (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
                ? await _banqueBusiness.GetListeBanqueCompteVirement(Compte.Id)
                : await RoamingCompteBusiness.GetListeBanqueCompteVirement(Compte.Id);

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
        }


        /// <summary>
        /// Et à jour le solde du compte affiché
        /// </summary>
        /// <returns></returns>
        public async Task UpdateSoldeCompte()
        {
            if (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
            {
                Compte = await _compteBusiness.GetCompte(Compte.Id);
            }
            else
            {
                Compte = await RoamingCompteBusiness.GetCompte(Compte.Id);
            }
        }

        #endregion


        #region ListeMouvement

        /// <summary>
        /// Met à jour le solde du compte en fonction de la date entrée
        /// </summary>
        /// <returns></returns>
        public async Task UpdateDateSoldeCompte()
        {
            //si on fait des prévisions dans le futur on doit prendre en compte les échéanciers
            if (DateSoldeCompte > DateTime.Today)
            {
                double rajouterAuSolde = 0;
                var listeEcheancier = await _echeancierBusiness.GetEcheancier();
                ListeRajoutMouvement = new List<Mouvement>();
                var dateFictive = DateTime.Today;
                do
                {
                    if (listeEcheancier.Count(x => x.Date == dateFictive) > 0)
                    {
                        foreach (var echeancier in listeEcheancier.Where(x => x.Date == dateFictive))
                        {
                            if ((echeancier.IsDateLimite && echeancier.DateLimite < dateFictive.AddDays(1)) ||
                                !echeancier.IsDateLimite)
                            {
                                var mouv = new Mouvement
                                {
                                    Date = new DateTime(dateFictive.Year, dateFictive.Month, dateFictive.Day, 0, 0, 0),
                                    Credit = echeancier.Credit,
                                    Debit = echeancier.Debit,
                                    Commentaire = echeancier.Commentaire,
                                    IdCompte = echeancier.IdCompte,
                                    IdType = echeancier.IdType,
                                    IsPasse = false,
                                    IdMouvementVirement = 0,
                                    IsTypePerso = echeancier.IsTypePerso,
                                    Numero = 0,
                                    ModeMouvement = echeancier.ModeMouvement,
                                    Lock = true
                                };
                                ListeRajoutMouvement.Add(mouv);
                                if (mouv.Debit > 0)
                                {
                                    rajouterAuSolde -= mouv.Debit;
                                }
                                if (mouv.Credit > 0)
                                {
                                    rajouterAuSolde += mouv.Credit;
                                }

                                echeancier.Date =
                                    EcheancierBusiness.GetNbJoursPeriodicite(
                                        (PeriodiciteEnum) echeancier.IdPeriodicite, echeancier.Date, echeancier.NbJours);
                            }
                        }
                    }

                    dateFictive = dateFictive.AddDays(1);
                } while (dateFictive <= DateSoldeCompte);

                await RecompterPage(ListeRajoutMouvement);
                await RecalculerSoldeCompte(rajouterAuSolde);
            }
            else
            {
                ListeRajoutMouvement.Clear();
                await RecompterPage(null);
                await RecalculerSoldeCompte(null);
            }
        }

        /// <summary>
        /// Recalcul le solde d'un compte à partir d'un date précise
        /// </summary>
        /// <param name="rajout">le solde à rajouter</param>
        /// <returns></returns>
        public async Task RecalculerSoldeCompte(double? rajout)
        {
            Compte.Solde = await _compteBusiness.GetSoldeCompteDate(DateSoldeCompte, Compte.Id);
            if (rajout != null)
            {
                Compte.Solde += rajout.Value;
            }
            Compte = new Compte(Compte);
        }

        /// <summary>
        /// Compte le nombre de pages possible pour ce compte et ouvre la dernière
        /// </summary>
        /// <param name="listeMouvementRajout">une liste de mouvement à rajouter en plus de ceux en base</param>
        private async Task RecompterPage(List<Mouvement> listeMouvementRajout)
        {
            NombrePages = (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)? await _compteBusiness.GetNombrePageCompte(Compte.Id, NbOccurencesMax,DateSoldeCompte,(listeMouvementRajout != null)? listeMouvementRajout.Count():0):1;
            await ChangePage(NombrePages, false, false, listeMouvementRajout);
        }

        /// <summary>
        /// Permet la navigation entre les pages en chargeant la liste des mouvements d'une page et en modifiant la visibilité des boutons de navigations
        /// </summary>
        /// <param name="page">indique la page sur laquelle naviguer (null si aucune)</param>
        /// <param name="goToPrevious">true pour aller à la page précédente</param>
        /// <param name="goToNext">true pour aller à la page suivante</param>
        /// <param name="listeRajout">une liste de mouvement à rajouter en plus des mouvements en base</param>
        public async Task ChangePage(int? page,bool goToPrevious, bool goToNext,List<Mouvement> listeRajout)
        {
            //Changement de la page page en cours
            if (page != null)
            {
                if (page <= NombrePages && page > 0)
                {
                    PageEnCours = page.Value;
                }
            }
            else
            {
                if (goToPrevious)
                {
                    PageEnCours--;
                }

                if (goToNext)
                {
                    PageEnCours++;
                }
            }

            //Changement de la visibilité
            IsPreviousEnabled = PageEnCours > 1;
            IsNextEnabled = PageEnCours < NombrePages;

            //Chargement de la liste des mouvements
            ListeMouvements = new ObservableCollection<Mouvement>((App.ModeApp == AppareilEnum.ModeAppareilPrincipal) ?
                await _mouvementBusiness.GetListeMouvement(Compte.Id, PageEnCours, NbOccurencesMax,DateSoldeCompte,listeRajout) : await RoamingMouvementBusiness.GetMouvementsRoaming(Compte.Id));

        }

        /// <summary>
        /// Change l'état passé d'un mouvement
        /// </summary>
        /// <param name="mouvement">le mouvement à modifié</param>
        /// <param name="isPasse">true si le mouvement est passé</param>
        /// <returns></returns>
        public async Task ChangePasseMouvement(Mouvement mouvement, bool isPasse)
        {
            if (mouvement != null)
            {
                if (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
                {
                    await _mouvementBusiness.ChangePasseMouvement(mouvement.Id, isPasse);
                }
                else
                {
                    await RoamingMouvementBusiness.ModifierPasseMouvement(mouvement.Id, isPasse);
                }
                ListeMouvements.FirstOrDefault(x => x.Id == mouvement.Id).IsPasse = isPasse;
            }
        }

        #endregion


        #region Gestion des mouvements


        /// <summary>
        /// Modidie un mouvement (prépare les champs
        /// </summary>
        /// <param name="mouvement">le mouvement à modifié </param>
        public async Task ModifierMouvement(Mouvement mouvement)
        {
            IsModif = true;
            IdMouvementSelect = mouvement.Id;
            IdMouvementVirement = mouvement.IdMouvementVirement;
            Credit = mouvement.Credit;
            if (mouvement.Debit != 0)
            {
                Debit = mouvement.Debit;
            }
            else
            {
                Debit = null;
            }
            Numero = mouvement.Numero;
            DateMouvement = mouvement.Date;
            Commentaire = mouvement.Commentaire;
            SelectedTypeMouvement = TypeMouvementListe[mouvement.ModeMouvement - 1];
            SelectedCategorieFmList = ContexteAppli.ListeCategoriesMouvement.SelectMany(varA => varA.SousCategorieList.Where(varB => varB.Id == mouvement.IdType && varB.IsSousCategPerso == mouvement.IsTypePerso)).FirstOrDefault();
            var idCompteMouvb = (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
                ? await _mouvementBusiness.GetIdCompteMouvementLieVirement(mouvement.IdMouvementVirement)
                : await RoamingMouvementBusiness.GetIdCompteMouvementLieVirement(mouvement.IdMouvementVirement);
            SelectedCompteVirement = _listeCompte.SelectMany(banque => banque.ListeCompte.Where(compte => compte.Id == idCompteMouvb)).FirstOrDefault();
        }

        /// <summary>
        /// Efface les champs lros du clic sur le bouton Annuler
        /// </summary>
        public void AnnulerMouvement()
        {
            IsModif = false;
            IdMouvementSelect = null;
            IdMouvementVirement = null;
            Credit = null;
            Debit = null;
            Numero = 0;
            DateMouvement = DateUtils.GetMaintenant();
            SelectedCategorieFmList = null;
            Commentaire = null;
            SelectedTypeMouvement = TypeMouvementListe[0];
        }

        /// <summary>
        /// Sauvegarde en base un nouveau mouvement
        /// </summary>
        public async Task SaveMouvement()
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
                var mouvement = new Mouvement
                {
                    Id = IdMouvementSelect ?? 0,
                    Commentaire = Commentaire,
                    Credit = Credit ?? 0,
                    Debit = Debit ?? 0,
                    Date = DateMouvement,
                    IdCompte = Compte.Id,
                    ModeMouvement = SelectedTypeMouvement.Id,
                    Numero = Numero,
                    IdType = SelectedCategorieFmList?.Id ?? 0,
                    IsTypePerso = SelectedCategorieFmList?.IsSousCategPerso ?? false,
                    IdMouvementVirement = IdMouvementVirement ?? 0,
                };


            if (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
            {
                if (SelectedCompteVirement != null && SelectedCompteVirement.Id != 0 && SelectedTypeMouvement.Id == 3)
                {
                    await _mouvementBusiness.SaveVirement(mouvement, SelectedCompteVirement.Id);
                }
                else
                {
                    await _mouvementBusiness.SaveMouvement(mouvement);
                }
            }
            else
            {
                if (SelectedCompteVirement != null && SelectedCompteVirement.Id != 0 && SelectedTypeMouvement.Id == 3)
                {
                    await RoamingMouvementBusiness.SaveVirement(mouvement, SelectedCompteVirement.Id);
                }
                else
                {
                    if (IdMouvementSelect == null)
                    {
                        await RoamingMouvementBusiness.AjouterMouvementRoaming(mouvement);
                    }
                    else
                    {
                        await RoamingMouvementBusiness.ModifierMouvementRoaming(mouvement);
                    }
                }
            }
            await RecompterPage(null);
            await UpdateSoldeCompte();
            AnnulerMouvement();
        }

        /// <summary>
        /// Supprime un mouvement de la base
        /// </summary>
        /// <returns></returns>
        public async Task SupprimerMouvement()
        {
            if (IdMouvementSelect != null)
            {
                if (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
                {
                    await _mouvementBusiness.SupprimerMouvement(IdMouvementSelect.Value);
                }
                else
                {
                    await RoamingMouvementBusiness.SupprimerMouvementRoaming(IdMouvementSelect.Value);
                }
                await RecompterPage(null);
                await UpdateSoldeCompte();
                AnnulerMouvement();
            }
        }

        #endregion
    }
}
