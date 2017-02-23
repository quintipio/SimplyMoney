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
    /// Classe business communiquant avec la bdd pour les mouvements
    /// </summary>
    public class MouvementBusiness : AbstractBusiness
    {

        #region lien avec le roaming
        /// <summary>
        /// Recalcul le solde de chaques comptes
        /// </summary>
        /// <param name="listeIdMouvement">la liste des mouvements pour le recalcul</param>
        /// <returns>la task</returns>
        public async Task RecalculSoldesComptes(List<int> listeIdMouvement)
        {
            foreach (var idMouvement in listeIdMouvement)
            {
                var mouvement = await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == idMouvement).FirstOrDefaultAsync();
                var compte = (await Bdd.Connection.Table<Compte>().Where(x => x.Id == mouvement.IdCompte).FirstOrDefaultAsync());

                if (mouvement.Credit > 0)
                {
                    compte.Solde += mouvement.Credit;
                }
                else
                {
                    compte.Solde -= mouvement.Debit;
                }

                await Bdd.UpdateDonnee(compte);
                await RoamingCompteBusiness.ModifierCompte(compte);
            }
        }

        /// <summary>
        /// Sauvegarde en base un mouvement provenant du roaming
        /// </summary>
        /// <param name="mouvement">le mouvement à sauvegarder</param>
        /// <returns>l'id du nouveau mouvement sinon 0</returns>
        public async Task<int> SaveMouvementFmRoaming(Mouvement mouvement)
        {
            //verifie si l'id comtpe et l'id du virement existe
            if (await Bdd.Connection.Table<Compte>().Where(x => x.Id == mouvement.IdCompte).CountAsync() > 0)
            {
                var compteVirement = (await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == mouvement.IdMouvementVirement).FirstOrDefaultAsync());

                if ((mouvement.IdMouvementVirement != 0 && compteVirement != null && (await Bdd.Connection.Table<Compte>().Where(x => x.Id == compteVirement.IdCompte).CountAsync() > 0))
                    || mouvement.IdMouvementVirement == 0)
                    {
                        //ajout du mouvement
                        var mouvementId = await Bdd.Connection.Table<Mouvement>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                        var id = 1;
                        if (mouvementId != null)
                        {
                            id = mouvementId.Id + 1;
                        }
                        mouvement.Id = id;
                        await Bdd.AjouterDonnee(mouvement);
                        return id;
                    }
            }
            return 0;
        }

        /// <summary>
        /// Retourne tout les mouvements en base
        /// </summary>
        /// <returns>la liste des mouvements</returns>
        public async Task<List<Mouvement>> GetListeMouvement()
        {
            return await Bdd.Connection.Table<Mouvement>().ToListAsync();
        }

        /// <summary>
        /// Associe deux mouvements pour en faire un virement
        /// </summary>
        /// <param name="idMouvA">le premier id</param>
        /// <param name="idMouvB">le deuxième id</param>
        /// <returns>la task</returns>
        public async Task AssocierMouvementVirement(int idMouvA, int idMouvB)
        {
            var mouvementA = await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == idMouvA).FirstOrDefaultAsync();
            var mouvementB = await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == idMouvB).FirstOrDefaultAsync();

            mouvementA.IdMouvementVirement = mouvementB.Id;
            mouvementB.IdMouvementVirement = mouvementA.Id;

            await Bdd.UpdateDonnee(mouvementA);
            await Bdd.UpdateDonnee(mouvementB);
        }

        #endregion

        /// <summary>
        /// Ajoute un mouvement en base, ou le met à jour si déjà existant
        /// </summary>
        /// <param name="mouvement">le mouvement à ajouter avec l'id à 0 pour ajout, sinon l'id véritable</param>
        /// <returns>l'id du mouvement ajouter/updater</returns>
        public async Task<int> SaveMouvement(Mouvement mouvement)
        {
            //si c'est un nouveau mouvement creation
            if (mouvement.Id == 0)
            {
                var mouvementId =
                    await Bdd.Connection.Table<Mouvement>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                var id = 1;
                if (mouvementId != null)
                {
                    id = mouvementId.Id + 1;
                }
                mouvement.Id = id;
                mouvement.IsPasse = false;
                await Bdd.AjouterDonnee(mouvement);
                await CalculerSolde(mouvement, null);
                return id;
            }
            else
            {
                var oldMouvement = await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == mouvement.Id).FirstOrDefaultAsync();
                
                //si le mouvement à metre à jour est un ancien virement
                if (mouvement.IdMouvementVirement > 0)
                {
                    var mouvementToDelete = await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == mouvement.IdMouvementVirement).FirstOrDefaultAsync();
                    await Bdd.DeleteDonnee(mouvementToDelete);
                    mouvement.IdMouvementVirement = 0;
                    await CalculerSolde(null, mouvementToDelete);
                }

                await Bdd.UpdateDonnee(mouvement);
                await CalculerSolde(mouvement, oldMouvement);
                return mouvement.Id;
            }
        }


        /// <summary>
        /// Retourne une liste de mouvement
        /// </summary>
        /// <param name="idCompte">l'id du compte dont on cherche les mouvements</param>
        /// <param name="page">info pour la limit</param>
        /// <param name="nbOccurences">info pour le nombre d'occurences à retourner</param>
        /// <param name="dateLimiteSoldeCompte">la date limite de récupération des mouvements</param>
        /// <returns>la liste de mouvements</returns>
        public async Task<List<Mouvement>> GetListeMouvement(int idCompte,int page, int nbOccurences,DateTime dateLimiteSoldeCompte)
        {
            var devise =DeviseUtils.GetDiminutifDevise((await Bdd.Connection.Table<Compte>().Where(x => x.Id == idCompte).FirstOrDefaultAsync()).IdDevise);
            var retour = await Bdd.Connection.Table<Mouvement>().Where(x => x.IdCompte == idCompte && x.Date <= dateLimiteSoldeCompte).OrderBy(x => x.Date).Skip((page-1)*nbOccurences).Take(nbOccurences).ToListAsync();

            foreach (var mouv in retour)
            {
                mouv.MouvementChiffre = ((mouv.Credit > 0) ? "+" + mouv.Credit : "-" + mouv.Debit) + " " + devise;
                mouv.Type =
                    ContexteAppli.ListeCategoriesMouvement.SelectMany(
                        varA =>
                            varA.SousCategorieList.Where(
                                varB => varB.Id == mouv.IdType && varB.IsSousCategPerso == mouv.IsTypePerso))
                        .FirstOrDefault();
            }

            return retour;
        }

        /// <summary>
        /// Retourne une liste de mouvement d'un compte sur une période déterminé
        /// </summary>
        /// <param name="idCompte">l'id du compte dont on cherche les mouvements</param>
        /// <param name="dateMin">la date min des mouvements</param>
        /// <param name="dateMax">la date max des mouvements</param>
        /// <returns>la liste de mouvements</returns>
        public async Task<List<Mouvement>> GetListeMouvement(int idCompte,DateTime dateMin, DateTime dateMax)
        {
                var liste = await  Bdd.Connection.Table<Mouvement>().Where(x => x.IdCompte == idCompte && x.Date >= dateMin && x.Date <= dateMax).ToListAsync();
                return liste;
        }

        /// <summary>
        /// Change l'état passé d'un mouvement en base
        /// </summary>
        /// <param name="idMouvement">l'id du mouvement à modifier</param>
        /// <param name="isPasse">l'état passé</param>
        /// <returns>la task</returns>
        public async Task ChangePasseMouvement(int idMouvement, bool isPasse)
        {
            var mouvement =
                await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == idMouvement).FirstOrDefaultAsync();
            mouvement.IsPasse = isPasse;
            await Bdd.UpdateDonnee(mouvement);
        }

        /// <summary>
        /// Recalcul la solde d'un compte après l'ajout ou la modification d'un mouvement
        /// </summary>
        /// <param name="mouvement">le mouvement (null pour une suppresion de mouvement)</param>
        /// <param name="oldMouvement">l'ancien mouvement (null si ajout)</param>
        /// <returns>la task</returns>
        private async Task CalculerSolde(Mouvement mouvement, Mouvement oldMouvement)
        {
            var idCompte = mouvement?.IdCompte ?? oldMouvement.IdCompte;
            var compte = (await Bdd.Connection.Table<Compte>().Where(x => x.Id == idCompte).FirstOrDefaultAsync());

            //si c'est une modification, on efface la prise en compte de l'ancienne valeur
            if (oldMouvement != null)
            {
                if (oldMouvement.Credit > 0)
                {
                    compte.Solde -= oldMouvement.Credit;
                }
                else
                {
                    compte.Solde += oldMouvement.Debit;
                }
            }

            if (mouvement != null)
            {
                //ajout de la nouvelle valeur
                if (mouvement.Credit > 0)
                {
                    compte.Solde += mouvement.Credit;
                }
                else
                {
                    compte.Solde -= mouvement.Debit;
                }
            }

            await Bdd.UpdateDonnee(compte);
            await RoamingCompteBusiness.ModifierCompte(compte);
        }

        /// <summary>
        /// Supprime un mouvement du compte
        /// </summary>
        /// <param name="idMouvement">l'id du mouvement à supprimer</param>
        /// <returns>la task</returns>
        public async Task SupprimerMouvement(int idMouvement)
        {
            var mouvement =
                await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == idMouvement).FirstOrDefaultAsync();
            await Bdd.DeleteDonnee(mouvement);
            await CalculerSolde(null, mouvement);

            //supression en cas de virement
            if (mouvement.IdMouvementVirement > 0)
            {
                var mouvementB =
                    await
                        Bdd.Connection.Table<Mouvement>()
                            .Where(x => x.Id == mouvement.IdMouvementVirement)
                            .FirstOrDefaultAsync();
                await Bdd.DeleteDonnee(mouvementB);
                await CalculerSolde(null, mouvementB);
            }
        }


        /// <summary>
        /// Sauvegarde un virement
        /// </summary>
        /// <param name="mouvement">le mouvement d'origine du virement</param>
        /// <param name="idCompte">l'id du compte sur lequel s'effectue sur le virement</param>
        /// <returns>la task</returns>
        public async Task SaveVirement(Mouvement mouvement, int idCompte)
        {
            var create = mouvement.Id == 0;
            Mouvement mouvementA;
            Mouvement mouvementB;

            //mise à jour
            if (!create)
            {
                var mouvementAOld =
                    await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == mouvement.Id).FirstOrDefaultAsync();
                var mouvementBOld =
                    await
                        Bdd.Connection.Table<Mouvement>()
                            .Where(x => x.Id == mouvementAOld.IdMouvementVirement)
                            .FirstOrDefaultAsync();

                var idDeviseA =
                    (await
                        Bdd.Connection.Table<Compte>().Where(x => x.Id == mouvementAOld.IdCompte).FirstOrDefaultAsync())
                        .IdDevise;

                var idDeviseB =
                    (await
                        Bdd.Connection.Table<Compte>().Where(x => x.Id == idCompte).FirstOrDefaultAsync())
                        .IdDevise;

                await Bdd.DeleteDonnee(mouvementBOld);
                await CalculerSolde(null, mouvementBOld);

                mouvementA = mouvement;

                mouvementB = new Mouvement
                {
                    Id = 0,
                    Commentaire = mouvementA.Commentaire,
                    Credit = DeviseUtils.ConvertisseurMonnaie(idDeviseA,idDeviseB,mouvementA.Debit),
                    Debit = DeviseUtils.ConvertisseurMonnaie(idDeviseA, idDeviseB, mouvementA.Credit),
                    Date = mouvementA.Date,
                    IdCompte = idCompte,
                    ModeMouvement = mouvementA.ModeMouvement,
                    Numero = mouvementA.Numero,
                    IdType = mouvementA.IdType,
                    IsTypePerso = mouvementA.IsTypePerso
                };

                var idMax = 0;
                var residR = (await Bdd.Connection.Table<Mouvement>().OrderByDescending(x => x.Id).FirstOrDefaultAsync());
                if (residR != null)
                {
                    idMax = residR.Id;
                }

                var mouvementBId = idMax + 1;
                mouvementA.IdMouvementVirement = mouvementBId;

                mouvementB.Id = mouvementBId;
                mouvementB.IdMouvementVirement = mouvementA.Id;

                await Bdd.UpdateDonnee(mouvementA);
                await CalculerSolde(mouvementA, mouvementAOld);
                
                await Bdd.AjouterDonnee(mouvementB);
                await CalculerSolde(mouvementB, null);
            }
            else //création
            {
                mouvementA = mouvement;

                var idDeviseA =
                    (await
                        Bdd.Connection.Table<Compte>().Where(x => x.Id == mouvementA.IdCompte).FirstOrDefaultAsync())
                        .IdDevise;

                var idDeviseB =
                    (await
                        Bdd.Connection.Table<Compte>().Where(x => x.Id == idCompte).FirstOrDefaultAsync())
                        .IdDevise;

                mouvementB = new Mouvement
                {
                    Id = 0,
                    Commentaire = mouvementA.Commentaire,
                    Credit = DeviseUtils.ConvertisseurMonnaie(idDeviseA, idDeviseB, mouvementA.Debit),
                    Debit = DeviseUtils.ConvertisseurMonnaie(idDeviseA, idDeviseB, mouvementA.Credit),
                    Date = mouvementA.Date,
                    IdCompte = idCompte,
                    ModeMouvement = mouvementA.ModeMouvement,
                    Numero = mouvementA.Numero,
                    IdType = mouvementA.IdType,
                    IsTypePerso = mouvementA.IsTypePerso
                };

                var idMax = 0;
                var residR = (await Bdd.Connection.Table<Mouvement>().OrderByDescending(x => x.Id).FirstOrDefaultAsync());
                if (residR != null)
                {
                    idMax = residR.Id;
                }

                var mouvementAId = idMax + 1;
                var mouvementBId = idMax + 2;

                mouvementA.Id = mouvementAId;
                mouvementA.IdMouvementVirement = mouvementBId;

                mouvementB.Id = mouvementBId;
                mouvementB.IdMouvementVirement = mouvementAId;

                await Bdd.AjouterDonnee(mouvementA);
                await CalculerSolde(mouvementA, null);

                await Bdd.AjouterDonnee(mouvementB);
                await CalculerSolde(mouvementB, null);
            }
        }

        /// <summary>
        /// Retourne le numéro de compte d'un mouvement
        /// </summary>
        /// <param name="idMouvement">l'id du mouvement</param>
        /// <returns>le compte</returns>
        public async Task<int> GetIdCompteMouvementLieVirement(int idMouvement)
        {
            return idMouvement > 0 ? (await Bdd.Connection.Table<Mouvement>().Where(x => x.Id == idMouvement).FirstOrDefaultAsync()).IdCompte : 0;
        }

        /// <summary>
        /// Ajoute un mouvement provenant de l'outil de restauration
        /// </summary>
        /// <param name="mouvement">le mouvement à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterMouvementFmRestauration(Mouvement mouvement)
        {
            await Bdd.AjouterDonnee(mouvement);
        }
    }
}
