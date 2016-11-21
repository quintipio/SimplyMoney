using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using CompteWin10.Business;
using CompteWin10.Com;
using CompteWin10.Context;
using CompteWin10.Model;
using CompteWin10.Roaming.Model;
using CompteWin10.Utils;

namespace CompteWin10.Roaming.Business
{
    /// <summary>
    /// Business pour l'écriture des échéanciers
    /// </summary>
    public static class RoamingEcheancierBusiness
    {
        private static ComFile _roamingEcheancierFile;

        private static RoamingEcheancierModel _roamingEcheancier;

        #region général au roaming
        /// <summary>
        /// Charger les données liées au roaming
        /// </summary>
        /// <returns>la task</returns>
        private static async Task DemarrageRoaming()
        {
            if (_roamingEcheancier == null)
            {
                _roamingEcheancier = new RoamingEcheancierModel();
            }

            if (_roamingEcheancierFile == null)
            {
                _roamingEcheancierFile = new ComFile(ContexteStatic.FichierEcheancierRoaming, ComFile.PlaceEcriture.Roaming);
            }
            await LoadFileCompte();
        }

        /// <summary>
        /// Efface les données de roaming
        /// </summary>
        public static async Task DeleteRoaming()
        {
            await DemarrageRoaming();
            await _roamingEcheancierFile.DeleteFile();
            _roamingEcheancierFile = null;
            _roamingEcheancier = null;
        }


        /// <summary>
        /// Sauvegarde le fichier contenant les informations de comptes
        /// </summary>
        private async static Task SaveFile()
        {
            if (App.IsRoamingEcheancierActive)
            {
                var xs = new XmlSerializer(typeof(RoamingEcheancierModel));
                var wr = new StringWriter();
                xs.Serialize(wr, _roamingEcheancier);
                var data = CryptUtils.AesEncryptStringToByteArray(wr.ToString(), ContexteStatic.CleChiffrement,
                    ContexteStatic.CleChiffrement);
                await _roamingEcheancierFile.Ecrire(data, CreationCollisionOption.ReplaceExisting);
            }
            
        }

        /// <summary>
        /// Charge les données du fichier de compte en roaming s'il existe
        /// </summary>
        private static async Task LoadFileCompte()
        {
            if (await _roamingEcheancierFile.FileExist())
            {
                var inFile = await _roamingEcheancierFile.LireByteArray();
                if (inFile != null)
                {
                    var xmlIn = CryptUtils.AesDecryptByteArrayToString(inFile, ContexteStatic.CleChiffrement,
                    ContexteStatic.CleChiffrement);

                    var xsb = new XmlSerializer(typeof(RoamingEcheancierModel));
                    var rd = new StringReader(xmlIn);
                    _roamingEcheancier = xsb.Deserialize(rd) as RoamingEcheancierModel;
                }
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
            var espaceOccupe = await _roamingEcheancierFile.GetSizeFile();
            var ret = (100 * espaceOccupe) / espaceMax;
            return (espaceOccupe > 0 && ret == 0) ? 1 : (int)ret;
        }
        #endregion

        /// <summary>
        /// Ajoute un échéancier au fichier de roaming
        /// </summary>
        /// <param name="echeancier">l'échéancier à ajouter</param>
        public static async Task AjouterEcheancierRoaming(Echeancier echeancier)
        {
            await DemarrageRoaming();
            _roamingEcheancier.ListeEcheancier.Add(echeancier);
            await SaveFile();
        }

        /// <summary>
        /// Modifie un échéancier dasn le fichier de roaming
        /// </summary>
        /// <param name="echeancier">l'échéancier à modifier</param>
        /// <returns>la task</returns>
        public static async Task ModifierEcheancierRoaming(Echeancier echeancier)
        {
            await DemarrageRoaming();
            var res = _roamingEcheancier.ListeEcheancier.FirstOrDefault(x => x.Id == echeancier.Id);
            if (res != null)
            {
                res.Date = echeancier.Date;
                res.DateLimite = echeancier.DateLimite;
                res.IdCompte = echeancier.IdCompte;
                res.IdCompteVirement = echeancier.IdCompteVirement;
                res.IdPeriodicite = echeancier.IdPeriodicite;
                res.IsDateLimite = echeancier.IsDateLimite;
                res.ModeMouvement = echeancier.ModeMouvement;
                res.NbJours = echeancier.NbJours;
                res.Commentaire = echeancier.Commentaire;
                res.Credit = echeancier.Credit;
                res.Debit = echeancier.Debit;
                res.IdType = echeancier.IdType;
                res.IsTypePerso = echeancier.IsTypePerso;
            }
            await SaveFile();
        }

        /// <summary>
        /// Supprimer l'échéancier du roaming
        /// </summary>
        /// <param name="echeancier">l'échéancier à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerEcheancier(Echeancier echeancier)
        {
            await DemarrageRoaming();
            _roamingEcheancier.ListeEcheancier.RemoveAll(x => x.Id == echeancier.Id);
            await SaveFile();
        }

        /// <summary>
        /// Retourne les échéanciers dans le fichier de roaming
        /// </summary>
        /// <returns> la liste des échéanciers</returns>
        public static async Task<List<Echeancier>> GetAllEcheancier()
        {
            await DemarrageRoaming();
            return _roamingEcheancier.ListeEcheancier;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task ReCreerFichierRoaming()
        {
            await DemarrageRoaming();


            var echeancierBusiess = new EcheancierBusiness();
            await echeancierBusiess.Initialization;

            _roamingEcheancier.ListeEcheancier = new List<Echeancier>(await echeancierBusiess.GetEcheancier());

            await SaveFile();
        }
    }
}
