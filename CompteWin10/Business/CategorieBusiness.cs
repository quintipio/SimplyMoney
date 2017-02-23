using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompteWin10.Abstract;
using CompteWin10.Context;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Utils;

namespace CompteWin10.Business
{
    /// <summary>
    /// Classe de communication avec la base de donnée pour les catégories et sous catégories
    /// </summary>
    public class CategorieBusiness : AbstractBusiness
    {

        /// <summary>
        /// Retourne toute les catégories créer par l'utilisateur
        /// </summary>
        /// <returns>la liste des catégories</returns>
        public async Task<List<Categorie>> GetCategoriePerso()
        {
            var liste = await Bdd.Connection.Table<Categorie>().OrderBy(x =>x.Libelle).ToListAsync();
            foreach (var category in liste)
            {
                category.IsCategPerso = true;
            }
            return liste;
        }

        /// <summary>
        /// retourne la liste des sous catégories créer par l'utilisateur
        /// </summary>
        /// <returns>la liste des sous catégories</returns>
        public async Task<List<SousCategorie>> GetSousCategoriesPerso()
        {
            var liste = await Bdd.Connection.Table<SousCategorie>().OrderBy(x => x.Libelle).ToListAsync();
            foreach (var category in liste)
            {
                category.IsSousCategPerso = true;
            }
            return liste;
        }

        /// <summary>
        /// Ajoute une catégorie en base
        /// </summary>
        /// <param name="libelle">le libelle de la catégorie à ajouter</param>
        /// <returns>la catégorie créer</returns>
        public async Task<Categorie> AddCategorie(string libelle)
        {
            var categorie = new Categorie();
            var categId = await Bdd.Connection.Table<Categorie>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (categId != null)
            {
                id = categId.Id + 1;
            }
            categorie.Id = id;
            categorie.IsCategPerso = true;
            categorie.Libelle = libelle;
            categorie.SousCategorieList = new List<SousCategorie>();

            await Bdd.AjouterDonnee(categorie);
            await RoamingCategorieBusiness.AjouterCategorie(categorie);
            return categorie;
        }

        /// <summary>
        /// Créer une sous catégorie perso
        /// </summary>
        /// <param name="libelle">le libelle de la sous catégorie</param>
        /// <param name="idCategorie">l'id de la catégorie mère</param>
        /// <param name="isCategoriePerso">si la catégorie mère est perso ou non</param>
        /// <returns>la sous catégorie de créer</returns>
        public async Task<SousCategorie> AddSousCategorie(string libelle, int idCategorie, bool isCategoriePerso)
        {
            var sousCategorie = new SousCategorie();
            var sousCategId = await Bdd.Connection.Table<SousCategorie>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (sousCategId != null)
            {
                id = sousCategId.Id + 1;
            }
            sousCategorie.Id = id;
            sousCategorie.Libelle = libelle;
            sousCategorie.IdCategorie = idCategorie;
            sousCategorie.IsCategPerso = isCategoriePerso;
            sousCategorie.IsSousCategPerso = true;

            await Bdd.AjouterDonnee(sousCategorie);
            await RoamingCategorieBusiness.AjouterSousCategorie(sousCategorie);
            return sousCategorie;
        }

        /// <summary>
        /// Modifie une catégorie en base
        /// </summary>
        /// <param name="categorie">la catégorie à modifier</param>
        /// <returns>la task</returns>
        public async Task ModifierCategorie(Categorie categorie)
        {
            var categToModif = await Bdd.Connection.Table<Categorie>().Where(x => x.Id == categorie.Id).FirstOrDefaultAsync();
            categToModif.Libelle = categorie.Libelle;
            await Bdd.UpdateDonnee(categToModif);
            await RoamingCategorieBusiness.ModifierCategorie(categToModif);
        }

        /// <summary>
        /// modifie une sous categorie
        /// </summary>
        /// <param name="sousCategorie"> l'id de la sous catégorie à modifier</param>
        /// <returns>la task</returns>
        public async Task ModifierSousCategorie(SousCategorie sousCategorie)
        {
            var sousCategToModif =
                await Bdd.Connection.Table<SousCategorie>().Where(x => x.Id == sousCategorie.Id).FirstOrDefaultAsync();

            sousCategToModif.IdCategorie = sousCategorie.IdCategorie;
            sousCategToModif.Libelle = sousCategorie.Libelle;

            await Bdd.UpdateDonnee(sousCategToModif);
            await RoamingCategorieBusiness.ModifierSousCategorie(sousCategToModif);
        }

        /// <summary>
        /// efface une categorie
        /// </summary>
        /// <param name="idCategorie">la catégorie à effacer</param>
        /// <returns></returns>
        public async Task DeleteCategorie(int idCategorie)
        {
            var categ = await Bdd.Connection.Table<Categorie>().Where(x => x.Id == idCategorie).FirstOrDefaultAsync();
            var listeSousCateg =  await Bdd.Connection.Table<SousCategorie>().Where(x => x.IdCategorie == idCategorie).ToListAsync();
            await Bdd.DeleteDonnee(categ);

            foreach (var sousCategory in listeSousCateg)
            {
                await DeleteSousCategorie(sousCategory.Id);
            }

            await RoamingCategorieBusiness.SupprimerCategorie(categ);
        }

        /// <summary>
        /// Efface une sous categorie
        /// </summary>
        /// <param name="idSousCategorie">l'id de la sous categorie</param>
        /// <returns></returns>
        public async Task DeleteSousCategorie(int idSousCategorie)
        {
            var sousCateg = await Bdd.Connection.Table<SousCategorie>().Where(x => x.Id == idSousCategorie).FirstOrDefaultAsync();
            await Bdd.DeleteDonnee(sousCateg);
            await RoamingCategorieBusiness.SupprimerSousCategorie(sousCateg);
        }

        /// <summary>
        /// Vérifie si la supression d'une catégorie est possible ou non (utilisé dans les mouvements ou non)
        /// </summary>
        /// <param name="idCategorie">l'id de la catégorie à supprimer</param>
        /// <returns>true si supprimable</returns>
        public async Task<bool> CheckSupressionCategorie(int idCategorie)
        {
            var retour = true;
            var listeSousCateg = await Bdd.Connection.Table<SousCategorie>().Where(x => x.IdCategorie == idCategorie).ToListAsync();
            foreach (var sousCategory in listeSousCateg)
            {
                var res = await CheckSupressionSousCategorie(sousCategory.Id);
                if (!res)
                {
                    retour = false;
                }

            }
            return retour;
        }

        /// <summary>
        /// Vérifie si la supression d'une sous catégorie est possible ou non (utilisé dans les mouvements ou non)
        /// </summary>
        /// <param name="idSousCategorie">l'id de la sous catégorie à supprimer</param>
        /// <returns>true si supprimable</returns>
        public async Task<bool> CheckSupressionSousCategorie(int idSousCategorie)
        {
          var compteurA =  await
                Bdd.Connection.Table<Mouvement>()
                    .Where(x => x.IdType == idSousCategorie && x.IsTypePerso)
                    .CountAsync();

            var compteurB = await
                Bdd.Connection.Table<Echeancier>()
                    .Where(x => x.IdType == idSousCategorie && x.IsTypePerso)
                    .CountAsync();

            return (!(compteurA > 0) && !(compteurB >  0));
        }



        /// <summary>
        /// Retourne une map des mouvements pour chaque sous catégorie d'une catégorie
        /// </summary>
        /// <param name="idCategorie">l'id de la catégorie dont on cherche les mouvements </param>
        /// <param name="isCategPerso">indique si il s'agit d'une categorie perso ou non</param>
        /// <param name="dateMin">la date min du filtre</param>
        /// <param name="dateMax">la date max du filtre</param>
        /// <param name="idCompte">l'id du compte pour un filtre (0 si aucun)</param>
        /// <returns>un dictionnaire contenant en key la sous catégorie, et en value les mouvements associés</returns>
        public async Task<Dictionary<SousCategorie, List<Mouvement>>> GetMouvementByCategorie(int idCategorie, bool isCategPerso, DateTime dateMin,
            DateTime dateMax, int idCompte)
        {
            var retour = new Dictionary<SousCategorie,List<Mouvement>>();
            var listeSousCateg =
                ContexteAppli.ListeCategoriesMouvement.FirstOrDefault(
                    x => x.Id == idCategorie && x.IsCategPerso == isCategPerso).SousCategorieList;

            foreach (var sousCateg in listeSousCateg)
            {
                List<Mouvement> listeMouvement;
                if (idCompte > 0)
                {
                    listeMouvement =
                        await
                            Bdd.Connection.Table<Mouvement>()
                                .Where(
                                    x =>
                                        x.IdType == sousCateg.Id && x.IsTypePerso == sousCateg.IsSousCategPerso &&
                                        x.Date > dateMin && x.Date < dateMax && x.IdCompte == idCompte)
                                .ToListAsync();
                }
                else
                {
                    listeMouvement =
                        await
                            Bdd.Connection.Table<Mouvement>()
                                .Where(
                                    x =>
                                        x.IdType == sousCateg.Id && x.IsTypePerso == sousCateg.IsSousCategPerso &&
                                        x.Date > dateMin && x.Date < dateMax)
                                .ToListAsync();
                }

                retour.Add(sousCateg,listeMouvement);
            }
            return retour;
        }


        /// <summary>
        /// Retourne une map des mouvements pour chaque catégorie
        /// </summary>
        /// <param name="dateMin">la date min du filtre</param>
        /// <param name="dateMax">la date max du filtre</param>
        /// <param name="idCompte">l'id du compte pour un filtre (0 si aucun)</param>
        /// <returns>un dictionnaire contenant en key la sous catégorie, et en value les mouvements associés</returns>
        public async Task<Dictionary<Categorie, List<Mouvement>>> GetMouvementByCategorie(DateTime dateMin,
            DateTime dateMax, int idCompte)
        {
            var retour = new Dictionary<Categorie, List<Mouvement>>();

            foreach (var category in ContexteAppli.ListeCategoriesMouvement)
            {
                var listeMouvementCategory = new List<Mouvement>();

                var listeSousCateg =
               ContexteAppli.ListeCategoriesMouvement.FirstOrDefault(
                   x => x.Id == category.Id && x.IsCategPerso == category.IsCategPerso).SousCategorieList;

                foreach (var sousCateg in listeSousCateg)
                {
                    if (idCompte > 0)
                    {
                        listeMouvementCategory.AddRange(
                            await
                                Bdd.Connection.Table<Mouvement>()
                                    .Where(
                                        x =>
                                            x.IdType == sousCateg.Id && x.IsTypePerso == sousCateg.IsSousCategPerso &&
                                            x.Date > dateMin && x.Date < dateMax && x.IdCompte == idCompte)
                                    .ToListAsync());
                    }
                    else
                    {
                        listeMouvementCategory.AddRange(
                            await
                                Bdd.Connection.Table<Mouvement>()
                                    .Where(
                                        x =>
                                            x.IdType == sousCateg.Id && x.IsTypePerso == sousCateg.IsSousCategPerso &&
                                            x.Date > dateMin && x.Date < dateMax)
                                    .ToListAsync());
                    }
                }
                retour.Add(category, listeMouvementCategory);
            }
            return retour;
        }

        /// <summary>
        /// Ajoute une catégorie provenant de l'outil s de restauration
        /// </summary>
        /// <param name="categorie">la catégorie à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterCategorieFmRestauration(Categorie categorie)
        {
            categorie.SousCategorieList = new List<SousCategorie>();
            categorie.IsCategPerso = true;
            await Bdd.AjouterDonnee(categorie);
            await RoamingCategorieBusiness.AjouterCategorie(categorie);
        }

        /// <summary>
        /// Ajoute une sous catégorie provenant de l'outil s de restauration
        /// </summary>
        /// <param name="sousCategorie">la sous catégorie à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterSousCategorieFmRestauration(SousCategorie sousCategorie)
        {
            sousCategorie.IsSousCategPerso = true;
            await Bdd.AjouterDonnee(sousCategorie);
            await RoamingCategorieBusiness.AjouterSousCategorie(sousCategorie);
        }
        
        /// <summary>
        /// Retourne la liste des sous catégories à masquer
        /// </summary>
        /// <returns>une liste d'identifiants</returns>
        public async Task<List<int>> GetListeSousCategToHide()
        {
            var data = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            if (data != null)
            {
                var listeId = StringUtils.ConvertStringToList(data.ListeSousCategHidden);
                if (listeId != null)
                {
                    return listeId;
                }
            }
            return null;
        }

        /// <summary>
        /// Change la liste des sous catégories masquées
        /// </summary>
        /// <param name="listeId">la nouvelle liste des sous catégories</param>
        /// <returns></returns>
        public async Task ChangeVisibleSousCateg(string listeId)
        {
            var data = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            data.ListeSousCategHidden = listeId;
            await Bdd.UpdateDonnee(data);
        }
    }
}
