using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using CompteWin10.Business;
using CompteWin10.Com;
using CompteWin10.Context;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Model;
using CompteWin10.Utils;

namespace CompteWin10.Roaming.Business
{
    /// <summary>
    /// Classe permettant de gérer les informations partagées de l'appli (dans le dossier Roaming)
    /// </summary>
    public static class RoamingCompteBusiness
    {
        private static ComFile _roamingCompteFile;

        private static RoamingCompteModel _roamingCompte;


        #region général au roaming
        /// <summary>
        /// Charger les données liées au roaming
        /// </summary>
        /// <returns>la task</returns>
        private static async Task DemarrageRoaming()
        {
            if (_roamingCompte == null)
            {
                _roamingCompte = new RoamingCompteModel();
            }

            if (_roamingCompteFile == null)
            {
                _roamingCompteFile = new ComFile(ContexteStatic.FichierCompteRoaming, ComFile.PlaceEcriture.Roaming);

                var sqlite = await ComSqlite.GetComSqlite();
                var dbExist = await sqlite.CheckDbExist();

                //controle du fichier (si il est vide et que la base de donnée (si elle existe) contient des données, on syncrhonise)
                if ((!await _roamingCompteFile.FileExist() || (await _roamingCompteFile.FileExist() && await _roamingCompteFile.GetSizeFile() == 0)) 
                        && App.ModeApp == AppareilEnum.ModeAppareilPrincipal 
                        && dbExist)
                {
                    var business = new BanqueBusiness();
                    await business.Initialization;
                    var listeBanque = await business.GetBanques();
                    foreach (var banque in listeBanque)
                    {
                        await AjouterBanque(banque);
                        var listeCompte = await business.GetCompteFmBanque(banque.Id);

                        foreach (var compte in listeCompte)
                        {
                            await AjouterCompte(compte);
                        }
                    }
                }
            }

            await LoadFileCompte();
        }

        /// <summary>
        /// Efface les données de roaming
        /// </summary>
        public static async Task DeleteRoaming()
        {
            await DemarrageRoaming();
            await _roamingCompteFile.DeleteFile();
            _roamingCompteFile = null;
            _roamingCompte = null;
        }


        /// <summary>
        /// Sauvegarde le fichier contenant les informations de comptes
        /// </summary>
        private static async Task SaveFile()
        {
            var xs = new XmlSerializer(typeof(RoamingCompteModel));
            var wr = new StringWriter();
            xs.Serialize(wr, _roamingCompte);
            var data = CryptUtils.AesEncryptStringToByteArray(wr.ToString(), ContexteStatic.CleChiffrement,
                ContexteStatic.CleChiffrement);
            await _roamingCompteFile.Ecrire(data, CreationCollisionOption.ReplaceExisting);
        }

        /// <summary>
        /// Charge les données du fichier de compte en roaming s'il existe
        /// </summary>
        private static async Task LoadFileCompte()
        {
            if (await _roamingCompteFile.FileExist() && await _roamingCompteFile.GetSizeFile() > 0)
            {
                var inFile = await _roamingCompteFile.LireByteArray();
                var xmlIn = CryptUtils.AesDecryptByteArrayToString(inFile, ContexteStatic.CleChiffrement,
                    ContexteStatic.CleChiffrement);

                var xsb = new XmlSerializer(typeof(RoamingCompteModel));
                var rd = new StringReader(xmlIn);
                _roamingCompte = xsb.Deserialize(rd) as RoamingCompteModel;
            }  
        }

        /// <summary>
        /// Retourne en % l'espace occupé par le fichier dans le dossier de roaming
        /// </summary>
        /// <returns></returns>
        public static async Task<int> GetEspaceFichierOccupePourcent()
        {
            await DemarrageRoaming();
            var espaceMax = (ApplicationData.Current.RoamingStorageQuota > 0)
                ? ApplicationData.Current.RoamingStorageQuota * 1000 : ContexteStatic.RoaminsStorageQuotaBis * 1000;
            var espaceOccupe = await _roamingCompteFile.GetSizeFile();
            var ret = (100*espaceOccupe)/espaceMax;
            return (espaceOccupe > 0 && ret == 0) ? 1 :(int) ret;
        }

        #endregion
        
        #region banque

        /// <summary>
        /// Retourne la liste des banques
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Banque>> GetListeBanques()
        {
            await DemarrageRoaming();

            foreach (var compte in _roamingCompte.ListeBanque.SelectMany(banque => banque.ListeCompte))
            {
                compte.DeviseToAffiche = DeviseUtils.GetDiminutifDevise(compte.IdDevise);
            }

            return _roamingCompte.ListeBanque;
        }

        /// <summary>
        /// Retourne une liste de comptes pour les virements en excluant le compte ouvert
        /// </summary>
        /// <param name="idCompteExclus">l'id du comtpe à exclure</param>
        /// <returns>la liste des banqes avec la liste des comptes</returns>
        public static async Task<List<Banque>> GetListeBanqueCompteVirement(int idCompteExclus)
        {
            await DemarrageRoaming();
            return _roamingCompte.ListeBanque.Select(banque => new Banque
            {
                Id = banque.Id, IdDevise = banque.IdDevise, Nom = banque.Nom, ListeCompte = new List<Compte>(banque.ListeCompte.Where(x => x.Id != idCompteExclus).ToList())
            }).ToList();
        }

        /// <summary>
        /// Ajoute une banque au fichier de roaming
        /// </summary>
        /// <param name="banque">la banque à ajouter</param>
        public static async Task AjouterBanque(Banque banque)
        {
            await DemarrageRoaming();
            var newBanque = new Banque
            {
                Id = banque.Id,
                IdDevise = banque.IdDevise,
                IdPays = banque.IdPays,
                Nom = banque.Nom,
                ListeCompte = new List<Compte>()
            };

            _roamingCompte.ListeBanque.Add(newBanque);
            await SaveFile();
        }

        /// <summary>
        /// modifie les données d'une banque dans le fichier de roaming
        /// </summary>
        /// <param name="banque">la banque à modifier</param>
        /// <returns></returns>
        public static async Task ModifierBanque(Banque banque)
        {
            await DemarrageRoaming();
            _roamingCompte.ListeBanque.First(x => x.Id == banque.Id).Nom = banque.Nom;
            _roamingCompte.ListeBanque.First(x => x.Id == banque.Id).IdDevise = banque.IdDevise;
            _roamingCompte.ListeBanque.First(x => x.Id == banque.Id).IdPays = banque.IdPays;
            await SaveFile();
        }

        /// <summary>
        /// supprime les données d'une banque dans le fichier de roaming
        /// </summary>
        /// <param name="banque">la banque à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerBanque(Banque banque)
        {
            await DemarrageRoaming();
            _roamingCompte.ListeBanque.RemoveAll(x => x.Id == banque.Id);
            await SaveFile();
        }
        #endregion

        #region Comptes

        /// <summary>
        /// Ajoute  un compte au fichier de roaming
        /// </summary>
        /// <param name="compte">le comtpe à ajouter</param>
        /// <returns>la task</returns>
        public static async Task AjouterCompte(Compte compte)
        {
            await DemarrageRoaming();
            _roamingCompte.ListeBanque.First(x => x.Id == compte.IdBanque).ListeCompte.Add(compte);
            await SaveFile();
        }

        /// <summary>
        /// Modifie un compte dans le fichier de roaming
        /// </summary>
        /// <param name="compte">le comtpe à modifier</param>
        /// <returns>la task</returns>
        public static async Task ModifierCompte(Compte compte)
        {
            await DemarrageRoaming();
            //ajout de la banque si elle n'existe pas
            if (_roamingCompte.ListeBanque.Count(x => x.Id == compte.IdBanque) == 0)
            {
                var business = new BanqueBusiness();
                await business.Initialization;
                var banque = await business.GetBanque(compte.IdBanque);
                await AjouterBanque(banque);
            }
            //si la banque existe mais pas le compte, ont ajoute le compte
            if (_roamingCompte.ListeBanque.Count(x => x.Id == compte.IdBanque) > 0 &&
                _roamingCompte.ListeBanque.First(x => x.Id == compte.IdBanque).ListeCompte.Count(y => y.Id == compte.Id) ==
                0)
            {
                await AjouterCompte(compte);
            }

            _roamingCompte.ListeBanque.First(x => x.Id == compte.IdBanque).ListeCompte.First(y => y.Id == compte.Id).Nom
                = compte.Nom;
            _roamingCompte.ListeBanque.First(x => x.Id == compte.IdBanque).ListeCompte.First(y => y.Id == compte.Id).Solde
                = compte.Solde;
            _roamingCompte.ListeBanque.First(x => x.Id == compte.IdBanque).ListeCompte.First(y => y.Id == compte.Id).IdDevise
                = compte.IdDevise;
            await SaveFile();
        }

        /// <summary>
        /// Supprime un compte du fichier de roamin
        /// </summary>
        /// <param name="compte">le comtpe à supprimer</param>
        /// <returns>la task</returns>
        public static async Task SupprimerCompte(Compte compte)
        {
            await DemarrageRoaming();
            _roamingCompte.ListeBanque.First(x => x.Id == compte.IdBanque).ListeCompte.RemoveAll( x=> x.Id == compte.Id);
            await SaveFile();
        }
        /// <summary>
        /// Retourne un compte contenu en roaming à partir de son id
        /// </summary>
        /// <param name="idCompte">l'id du compte</param>
        /// <returns>le compte</returns>

        public static async Task<Compte> GetCompte(int idCompte)
        {
            await DemarrageRoaming();
            var compteRetour = _roamingCompte.ListeBanque.SelectMany(
                    banque => banque.ListeCompte.Where(compte => compte.Id == idCompte)).FirstOrDefault();
            compteRetour.DeviseToAffiche = DeviseUtils.GetDiminutifDevise(compteRetour.IdDevise);
            return compteRetour;
        }

        /// <summary>
        /// Retourne la devise d'un compte
        /// </summary>
        /// <param name="idCompte">l'id du compte</param>
        /// <returns>la devise</returns>
        public static async Task<string> GetDevise(int idCompte)
        {
            await DemarrageRoaming();
            var t = DeviseUtils.GetDiminutifDevise(_roamingCompte.ListeBanque.SelectMany(
                banque => banque.ListeCompte.Where(compte => compte.Id == idCompte)).FirstOrDefault().IdDevise);
            return t;
        }
        #endregion
        

    }
}
