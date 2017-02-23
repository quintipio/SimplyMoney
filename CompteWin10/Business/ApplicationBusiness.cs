using System;
using System.Threading.Tasks;
using CompteWin10.Abstract;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Strings;
using CompteWin10.Utils;

namespace CompteWin10.Business
{


    /// <summary>
    /// Classe pour la communication avec la partie Com
    /// </summary>
    public class ApplicationBusiness : AbstractBusiness
    {
        /// <summary>
        /// Créer les élements de bases de la base de donnée
        /// </summary>
        /// <param name="appareil">le mode de l'appareil contenant l'appli</param>
        /// <returns>true si ok</returns>
        public async Task<bool> CreerBase(AppareilEnum appareil)
        {
            await Bdd.CreateDb();

            switch (appareil)
            {
                    case AppareilEnum.ModeAppareilPrincipal:
                    var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
                    res.ModeAppareil = (int) AppareilEnum.ModeAppareilPrincipal;
                    res.IdBackGround = 0;
                    res.IdLangue = ListeLangues.GetLangueEnCours().diminutif;
                    res.DateDernierDemarrage = DateUtils.GetMaintenant();
                    res.IsSynchroCategorieActive = true;
                    res.IsSynchroEcheancierActive = true;
                    await Bdd.UpdateDonnee(res);
                    break;

                    case AppareilEnum.ModeAppareilSecondaire:
                    var resb = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
                    resb.ModeAppareil = (int)AppareilEnum.ModeAppareilSecondaire;
                    resb.IdBackGround = 0;
                    resb.IdLangue = ListeLangues.GetLangueEnCours().diminutif;
                    resb.DateDernierDemarrage = DateUtils.GetMaintenant();
                    resb.IsSynchroCategorieActive = true;
                    resb.IsSynchroEcheancierActive = true;
                    await Bdd.UpdateDonnee(resb);
                    break;
            }

            return false;
        }

        /// <summary>
        /// Change en base la couleur de fond sélectionnée
        /// </summary>
        /// <param name="newId">le nouvel id de la couleur</param>
        /// <returns>la task</returns>
        public async Task ChangeIdCouleurBackground(int newId)
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            res.IdBackGround = newId;
            await Bdd.UpdateDonnee(res);
        }

        /// <summary>
        /// Change en base la langue sélectionnée
        /// </summary>
        /// <param name="newId">la nouvelle langue</param>
        /// <returns>la task</returns>
        public async Task ChangeIdLangue(ListeLangues.LanguesStruct langue)
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            res.IdLangue = langue.diminutif;
            await Bdd.UpdateDonnee(res);
        }

        /// <summary>
        /// Met à la date du jour la date de dernier démarrage de l'appli
        /// </summary>
        /// <returns>la task</returns>
        public async Task SetDateDernierDemarrage()
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            res.DateDernierDemarrage = DateUtils.GetMaintenant();
            await Bdd.UpdateDonnee(res);
        }

        /// <summary>
        /// Créer la base de donnée à partir des données présentes dans le fihier Roaming
        /// </summary>
        /// <returns>las task</returns>
        public async Task CreerBaseDeDonneeFromRoaming(AppareilEnum appareil)
        {
            //creer la base
            await CreerBase(appareil);

            if (appareil == AppareilEnum.ModeAppareilPrincipal)
            {
                //ajout des banques et des comptes
                var listeBanques = await RoamingCompteBusiness.GetListeBanques();
                foreach (var banque in listeBanques)
                {
                    // ajout de la banque
                    var banqueId = await Bdd.Connection.Table<Banque>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    var idbanque = 1;
                    if (banqueId != null)
                    {
                        idbanque = banqueId.Id + 1;
                    }
                    banque.Id = idbanque;
                    await Bdd.AjouterDonnee(banque);

                    //ajout des comtpes
                    foreach (var compte in banque.ListeCompte)
                    {
                        var compteId = await Bdd.Connection.Table<Compte>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                        var id = 1;
                        if (compteId != null)
                        {
                            id = compteId.Id + 1;
                        }
                        compte.Id = id;
                        compte.IdBanque = banque.Id;
                        await Bdd.AjouterDonnee(compte);
                    }
                }

                //Ajout des échéanciers
                var listeEcheancier = await RoamingEcheancierBusiness.GetAllEcheancier();
                foreach (var echeancier in listeEcheancier)
                {
                    await Bdd.AjouterDonnee(echeancier);
                }

                //ajout des catégories perso
                var listeCategorie= await RoamingCategorieBusiness.GetCategorieRoaming();
                foreach (var category in listeCategorie)
                {
                    await Bdd.AjouterDonnee(category);
                }

                var listeSousCategorie = await RoamingCategorieBusiness.GetSousCategorieRoaming();
                foreach (var sousCategory in listeSousCategorie)
                {
                    await Bdd.AjouterDonnee(sousCategory);
                }

            }
        }

        /// <summary>
        /// Retourne l'id de la couleur de fond de l'appli
        /// </summary>
        /// <returns>id de la couleur</returns>
        public async Task<int> GetIdCouleurBackGround()
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            return res.IdBackGround;
        }

        /// <summary>
        /// Retourne l'id de la langue sélectionné
        /// </summary>
        /// <returns>id de la langue</returns>
        public async Task<string> GetLangueAppli()
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            return res.IdLangue;
        }

        /// <summary>
        /// Retourne le mode de fonctionnement
        /// </summary>
        /// <returns>le mdoe de fonctionnement de l'appli</returns>
        public async Task<AppareilEnum> GetModeAppli()
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            return (AppareilEnum) res.ModeAppareil;
        }

        /// <summary>
        /// Retourne le boolean indiquant si le roaming des categories est actif
        /// </summary>
        /// <returns>le boolean</returns>
        public async Task<bool> GetRoamingCategorieActive()
        {
            return (await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync()).IsSynchroCategorieActive;
        }

        /// <summary>
        /// Retourne le boolean indiquant si le roaming des échéancier est actif
        /// </summary>
        /// <returns>le boolean</returns>
        public async Task<bool> GetRoamingEcheancierActive()
        {
            return (await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync()).IsSynchroEcheancierActive;
        }

        /// <summary>
        /// Set si le roaming des categories est actif
        /// </summary>
        public async Task SetRoamingCategorieActive(bool active)
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            res.IsSynchroCategorieActive = active;
            await Bdd.UpdateDonnee(res);
        }

        /// <summary>
        /// Set si le roaming des échéancier est actif
        /// </summary>
        public async Task SetRoamingEcheancierActive(bool active)
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            res.IsSynchroEcheancierActive = active;
            await Bdd.UpdateDonnee(res);
        }

        /// <summary>
        /// Efface la base de donnée
        /// </summary>
        /// <returns>la task</returns>
        public async Task DeleteDatabase()
        {
           await Bdd.DeleteDatabase();
        }

        /// <summary>
        /// Efface toute les données devant être restauré
        /// </summary>
        /// <returns>la task</returns>
        public async Task DeleteForRestauration()
        {
            await Bdd.Connection.DropTableAsync<Banque>();
            await Bdd.Connection.DropTableAsync<Compte>();
            await Bdd.Connection.DropTableAsync<Echeancier>();
            await Bdd.Connection.DropTableAsync<Mouvement>();
            await Bdd.Connection.DropTableAsync<Categorie>();
            await Bdd.Connection.DropTableAsync<SousCategorie>();
            await Bdd.Connection.DropTableAsync<SoldeInitial>();

            await Bdd.Connection.CreateTableAsync<Banque>();
            await Bdd.Connection.CreateTableAsync<Compte>();
            await Bdd.Connection.CreateTableAsync<Echeancier>();
            await Bdd.Connection.CreateTableAsync<Mouvement>();
            await Bdd.Connection.CreateTableAsync<Categorie>();
            await Bdd.Connection.CreateTableAsync<SousCategorie>();
            await Bdd.Connection.CreateTableAsync<SoldeInitial>();

            await RoamingCategorieBusiness.DeleteRoaming();
            await RoamingCompteBusiness.DeleteRoaming();
            await RoamingEcheancierBusiness.DeleteRoaming();
            await RoamingMouvementBusiness.DeleteRoaming();
        }

        /// <summary>
        /// Vérifie la version éxécuté et met à jours si besoin
        /// </summary>
        /// <returns></returns>
        public async Task CheckVersion()
        {
            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            if (res.Version != ContexteStatic.Version)
            {
                //changement pour la version 1.1.2 = miseà miuit des dates d'échéanciers
                if (StringUtils.CheckVersion(res.Version, "1.1.2"))
                {
                    var listeEcheancier = await Bdd.Connection.Table<Echeancier>().ToListAsync();
                    foreach (var echeancier in listeEcheancier)
                    {
                        echeancier.Date = new DateTime(echeancier.Date.Year, echeancier.Date.Month, echeancier.Date.Day);
                        if (echeancier.IsDateLimite)
                        {
                            echeancier.DateLimite = new DateTime(echeancier.DateLimite.Year, echeancier.DateLimite.Month, echeancier.DateLimite.Day);
                        }
                        await Bdd.UpdateDonnee(echeancier);
                    }
                }

                if (StringUtils.CheckVersion(res.Version, "1.2.0"))
                {
                    await Bdd.Connection.CreateTableAsync<Application>();
                }

                res.Version = ContexteStatic.Version;
            }
            await Bdd.UpdateDonnee(res);
        }
    }
}
