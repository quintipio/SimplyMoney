using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompteWin10.Abstract;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;
using CompteWin10.Utils;

namespace CompteWin10.Business
{
    /// <summary>
    /// Classe business pour traiter les échéancier
    /// </summary>
    public class EcheancierBusiness : AbstractBusiness
    {
        /// <summary>
        /// Permet d'obtenir la liste des échéancier en base
        /// </summary>
        /// <returns>la liste des échéancier</returns>
        public async Task<List<Echeancier>> GetEcheancier()
        {
            var list = await Bdd.Connection.Table<Echeancier>().ToListAsync();
            foreach (var echeancier in list)
            {
                var devise = DeviseUtils.GetDiminutifDevise((await Bdd.Connection.Table<Compte>().Where(x => x.Id == echeancier.IdCompte).FirstOrDefaultAsync()).IdDevise);
                echeancier.MouvementChiffre = ((echeancier.Credit > 0) ? "+" + echeancier.Credit : "-" + echeancier.Debit) + " " + devise;
                echeancier.Type =
                    ContexteAppli.ListeCategoriesMouvement.SelectMany(
                        varA =>
                            varA.SousCategorieList.Where(
                                varB => varB.Id == echeancier.IdType && varB.IsSousCategPerso == echeancier.IsTypePerso))
                        .FirstOrDefault();
            }

            return list;
        }

        /// <summary>
        /// Ajoute ou modifie en base un échéancier en base
        /// </summary>
        /// <param name="echeancier">l'échéancier à sauvegardé (Id à zéro pour un ajout)</param>
        /// <returns>l'id de l'échéancier</returns>
        public async Task<int> SaveEcheancier(Echeancier echeancier)
        {
            if (echeancier.Id == 0)
            {
                var echeancierId = await Bdd.Connection.Table<Echeancier>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                var id = 1;
                if (echeancierId != null)
                {
                    id = echeancierId.Id + 1;
                }
                echeancier.Id = id;
                echeancier.Date = new DateTime(echeancier.Date.Year,echeancier.Date.Month,echeancier.Date.Day);
                if (echeancier.IsDateLimite)
                {
                    echeancier.DateLimite = new DateTime(echeancier.DateLimite.Year, echeancier.DateLimite.Month, echeancier.DateLimite.Day);
                }

                await Bdd.AjouterDonnee(echeancier);
                await RoamingEcheancierBusiness.AjouterEcheancierRoaming(echeancier);
            }
            else
            {
                await Bdd.UpdateDonnee(echeancier);
                await RoamingEcheancierBusiness.ModifierEcheancierRoaming(echeancier);
            }
            return echeancier.Id;
        }

        /// <summary>
        /// Supprimer un échéancier de la base de donnée
        /// </summary>
        /// <param name="idEcheancier">l'id de l'échéancier à supprimer</param>
        /// <returns>la task</returns>
        public async Task SupprimerEcheancier(int idEcheancier)
        {
            var obj = await Bdd.Connection.Table<Echeancier>().Where(x => x.Id == idEcheancier).FirstOrDefaultAsync();
            await Bdd.DeleteDonnee(obj);
            await RoamingEcheancierBusiness.SupprimerEcheancier(obj);
        }

        /// <summary>
        /// Supprime une liste d'échéancier de la base
        /// </summary>
        /// <param name="liste">la liste des échéanciers à supprimer</param>
        /// <returns>la task</returns>
        public async Task SupprimerEcheancier(IEnumerable<Echeancier> liste)
        {
            foreach (var echeancier in liste)
            {
               await SupprimerEcheancier(echeancier.Id);
            }
        }

        /// <summary>
        /// Exécute les échéancier depuis le dernier démarrage de l'appli
        /// </summary>
        /// <returns>la task</returns>
        public async Task ExecuteEcheancier()
        {
            var mouvementBusiness = new MouvementBusiness();
            await mouvementBusiness.Initialization;

            var dateDerniereExecution = (await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync()).DateDernierDemarrage;
            var dateMaintenant = DateUtils.GetMaintenant();

            //récupération des échéanciers à éxécuter
            var echeancierToExecute = await Bdd.Connection.Table<Echeancier>()
                .Where(x => x.Date > dateDerniereExecution && x.Date <= dateMaintenant)
                .ToListAsync();
            

            foreach (var echeancier in echeancierToExecute)
            {
                do
                {
                    if ((echeancier.Date <= echeancier.DateLimite && echeancier.IsDateLimite) ||
                        !echeancier.IsDateLimite)
                    {
                        //éxécution du mouvement
                        var mouvement = new Mouvement
                        {
                            Id = 0,
                            Commentaire = echeancier.Commentaire,
                            Credit = echeancier.Credit,
                            Debit = echeancier.Debit,
                            Date = echeancier.Date,
                            IdCompte = echeancier.IdCompte,
                            IdType = echeancier.IdType,
                            IsPasse = false,
                            IdMouvementVirement = 0,
                            IsTypePerso = echeancier.IsTypePerso,
                            Numero = 0,
                            ModeMouvement = echeancier.ModeMouvement
                        };

                        if (echeancier.IdCompteVirement > 0 && echeancier.IdCompte != echeancier.IdCompteVirement)
                        {
                            await mouvementBusiness.SaveVirement(mouvement, echeancier.IdCompteVirement);
                        }
                        else
                        {
                            await mouvementBusiness.SaveMouvement(mouvement);
                        }
                    }
                    else
                    {
                        break;
                    }

                    //enregistrement de la prochaine date
                    echeancier.Date = GetNbJoursPeriodicite((PeriodiciteEnum) echeancier.IdPeriodicite, echeancier.Date,
                        echeancier.NbJours);
                    await SaveEcheancier(echeancier);

                    //répétition tant que la date d'éxécution est toujours inférieur à aujourd'hui
                } while (echeancier.Date <= dateMaintenant);
            }
        }

        /// <summary>
        /// Retourne la prochaine date d'éxécution d'un échéancier en fonction de sa périodicité
        /// </summary>
        /// <param name="periodicite">la périodicité</param>
        /// <param name="dateDernierMouvement">la date de dernière éxécution de l'échéancier</param>
        /// <param name="nbJours">le nombre de jours (null si aucun)</param>
        /// <returns></returns>
        private static DateTime GetNbJoursPeriodicite(PeriodiciteEnum periodicite,DateTime dateDernierMouvement,int nbJours)
        {
            switch (periodicite)
            {
                    case PeriodiciteEnum.Annuel:
                    return DateUtils.AjouterAnnee(dateDernierMouvement, 1);

                    case PeriodiciteEnum.Bimestriel:
                    return DateUtils.AjouterMois(dateDernierMouvement, 6);

                    case PeriodiciteEnum.Trimestriel:
                    return DateUtils.AjouterMois(dateDernierMouvement, 3);

                    case PeriodiciteEnum.Mensuel:
                    return DateUtils.AjouterMois(dateDernierMouvement, 1);

                    case PeriodiciteEnum.Hebdomadaire:
                    return DateUtils.AjouterJours(dateDernierMouvement, 7);

                    case PeriodiciteEnum.Quotidien:
                    return DateUtils.AjouterJours(dateDernierMouvement, 1);

                    case PeriodiciteEnum.Personalise:
                    return DateUtils.AjouterJours(dateDernierMouvement, (nbJours == 0)?1:nbJours);

                default:
                    return DateUtils.AjouterJours(dateDernierMouvement, 1);
            }
        }

        /// <summary>
        /// Ajoute une échéancier provenant de l'outil de restauration
        /// </summary>
        /// <param name="echeancier"> l'échéancier à ajouter</param>
        /// <returns>la task</returns>
        public async Task AjouterEcheancierFmRestauration(Echeancier echeancier)
        {
            await Bdd.AjouterDonnee(echeancier);
            await RoamingEcheancierBusiness.AjouterEcheancierRoaming(echeancier);
        }

    }
}
