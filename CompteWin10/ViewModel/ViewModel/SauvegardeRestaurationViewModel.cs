using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using CompteWin10.Abstract;
using CompteWin10.Business;
using CompteWin10.Context;
using CompteWin10.Model;
using CompteWin10.Strings;
using CompteWin10.Utils;

namespace CompteWin10.ViewModel
{
    /// <summary>
    /// ViewModel de l'import et de l'export
    /// </summary>
    public partial class SauvegardeRestaurationViewModel : AbstractViewModel
    {
        private ApplicationBusiness _applicationBusiness;

        private CategorieBusiness _categorieBusiness;

        private BanqueBusiness _banqueBusiness;

        private CompteBusiness _compteBusiness;

        private EcheancierBusiness _echeancierBusiness;

        private MouvementBusiness _mouvementBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public SauvegardeRestaurationViewModel()
        {
            Initialization = InitializeAsync();
        }

        public sealed async override Task InitializeAsync()
        {
            _applicationBusiness = new ApplicationBusiness();
            await _applicationBusiness.Initialization;

            _categorieBusiness = new CategorieBusiness();
            await _categorieBusiness.Initialization;

            _banqueBusiness = new BanqueBusiness();
            await _banqueBusiness.Initialization;

            _compteBusiness = new CompteBusiness();
            await _compteBusiness.Initialization;

            _echeancierBusiness = new EcheancierBusiness();
            await _echeancierBusiness.Initialization;

            _mouvementBusiness = new MouvementBusiness();
            await _mouvementBusiness.Initialization;
        }

        /// <summary>
        /// Controle les données avant l'import ou l'export
        /// </summary>
        /// <returns>les erreurs</returns>
        public string Validate()
        {
            var retour = "";

            if (IsMdp && string.IsNullOrWhiteSpace(MotDePasse))
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("AucunMdp")+"\r\n";
            }

            if (Fichier == null)
            {
                retour += ResourceLoader.GetForCurrentView("Errors").GetString("AucunFichier") + "\r\n";
            }

            return retour;
        }

        /// <summary>
        /// importe les données
        /// </summary>
        /// <returns>true si ok</returns>
        public async Task<bool> Restauration()
        {
            try
            {
                //deserialization
                string xml;
                if (IsMdp)
                {
                    var buffer = await FileIO.ReadBufferAsync(Fichier);
                    byte[] inFile = buffer.ToArray();
                    xml = CryptUtils.AesDecryptByteArrayToString(inFile, MotDePasse, MotDePasse);
                }
                else
                {
                    xml = await FileIO.ReadTextAsync(Fichier, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
                var xsb = new XmlSerializer(typeof (SauvegardeModel));
                var rd = new StringReader(xml);
                var resultImport = xsb.Deserialize(rd) as SauvegardeModel;

                //effacement des données
                await _applicationBusiness.DeleteForRestauration();

                //restauration
                //appli
                await _applicationBusiness.ChangeIdCouleurBackground(resultImport.Application.IdBackGround);
                await _applicationBusiness.ChangeIdLangue(ListeLangues.GetLangueById(resultImport.Application.IdLangue));
                ListeLangues.ChangeLangueAppli(resultImport.Application.IdLangue);

                //banque et compte
                foreach (var banque in resultImport.ListeBanque)
                {
                    await _banqueBusiness.AjouterBanqueFmRestauration(banque);
                    foreach (var compte in banque.ListeCompte)
                    {
                        await _compteBusiness.AjouterCompteFmRestauration(compte);
                    }
                }

                //solde init
                foreach (var soldeInitial in resultImport.ListeSoldeInit)
                {
                    await _compteBusiness.AjouterSoldeInitialFmRestauration(soldeInitial);
                }

                //categorie
                foreach (var category in resultImport.ListeCategorie)
                {
                    await _categorieBusiness.AjouterCategorieFmRestauration(category);
                }

                //sous categorie
                foreach (var sousCategory in resultImport.ListeSousCategorie)
                {
                    await _categorieBusiness.AjouterSousCategorieFmRestauration(sousCategory);
                }

                //échéancier
                foreach (var echeancier in resultImport.ListeEcheancier)
                {
                    await _echeancierBusiness.AjouterEcheancierFmRestauration(echeancier);
                }

                //mouvement
                foreach (var mouvement in resultImport.ListeMouvement)
                {
                    await _mouvementBusiness.AjouterMouvementFmRestauration(mouvement);
                }

                //regénère les catégories
                await ContexteAppli.GenerateCategorieMouvement();
                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// exporte les données
        /// </summary>
        /// <returns>true si ok</returns>
        public async Task<bool> Sauvegarde()
        {
            try
            {
                var data = new SauvegardeModel
                {
                    Application =
                    {
                        IdLangue = await _applicationBusiness.GetLangueAppli(),
                        IdBackGround = await _applicationBusiness.GetIdCouleurBackGround()
                    },
                    ListeCategorie = await _categorieBusiness.GetCategoriePerso(),
                    ListeSousCategorie = await _categorieBusiness.GetSousCategoriesPerso(),
                    ListeBanque = await _compteBusiness.GetResumeCompte(),
                    ListeEcheancier = await _echeancierBusiness.GetEcheancier(),
                    ListeMouvement = await _mouvementBusiness.GetListeMouvement(),
                    ListeSoldeInit = await _compteBusiness.GetAllSoldeInitial()
                };

                var xs = new XmlSerializer(typeof(SauvegardeModel));
                var wr = new StringWriter();
                xs.Serialize(wr, data);

                if (IsMdp)
                {
                    var dataCrypt = CryptUtils.AesEncryptStringToByteArray(wr.ToString(), MotDePasse, MotDePasse);
                    await FileIO.WriteBytesAsync(Fichier, dataCrypt);
                }
                else
                {
                    await FileIO.WriteTextAsync(Fichier, wr.ToString(), Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


    }
}
