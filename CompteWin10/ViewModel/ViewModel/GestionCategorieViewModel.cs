using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
 /// ViewModel de la page de gestion des catégories
 /// </summary>
    public partial class GestionCategorieViewModel : AbstractViewModel
    {
        private CategorieBusiness _categorieBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public GestionCategorieViewModel()
        {
            Initialization = InitializeAsync();
        }

        public sealed async override Task InitializeAsync()
        {
           _categorieBusiness = new CategorieBusiness();
           await _categorieBusiness.Initialization;
            
            GenererListeCategories();
        }

        /// <summary>
        /// regénère la liste des catégories
        /// </summary>
        public void GenererListeCategories()
        {
           ListeCateg = new List<Categorie>(ContexteAppli.ListeCategoriesMouvement);
            
            foreach (var category in ListeCateg)
            {
                category.IsVisibleForModif = category.IsCategPerso && App.ModeApp == AppareilEnum.ModeAppareilPrincipal;

                foreach (var sousCategory in category.SousCategorieList)
                {
                    sousCategory.IsVisibleForModif = sousCategory.IsSousCategPerso && App.ModeApp == AppareilEnum.ModeAppareilPrincipal;
                }
            }
        }

        /// <summary>
        /// Annuler une modif ou un ajout
        /// </summary>
        public void AnnulerModifAjout()
        {
            ModeOuverture = ModeOuvertureGestionCategorieEnum.Aucun;
            LibelleSelected = null;
            SelectedCategorie = null;
            SelectedSousCategorie = null;
        }

        /// <summary>
        /// Vérifie les données avant validation
        /// </summary>
        /// <returns>les erreurs à affiché sinon une chaine vide</returns>
        private string Validate()
        {
            string retour = "";

            if (string.IsNullOrWhiteSpace(LibelleSelected))
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("ChampsVide") + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(LibelleSelected) && LibelleSelected.Length > 50)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("NomTropLong") + "\r\n";
            }

            return retour;
        }

        /// <summary>
        /// Sauvegarde la modification ou l'ajout de la donnée
        /// </summary>
        /// <returns>les erreurs sinon une chaine vide</returns>
        public async Task<string> Save()
        {
            var retour = Validate();
            if (string.IsNullOrWhiteSpace(retour))
            {
                LibelleSelected = StringUtils.FirstLetterUpper(LibelleSelected);
                switch (ModeOuverture)
                {
                    case ModeOuvertureGestionCategorieEnum.OuvertureAjouterCategorie:
                        await _categorieBusiness.AddCategorie(LibelleSelected);
                        break;

                     case ModeOuvertureGestionCategorieEnum.OuvertureAjouterSousCategorie:
                        await
                            _categorieBusiness.AddSousCategorie(LibelleSelected, SelectedCategorie.Id,
                                SelectedCategorie.IsCategPerso);
                        break;

                    case ModeOuvertureGestionCategorieEnum.OuvertureModifierCategorie:
                        SelectedCategorie.Libelle = LibelleSelected;
                        await _categorieBusiness.ModifierCategorie(SelectedCategorie);
                        break;

                    case ModeOuvertureGestionCategorieEnum.OuvertureModifierSousCategorie:
                        SelectedSousCategorie.Libelle = LibelleSelected;
                        await _categorieBusiness.ModifierSousCategorie(SelectedSousCategorie);
                        break;
                }
                await ContexteAppli.GenerateCategorieMouvement();
                GenererListeCategories();
                AnnulerModifAjout();
            }

            return retour;
        }

        /// <summary>
        /// Vérifie si la suppression d'une catégorie est possible
        /// </summary>
        /// <returns>true si supprimable</returns>
        public async Task<bool> CheckSuppressionCategorie()
        {
            return await _categorieBusiness.CheckSupressionCategorie(SelectedCategorie.Id);
        }

        /// <summary>
        /// Supprime une catégorie
        /// </summary>
        /// <returns>la task</returns>
        public async Task SuppressionCategorie()
        {
            await _categorieBusiness.DeleteCategorie(SelectedCategorie.Id);
            await ContexteAppli.GenerateCategorieMouvement();
            GenererListeCategories();
        }

        /// <summary>
        /// Supprime une sous catégorie
        /// </summary>
        /// <returns>la task</returns>
        public async Task SuppressionSousCategorie()
        {
            await _categorieBusiness.DeleteSousCategorie(SelectedSousCategorie.Id);
            await ContexteAppli.GenerateCategorieMouvement();
            GenererListeCategories();
        }

        /// <summary>
        /// Vérifie si la suppression d'une sous catégorie est possible
        /// </summary>
        /// <returns>true si supprimable</returns>
        public async Task<bool> CheckSuppressionSousCategorie()
        {
            return await _categorieBusiness.CheckSupressionSousCategorie(SelectedSousCategorie.Id);
        }

        /// <summary>
        /// Masque ou démasque une sous catégorie
        /// </summary>
        /// <param name="sousCateg">la sous catégorie dont on souhaite changer l'état</param>
        /// <param name="toHide">true si elle doit être masqué</param>
        /// <returns></returns>
        public async Task HideUnhideSousCateg(SousCategorie sousCateg, bool toHide)
        {
            //met à jour la sous catégorie
            foreach (var category in ContexteAppli.ListeCategoriesMouvement)
            {
                foreach (var sousCategory in category.SousCategorieList)
                {
                    if (sousCategory.Id == sousCateg.Id)
                    {
                        sousCategory.IsHidden = toHide;
                    }
                }
            }
            
            //recupère tout les id à masquer
            var newListeId = "";
            foreach (var category in ContexteAppli.ListeCategoriesMouvement)
            {
                foreach (var sousCategory in category.SousCategorieList)
                {
                    if (sousCategory.IsHidden)
                    {
                        newListeId += sousCategory.Id + ",";
                    }
                }
            }

            if (newListeId.EndsWith(","))
            {
                newListeId = newListeId.Remove(newListeId.Length - 1, 1);
            }

            //met à jour la liste en base
            await _categorieBusiness.ChangeVisibleSousCateg(newListeId);

            //met à jour l'affichage
            GenererListeCategories();
        }
    }
}
