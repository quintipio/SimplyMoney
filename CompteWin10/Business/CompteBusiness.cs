using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompteWin10.Abstract;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Utils;

namespace CompteWin10.Business
{
    /// <summary>
    /// Business pour communiquer avec les données liées aux comptes
    /// </summary>
    public class CompteBusiness : AbstractBusiness
    {
        /// <summary>
        /// Indique le nombre de pages pour un compte
        /// </summary>
        /// <param name="idCompte">l'id du comtpe sur lequel s'effectue la recherche</param>
        /// <param name="limit">le nombre d'occurence par pages</param>
        /// <param name="dateLimite">le nombre d'occurence par pages</param>
        /// <param name="nbRajout">le nombre d'occurence par pages</param>
        /// <returns>le nombre de page</returns>
        public async Task<int> GetNombrePageCompte(int idCompte, int limit,DateTime dateLimite,int nbRajout)
        {
            var nboccurences = await Bdd.Connection.Table<Mouvement>().Where(x => x.IdCompte == idCompte && x.Date<=dateLimite).CountAsync();
            nboccurences += nbRajout;
            var res =  nboccurences / limit;
            if(nboccurences%limit >  0)
            {
                res++;
            }
            return res;
        }

        /// <summary>
        /// Retourne un compte de la base à partir de son id
        /// </summary>
        /// <param name="idCompte">l'id du compte</param>
        /// <returns>le compte</returns>
        public async Task<Compte> GetCompte(int idCompte)
        {
            var compte = await Bdd.Connection.Table<Compte>().Where(x => x.Id == idCompte).FirstOrDefaultAsync();
            compte.DeviseToAffiche = DeviseUtils.GetDiminutifDevise(compte.IdDevise);
            compte.Solde = Math.Round(compte.Solde, 2);
            return compte;
        }

        /// <summary>
        /// Retourne le solde d'un compte à une date voulue
        /// </summary>
        /// <param name="date">la date du solde de compte choisi</param>
        /// <param name="idCompte">le compte dont on souhaite la solde</param>
        /// <returns>le solde</returns>
        public async Task<double> GetSoldeCompteDate(DateTime date,int idCompte)
        {
           var solde = await GetOneSolde(idCompte);
                var listeMouvement = await Bdd.Connection.Table<Mouvement>().Where(x => x.IdCompte == idCompte && x.Date >= date && x.Date <= DateTime.Today).ToListAsync();
                foreach (var mouv in listeMouvement)
                {
                    if (mouv.Credit > 0)
                    {
                        solde -= mouv.Credit;
                    }

                    if (mouv.Debit > 0)
                    {
                        solde += mouv.Debit;
                    }
                }
            solde = Math.Round(solde, 2);
            return solde;
        }

        /// <summary>
        /// Retourne le sodle initial d'un compte en base
        /// </summary>
        /// <returns>le solde d'un compte</returns>
        public async Task<double> GetOneSolde(int idCompte)
        {
            var compte = await Bdd.Connection.Table<Compte>().Where(x => x.Id == idCompte).FirstOrDefaultAsync();
            if (compte != null)
            {
                return compte.Solde;
            }
            return 0;
        }

        /// <summary>
        /// Ajoute une liste de compte en banque en base
        /// </summary>
        /// <param name="listeCompte">liste des comptes à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterCompte(IEnumerable<Compte> listeCompte)
        {
            foreach (var compte in listeCompte)
            {
                await AjouterCompte(compte);
            }
        }

        /// <summary>
        /// Ajoute un compte en base
        /// </summary>
        /// <param name="compte">le compte à ajouter</param>
        /// <returns>la task</returns>
        public async Task<int> AjouterCompte(Compte compte)
        {
            var compteId = await Bdd.Connection.Table<Compte>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (compteId != null)
            {
                id = compteId.Id + 1;
            }
            compte.Id = id;
            await Bdd.AjouterDonnee(compte);
            await RoamingCompteBusiness.AjouterCompte(compte);
            return compte.Id;
        }
        
        /// <summary>
        /// Permet d'obtenir un résumé des comptes de chaque banque pour la page d'acceuil
        /// </summary>
        public async Task<List<Banque>> GetResumeCompte()
        {
           var listeBanque = await Bdd.Connection.Table<Banque>().ToListAsync();
            foreach (var banque in listeBanque)
            {
                banque.ListeCompte = await Bdd.Connection.Table<Compte>().Where(x => x.IdBanque == banque.Id).ToListAsync();
                foreach (var compte in banque.ListeCompte)
                {
                    compte.DeviseToAffiche = DeviseUtils.GetDiminutifDevise(compte.IdDevise);
                }
            }
            return listeBanque;
        }

        /// <summary>
        /// Supprimer un compte de la base puis du roaming
        /// </summary>
        /// <param name="compte">le compte à supprimer</param>
        /// <returns>la task</returns>
        public async Task SupprimerCompte(Compte compte)
        {
            if (await Bdd.DeleteDonnee(compte) > 0)
            {
                //supression du compte
                await SupprimerMouvementsCompte(compte.Id);

                //supression des échéanciers lié au compte
                var echeancierBusiness = new EcheancierBusiness();
                await echeancierBusiness.Initialization;
                var listeEcheancier =
                    await
                        Bdd.Connection.Table<Echeancier>()
                            .Where(x => x.IdCompte == compte.Id || x.IdCompteVirement == compte.Id).ToListAsync();
                await echeancierBusiness.SupprimerEcheancier(listeEcheancier);

                //supression en roaming
                await RoamingCompteBusiness.SupprimerCompte(compte);
            }
        }

        /// <summary>
        /// Met à jour le nom et la devise d'un compte
        /// </summary>
        /// <param name="compte">le compte à mettre à jour</param>
        /// <returns>la task</returns>
        public async Task ModifierCompte(Compte compte)
        {
            //mise à jour du compte
            var res = await Bdd.Connection.Table<Compte>().Where(x => x.Id == compte.Id).FirstOrDefaultAsync();
            res.IdDevise = compte.IdDevise;
            res.Nom = compte.Nom;
            await Bdd.UpdateDonnee(res);
            await RoamingCompteBusiness.ModifierCompte(compte);
        }


        /// <summary>
        /// Efface tout les mouvements d'un compte, et efface également les liens des virements
        /// </summary>
        /// <param name="idCompte">l'id du compte à effacer</param>
        /// <returns>la task</returns>
        private async Task SupprimerMouvementsCompte(int idCompte)
        {
            var listeMouvements = await Bdd.Connection.Table<Mouvement>().Where(x => x.IdCompte == idCompte).ToListAsync();
            var listeVirements = (from v in await Bdd.Connection.Table<Mouvement>().ToListAsync() from m in listeMouvements where m.Id == v.IdMouvementVirement select v).ToList();
            foreach (var v in listeVirements)
            {
                v.IdMouvementVirement = 0;
            }
            await Bdd.UpdateListeDonnee(listeVirements);

            await Bdd.DeleteListeDonnee(listeMouvements);
        }


        #region Solde initial

        /// <summary>
        /// Ajoute à la table des soldes initials un nouveau solde avec un id du compte
        /// </summary>
        /// <param name="solde">le solde</param>
        /// <param name="idCompte">l'id du compte</param>
        /// <returns>la task</returns>
        public async Task AjouterSoldeInitial(double solde, int idCompte)
        {
            var obj = new SoldeInitial(idCompte,solde);
            var soldeInitId = await Bdd.Connection.Table<SoldeInitial>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (soldeInitId != null)
            {
                id = soldeInitId.Id + 1;
            }
            obj.Id = id;
            await Bdd.AjouterDonnee(obj);
        }

        /// <summary>
        /// Supprime un compte de la table solde initial
        /// </summary>
        /// <param name="idCompte">l'id du compte</param>
        /// <returns>la task</returns>
        public async Task SupprimerSoldeInitial(int idCompte)
        {
            var soldeInit = await Bdd.Connection.Table<SoldeInitial>().Where(x => x.IdCompte == idCompte).ToListAsync();
            if (soldeInit != null && soldeInit.Count > 0)
            {
                await Bdd.DeleteListeDonnee(soldeInit);
            }
        }

        /// <summary>
        /// Retourne tout le soldes initial en base
        /// </summary>
        /// <returns>la liste de ssoldes initials</returns>
        public async Task<List<SoldeInitial>> GetAllSoldeInitial()
        {
            return await Bdd.Connection.Table<SoldeInitial>().ToListAsync();
        }
        
        /// <summary>
        /// Ajoute un compte provenant des données de restauration
        /// </summary>
        /// <param name="compte">le compte à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterCompteFmRestauration(Compte compte)
        {
            await Bdd.AjouterDonnee(compte);
            await RoamingCompteBusiness.AjouterCompte(compte);
        }

        /// <summary>
        /// Ajoute une solde initial provenant de la restauration
        /// </summary>
        /// <param name="soldeinitial">la solde initial</param>
        /// <returns>la task</returns>
        public async Task AjouterSoldeInitialFmRestauration(SoldeInitial soldeinitial)
        {
            await Bdd.AjouterDonnee(soldeinitial);
        }

        #endregion

    }
}
