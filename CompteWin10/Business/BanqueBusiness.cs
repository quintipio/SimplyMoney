using System.Collections.Generic;
using System.Threading.Tasks;
using CompteWin10.Abstract;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;

namespace CompteWin10.Business
{
    /// <summary>
    /// Connecteur vers la couche de donnée pour les banques
    /// </summary>
    public class BanqueBusiness : AbstractBusiness
    {
        /// <summary>
        /// Retourne une banque à partir de son id
        /// </summary>
        /// <param name="id">l'id de la banque recherché</param>
        /// <returns>la banque</returns>
        public async Task<Banque> GetBanque(int id)
        {
            return await Bdd.Connection.Table<Banque>().Where(x => x.Id == id).FirstAsync();
        }

        /// <summary>
        /// Retourne la liste des banques
        /// </summary>
        /// <returns>les banques en base</returns>
        public async Task<List<Banque>> GetBanques()
        {
            return await Bdd.Connection.Table<Banque>().ToListAsync();
        }

        /// <summary>
        /// Ajoute une banque en base
        /// </summary>
        /// <param name="banque">la banque à ajouter</param>
        /// <returns>la task</returns>
        public async Task<Banque> AjouterBanque(Banque banque)
        {
            var banqueId = await Bdd.Connection.Table<Banque>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (banqueId != null)
            {
                id = banqueId.Id + 1;
            }
            banque.Id = id;
            await Bdd.AjouterDonnee(banque);
            await RoamingCompteBusiness.AjouterBanque(banque);
            return banque;
        }

        /// <summary>
        /// Supprime une banque de la base de donnée et du roaming
        /// </summary>
        /// <param name="banque">la banque à supprimer</param>
        /// <returns>la task</returns>
        public async Task SupprimerBanque(Banque banque)
        {
            var comptes = await Bdd.Connection.Table<Compte>().Where(x => x.IdBanque == banque.Id).ToListAsync();

            var compteBusiness = new CompteBusiness();
            await compteBusiness.Initialization;

            foreach (var compte in comptes)
            {
               await compteBusiness.SupprimerCompte(compte);
            }

            if (await Bdd.DeleteDonnee(banque) > 0)
            {
                await RoamingCompteBusiness.SupprimerBanque(banque);
            }
        }

        /// <summary>
        /// Modifie une banque en base et en romaing ainsi que la liste des comptes pour les devises
        /// </summary>
        /// <param name="banque">la banque à modifier</param>
        /// <returns>la task</returns>
        public async Task ModifierBanque(Banque banque)
        {
            //mise à jour de la banque
            var res = await Bdd.Connection.Table<Banque>().Where(x => x.Id == banque.Id).FirstOrDefaultAsync();
            res.IdDevise = banque.IdDevise;
            res.Nom = banque.Nom;
            await Bdd.UpdateDonnee(res);
            await RoamingCompteBusiness.ModifierBanque(banque);

            //mise à jour de la devise de chaque comptes
            var resCompte = await Bdd.Connection.Table<Compte>().Where(x => x.IdBanque == banque.Id).ToListAsync();
            foreach (var compte in resCompte)
            {
                compte.IdDevise = banque.IdDevise;
                await Bdd.UpdateDonnee(compte);
                await RoamingCompteBusiness.ModifierCompte(compte);
            }
        }

        /// <summary>
        /// Retourne une liste de comptes pour les virements en excluant le compte ouvert (0 si aucun compte)
        /// </summary>
        /// <param name="idCompteExclus">l'id du comtpe à exclure</param>
        /// <returns>la liste des banqes avec la liste des comptes</returns>
        public async Task<List<Banque>> GetListeBanqueCompteVirement(int idCompteExclus)
        {
            var listeBanque = await Bdd.Connection.Table<Banque>().ToListAsync();
            foreach (var banque in listeBanque)
            {
                banque.ListeCompte = await Bdd.Connection.Table<Compte>().Where(x => x.IdBanque == banque.Id && x.Id != idCompteExclus).ToListAsync();
            }
            return listeBanque;
        }

        /// <summary>
        /// Retourne les comptes d'une banque
        /// </summary>
        /// <param name="idBanque">l'id de la banque</param>
        /// <returns>les compte de la banque</returns>
        public async Task<List<Compte>> GetCompteFmBanque(int idBanque)
        {
            return await Bdd.Connection.Table<Compte>().Where(x => x.IdBanque == idBanque).ToListAsync();
        }


        /// <summary>
        /// Ajoute une banque en base
        /// </summary>
        /// <param name="banque">la banque à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterBanqueFmRestauration(Banque banque)
        {
            await Bdd.AjouterDonnee(banque);
            await RoamingCompteBusiness.AjouterBanque(banque);
        }
    }
}
