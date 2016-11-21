
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
    /// Classe businness pour la gestion des catégorie en roaming
    /// </summary>
    public static class RoamingCategorieBusiness
    {
        private static ComFile _roamingCategorieFile;

        private static RoamingCategorieModel _roamingCategorie;

        #region général au roaming
        /// <summary>
        /// Charger les données liées au roaming
        /// </summary>
        /// <returns>la task</returns>
        private static async Task DemarrageRoaming()
        {
            if (_roamingCategorie == null)
            {
                _roamingCategorie = new RoamingCategorieModel();
            }

            if (_roamingCategorieFile == null)
            {
                _roamingCategorieFile = new ComFile(ContexteStatic.FichierCategorieRoaming, ComFile.PlaceEcriture.Roaming);
            }
            await LoadFileCompte();
        }

        /// <summary>
        /// Efface les données de roaming
        /// </summary>
        public static async Task DeleteRoaming()
        {
            await DemarrageRoaming();
            await _roamingCategorieFile.DeleteFile();
            _roamingCategorieFile = null;
            _roamingCategorie = null;
        }


        /// <summary>
        /// Sauvegarde le fichier contenant les informations de comptes
        /// </summary>
        private async static Task SaveFile()
        {
            if (App.IsRoamingCategorieActive)
            {
                var xs = new XmlSerializer(typeof(RoamingCategorieModel));
                var wr = new StringWriter();
                xs.Serialize(wr, _roamingCategorie);
                var data = CryptUtils.AesEncryptStringToByteArray(wr.ToString(), ContexteStatic.CleChiffrement,
                    ContexteStatic.CleChiffrement);
                await _roamingCategorieFile.Ecrire(data, CreationCollisionOption.ReplaceExisting);
            }
        }

        /// <summary>
        /// Charge les données du fichier de compte en roaming s'il existe
        /// </summary>
        private async static Task LoadFileCompte()
        {
            if (await _roamingCategorieFile.FileExist())
            {
                var inFile = await _roamingCategorieFile.LireByteArray();
                var xmlIn = CryptUtils.AesDecryptByteArrayToString(inFile, ContexteStatic.CleChiffrement,
                    ContexteStatic.CleChiffrement);

                var xsb = new XmlSerializer(typeof(RoamingCategorieModel));
                var rd = new StringReader(xmlIn);
                _roamingCategorie = xsb.Deserialize(rd) as RoamingCategorieModel;
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
            var espaceOccupe = await _roamingCategorieFile.GetSizeFile();
            var ret = (100 * espaceOccupe) / espaceMax;
            return (espaceOccupe > 0 && ret == 0) ? 1 : (int)ret;
        }
        #endregion


        #region categorie
        /// <summary>
        /// Retourne la lsite des catégories en roaming
        /// </summary>
        /// <returns>la liste des catégories</returns>
        public static async Task<List<Categorie>> GetCategorieRoaming()
        {
            await DemarrageRoaming();
            var retour = new List<Categorie>();
            foreach (var category in _roamingCategorie.ListeCategorie)
            {
                category.IsCategPerso = true;
                retour.Add(category);
            }
            return retour;

        }

        /// <summary>
        /// Ajoute une catégorie au roaming
        /// </summary>
        /// <param name="categorie">la catégorie à ajouter</param>
        /// <returns>la task</returns>
        public static async Task AjouterCategorie(Categorie categorie)
        {
            await DemarrageRoaming();
            categorie.SousCategorieList = new List<SousCategorie>();
            _roamingCategorie.ListeCategorie.Add(categorie);
            await SaveFile();
        }

        /// <summary>
        /// modifie une catégorie
        /// </summary>
        /// <param name="categorie">la catégorie à modifier</param>
        /// <returns>la task</returns>
        public static async Task ModifierCategorie(Categorie categorie)
        {
            await DemarrageRoaming();
            _roamingCategorie.ListeCategorie.FirstOrDefault(x => x.Id == categorie.Id).Libelle = categorie.Libelle;
            await SaveFile();
        }

        /// <summary>
        /// Supprime une catégorie
        /// </summary>
        /// <param name="categorie">La categorie à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerCategorie(Categorie categorie)
        {
            await DemarrageRoaming();
            _roamingCategorie.ListeCategorie.RemoveAll(x => x.Id == categorie.Id);
            await SaveFile();
        }

        #endregion


        #region sous catégorie

        /// <summary>
        /// Retourne la lsite des sous catégories en roaming
        /// </summary>
        /// <returns>la liste des sous catégories</returns>
        public static async Task<List<SousCategorie>> GetSousCategorieRoaming()
        {
            await DemarrageRoaming();
            var retour = new List<SousCategorie>();
            foreach (var souscategory in _roamingCategorie.ListeSousCategorie)
            {
                souscategory.IsSousCategPerso = true;
                retour.Add(souscategory);
            }
            return retour;

        }

        /// <summary>
        /// Ajoute une sous catégorie au roaming
        /// </summary>
        /// <param name="sousCategorie">la sous catégorie à ajouter</param>
        /// <returns>la task</returns>
        public static async Task AjouterSousCategorie(SousCategorie sousCategorie)
        {
            await DemarrageRoaming();
            _roamingCategorie.ListeSousCategorie.Add(sousCategorie);
            await SaveFile();
        }

        /// <summary>
        /// modifie une sous catégorie
        /// </summary>
        /// <param name="sousCategorie">la sous catégorie à modifier</param>
        /// <returns>la task</returns>
        public static async Task ModifierSousCategorie(SousCategorie sousCategorie)
        {
            await DemarrageRoaming();
            _roamingCategorie.ListeSousCategorie.First(x => x.Id == sousCategorie.Id).Libelle = sousCategorie.Libelle;
            await SaveFile();
        }

        /// <summary>
        /// Supprime une sous catégorie
        /// </summary>
        /// <param name="sousCategorie">La sous categorie à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerSousCategorie(SousCategorie sousCategorie)
        {
            await DemarrageRoaming();
            _roamingCategorie.ListeSousCategorie.RemoveAll(x => x.Id == sousCategorie.Id);
            await SaveFile();
        }
        #endregion

        /// <summary>
        /// Récréer le fichier de roaming à partir de la base
        /// </summary>
        /// <returns>la task</returns>
        public static async Task ReCreerFichierRoaming()
        {
            await DemarrageRoaming();

            var categorieBusiess = new CategorieBusiness();
            await categorieBusiess.Initialization;

            _roamingCategorie.ListeCategorie = new List<Categorie>(await categorieBusiess.GetCategoriePerso());
            _roamingCategorie.ListeSousCategorie = new List<SousCategorie> (await categorieBusiess.GetSousCategoriesPerso());

            await SaveFile();
        }
    }
}
