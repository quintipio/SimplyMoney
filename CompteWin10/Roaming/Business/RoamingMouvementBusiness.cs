using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using CompteWin10.Com;
using CompteWin10.Context;
using CompteWin10.Model;
using CompteWin10.Roaming.Model;
using CompteWin10.Utils;

namespace CompteWin10.Roaming.Business
{
    /// <summary>
    /// Classe permettant de gérer les mouvements dans le dosssier de roaming
    /// </summary>
    public static class RoamingMouvementBusiness
    {
        private static ComFile _roamingMouvementFile;

        private static RoamingMouvementModel _roamingMouvement;

        #region général au roaming
        /// <summary>
        /// Charger les données liées au roaming
        /// </summary>
        /// <returns>la task</returns>
        private static async Task DemarrageRoaming()
        {
            if (_roamingMouvement == null)
            {
                _roamingMouvement = new RoamingMouvementModel();
            }

            if (_roamingMouvementFile == null)
            {
                _roamingMouvementFile = new ComFile(ContexteStatic.FichierMouvementRoaming, ComFile.PlaceEcriture.Roaming);
            }
            await LoadFileCompte();
        }

        /// <summary>
        /// Efface les données de roaming
        /// </summary>
        public static async Task DeleteRoaming()
        {
            await DemarrageRoaming();
            await _roamingMouvementFile.DeleteFile();
            _roamingMouvementFile = null;
            _roamingMouvement = null;
        }


        /// <summary>
        /// Sauvegarde le fichier contenant les informations de comptes
        /// </summary>
        private async static Task SaveFile()
        {
            var xs = new XmlSerializer(typeof(RoamingMouvementModel));
            var wr = new StringWriter();
            xs.Serialize(wr, _roamingMouvement);
            var data = CryptUtils.AesEncryptStringToByteArray(wr.ToString(), ContexteStatic.CleChiffrement,
                ContexteStatic.CleChiffrement);
            await _roamingMouvementFile.Ecrire(data, CreationCollisionOption.ReplaceExisting);
        }

        /// <summary>
        /// Charge les données du fichier de compte en roaming s'il existe
        /// </summary>
        private async static Task LoadFileCompte()
        {
            if (await _roamingMouvementFile.FileExist())
            {
                var inFile = await _roamingMouvementFile.LireByteArray();
                var xmlIn = CryptUtils.AesDecryptByteArrayToString(inFile, ContexteStatic.CleChiffrement,
                    ContexteStatic.CleChiffrement);

                var xsb = new XmlSerializer(typeof(RoamingMouvementModel));
                var rd = new StringReader(xmlIn);
                _roamingMouvement = xsb.Deserialize(rd) as RoamingMouvementModel;
            }
        }

        /// <summary>
        /// Retourne en % l'espace occupé par le fichier dans le dossier de roaming
        /// </summary>
        /// <returns></returns>
        public async static Task<int> GetEspaceFichierOccupePourcent()
        {
            await DemarrageRoaming();
            var espaceMax = (ApplicationData.Current.RoamingStorageQuota > 0)
                ? ApplicationData.Current.RoamingStorageQuota * 1000 : ContexteStatic.RoaminsStorageQuotaBis * 1000;
            var espaceOccupe = await _roamingMouvementFile.GetSizeFile();
            var ret = (100 * espaceOccupe) / espaceMax;
            return (espaceOccupe > 0 && ret == 0) ? 1 : (int)ret;
        }
        #endregion


        #region gestion des mouvements

        /// <summary>
        /// Retourne la liste des mouvements en roaming
        /// </summary>
        /// <param name="idCompte">l'id du compte dont on cherche les mouvements</param>
        /// <returns>la liste des mouvements associés au compte</returns>
        public static async Task<List<Mouvement>> GetMouvementsRoaming(int idCompte)
        {
            await DemarrageRoaming();

            var devise = await RoamingCompteBusiness.GetDevise(idCompte);
            var retour = _roamingMouvement.ListeMouvement.Where(x => x.IdCompte == idCompte).ToList();

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
        /// Ajoute un mouvement à traiter dans le roaming
        /// </summary>
        /// <param name="mouvement">le mouvement à ajouter</param>
        /// <returns>l'id roaming du mouvement ajouté</returns>
        public static async Task<int> AjouterMouvementRoaming(Mouvement mouvement)
        {
            await DemarrageRoaming();
            var mouvementId = _roamingMouvement.ListeMouvement.OrderByDescending(x => x.Id).FirstOrDefault();
            var id = 1;
            if (mouvementId != null)
            {
                id = mouvementId.Id + 1;
            }
            mouvement.Id = id;

            _roamingMouvement.ListeMouvement.Add(mouvement);
            await CalculerSolde(mouvement, null);
            
            await SaveFile();
            return id;
        }


        /// <summary>
        /// Supprime un mouvement sans recalculer la solde du compte ou en prenant en compte les virements
        /// </summary>
        /// <param name="idMouvement">l'id du mouvement à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerMouvementSimpleRoaming(int idMouvement)
        {
            await DemarrageRoaming();
            _roamingMouvement.ListeMouvement.RemoveAll(x => x.Id == idMouvement);
            await SaveFile();
        }

        /// <summary>
        /// Supprime un mouvement des roaming à traiter
        /// </summary>
        /// <param name="idMouvement">l'id du mouvement à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerMouvementRoaming(int idMouvement)
        {
            await DemarrageRoaming();
            var mouvementDelete = _roamingMouvement.ListeMouvement.FirstOrDefault(x => x.Id == idMouvement);
            _roamingMouvement.ListeMouvement.RemoveAll(x => x.Id == idMouvement);
            await CalculerSolde(null, mouvementDelete);

            //supression en cas de virement
            if (mouvementDelete.IdMouvementVirement > 0)
            {
                var mouvementB = _roamingMouvement.ListeMouvement.FirstOrDefault(x => x.Id == mouvementDelete.IdMouvementVirement);
                if (mouvementB != null)
                {
                    _roamingMouvement.ListeMouvement.RemoveAll(x => x.Id == mouvementB.Id);
                    await CalculerSolde(null, mouvementB);
                }
                
            }
            await SaveFile();
        }

        /// <summary>
        /// modifie l'info de passe d'un mouvement 
        /// </summary>
        /// <param name="idMouvement">le mouvement</param>
        /// <param name="isPasse">true si passe sinon false</param>
        /// <returns>la task</returns>
        public static async Task ModifierPasseMouvement(int idMouvement, bool isPasse)
        {
            await DemarrageRoaming();
            var mouv = _roamingMouvement.ListeMouvement.FirstOrDefault(x => x.Id == idMouvement);
            mouv.IsPasse = isPasse;
            await ModifierMouvementRoaming(mouv);
        }

        /// <summary>
        /// Modifie un mouvement en roaming
        /// </summary>
        /// <param name="mouvement">le mouvement à modifié</param>
        /// <returns>la task</returns>
        public static async Task ModifierMouvementRoaming(Mouvement mouvement)
        {
            await DemarrageRoaming();
            var oldMouvement = _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id);

            await CalculerSolde(mouvement,oldMouvement);
            
            //si ca contient un idVirement mais quel le type n'est pas un virement, on efface le virement
            if (mouvement.IdMouvementVirement > 0 && mouvement.ModeMouvement != 3)
            {
                var mouvementDelete = _roamingMouvement.ListeMouvement.FirstOrDefault(x => x.Id == mouvement.IdMouvementVirement);
                _roamingMouvement.ListeMouvement.RemoveAll(x => x.Id == mouvementDelete.Id);
                mouvement.IdMouvementVirement = 0;
                await CalculerSolde(null, mouvementDelete);
            }

            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).Credit = mouvement.Credit;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).Debit = mouvement.Debit;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).Date = mouvement.Date;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).Numero = mouvement.Numero;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).Commentaire = mouvement.Commentaire;

            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).IsPasse = mouvement.IsPasse;

            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).IdCompte = mouvement.IdCompte;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).ModeMouvement = mouvement.ModeMouvement;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).IdMouvementVirement = mouvement.IdMouvementVirement;
            
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).IsTypePerso = mouvement.IsTypePerso;
            _roamingMouvement.ListeMouvement.First(x => x.Id == mouvement.Id).IdType = mouvement.IdType;
            
            await SaveFile();
        }

        /// <summary>
        /// Sauvegarde un virement
        /// </summary>
        /// <param name="mouvement">le mouvement d'origine du virement</param>
        /// <param name="idCompte">l'id du compte sur lequel s'effectue sur le virement</param>
        /// <returns>la task</returns>
        public static async Task SaveVirement(Mouvement mouvement, int idCompte)
        {
            await DemarrageRoaming();
            var create = mouvement.Id == 0;
            Mouvement mouvementA;
            Mouvement mouvementB;

            //mise à jour
            if (!create)
            {
                await SupprimerMouvementRoaming(mouvement.Id);
                mouvement.Id = 0;
            }
            mouvementA = mouvement;
               
            var idA = await AjouterMouvementRoaming(mouvementA);

            var idDeviseA = (await RoamingCompteBusiness.GetCompte(mouvementA.IdCompte)).IdDevise;
            var idDeviseB = (await RoamingCompteBusiness.GetCompte(idCompte)).IdDevise;

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
                IsTypePerso = mouvementA.IsTypePerso,
                IdMouvementVirement = idA
            };

            var idB = await AjouterMouvementRoaming(mouvementB);
            mouvementA.Id = idA;
            mouvementA.IdMouvementVirement = idB;
            await ModifierMouvementRoaming(mouvementA);
        }


        /// <summary>
        /// Retourne le numéro de compte d'un mouvement
        /// </summary>
        /// <param name="idMouvement">l'id du mouvement</param>
        /// <returns>le compte</returns>
        public static async Task<int> GetIdCompteMouvementLieVirement(int idMouvement)
        {
            await DemarrageRoaming();
            return idMouvement > 0 ? (_roamingMouvement.ListeMouvement.FirstOrDefault(x => x.Id == idMouvement)).IdCompte : 0;
        }

        #endregion


        /// <summary>
        /// Retourne la liste des mouvements en roaming
        /// </summary>
        /// <returns>une liste de mouvements</returns>
        public static async Task<List<Mouvement>> GetAllMouvement()
        {
            await DemarrageRoaming();
            return _roamingMouvement.ListeMouvement;
        }


        /// <summary>
        /// Recalcul la solde d'un compte après l'ajout ou la modification d'un mouvement
        /// </summary>
        /// <param name="mouvement">le mouvement (null pour une suppresion de mouvement)</param>
        /// <param name="oldMouvement">l'ancien mouvement (null si ajout)</param>
        /// <returns>la task</returns>
        private static async Task CalculerSolde(Mouvement mouvement, Mouvement oldMouvement)
        {
            var idCompte = mouvement?.IdCompte ?? oldMouvement.IdCompte;
            var compte = await RoamingCompteBusiness.GetCompte(idCompte);

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
            
            await RoamingCompteBusiness.ModifierCompte(compte);
        }

    }
}
