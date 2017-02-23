using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Media;
using CompteWin10.Business;
using CompteWin10.Com;
using CompteWin10.Enum;
using CompteWin10.Model;
using CompteWin10.Roaming.Business;

namespace CompteWin10.Context
{
    /// <summary>
    /// Object pour regrouper les listes de son
    /// </summary>
    /// <typeparam name="T">le type des objets listé</typeparam>
    public class GroupInfoList<T> : List<object>
    {
        /// <summary>
        /// le nom du groupe
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// la liste des objets
        /// </summary>
        /// <returns>la liste</returns>
        public new IEnumerator<object> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }



    /// <summary>
    /// Contexte de l'application contenant des méthdoes servant partout dans l'appli ou des données ayant la durée de vie de l'appli
    /// </summary>
    public  static class ContexteAppli
    {
        /// <summary>
        /// La liste des categories de mouvement des comptes
        /// </summary>
        public static List<Categorie> ListeCategoriesMouvement { get; private set; }
        

        /// <summary>
        /// Retourne une couleur solidCouleurBrush à partir d'un uint
        /// </summary>
        /// <param name="colorA">la couleur de base la plus sombre écrite en '0xFF00613F'</param>
        public static SolidColorBrush SetColorTheme(uint colorA)
        {

            var hex = string.Format("{0:X}", colorA);
            var cColor = Color.FromArgb(byte.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                byte.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                byte.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier),
                byte.Parse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier));
            return new SolidColorBrush(cColor);
        }



        #region les catégories

        /// <summary>
        /// Genere les categories pour les mouvement (celles par défaut, celles en base)
        /// </summary>
        public static async Task GenerateCategorieMouvement()
        {
            await ComFile.GetSizeRoamingFolder();
            await GenerateCategoriesParentDefaut();
            await GenerateSousCategoriesDefaut();
            if (App.ModeApp == AppareilEnum.ModeAppareilPrincipal)
            {
                await AjoutCategorieUserFmBdd();
            }
            else
            {
                await AjoutCategorieUserFmRoaming();
            }

            //tri par ordre alphabétique
            foreach (var category in ListeCategoriesMouvement)
            {
                category.SousCategorieList.Sort((x, y) => string.CompareOrdinal(x.Libelle, y.Libelle));
            }
            ListeCategoriesMouvement.Sort((x, y) => string.CompareOrdinal(x.Libelle, y.Libelle));
        }

        /// <summary>
        /// Génère une liste de catégories par défaut
        /// </summary>
        private static async Task GenerateCategoriesParentDefaut()
        {
            ListeCategoriesMouvement =  new List<Categorie>
            {
                new Categorie(1, ResourceLoader.GetForCurrentView("Categories").GetString("Alimentation"),false),
                new Categorie(2, ResourceLoader.GetForCurrentView("Categories").GetString("AnimauxDomestiques"),false),
                new Categorie(3, ResourceLoader.GetForCurrentView("Categories").GetString("Assurance"),false),
                new Categorie(4, ResourceLoader.GetForCurrentView("Categories").GetString("Automobile"),false),
                new Categorie(5, ResourceLoader.GetForCurrentView("Categories").GetString("Enfants"),false),
                new Categorie(6, ResourceLoader.GetForCurrentView("Categories").GetString("Etudes"),false),
                new Categorie(7, ResourceLoader.GetForCurrentView("Categories").GetString("FraisBancaire"),false),
                new Categorie(8, ResourceLoader.GetForCurrentView("Categories").GetString("Impots"),false),
                new Categorie(9, ResourceLoader.GetForCurrentView("Categories").GetString("Logement"),false),
                new Categorie(10, ResourceLoader.GetForCurrentView("Categories").GetString("Loisirs"),false),
                new Categorie(11, ResourceLoader.GetForCurrentView("Categories").GetString("Revenus"),false),
                new Categorie(12, ResourceLoader.GetForCurrentView("Categories").GetString("Santé"),false),
                new Categorie(13, ResourceLoader.GetForCurrentView("Categories").GetString("Soins"),false),
                new Categorie(14, ResourceLoader.GetForCurrentView("Categories").GetString("Transport"),false),
            };
        }

        /// <summary>
        /// Génère une liste de sous catégories par défaut
        /// </summary>
        private static async Task GenerateSousCategoriesDefaut()
        {
            var categBusiness = new CategorieBusiness();
            await categBusiness.Initialization;

            var liste = await categBusiness.GetListeSousCategToHide();


            var sousCateg = new List<SousCategorie>
            {
                new SousCategorie(1, ResourceLoader.GetForCurrentView("Categories").GetString("Bar"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(2, ResourceLoader.GetForCurrentView("Categories").GetString("Boulangerie"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(3, ResourceLoader.GetForCurrentView("Categories").GetString("Boucher"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(4, ResourceLoader.GetForCurrentView("Categories").GetString("Epicier"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(5, ResourceLoader.GetForCurrentView("Categories").GetString("Traiteur"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(6, ResourceLoader.GetForCurrentView("Categories").GetString("Restaurant"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(7, ResourceLoader.GetForCurrentView("Categories").GetString("Self"),false, ListeCategoriesMouvement[0]),
                 new SousCategorie(8, ResourceLoader.GetForCurrentView("Categories").GetString("Supermarche"),false, ListeCategoriesMouvement[0]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[0].SousCategorieList = new List<SousCategorie>(sousCateg);
            
            sousCateg = new List<SousCategorie>
            {
                new SousCategorie(9, ResourceLoader.GetForCurrentView("Categories").GetString("AlimentationAnimaux"), false, ListeCategoriesMouvement[1]),
                new SousCategorie(10, ResourceLoader.GetForCurrentView("Categories").GetString("Toilettage"), false, ListeCategoriesMouvement[1]),
                new SousCategorie(11, ResourceLoader.GetForCurrentView("Categories").GetString("Veterinaire"), false, ListeCategoriesMouvement[1]),
                new SousCategorie(12, ResourceLoader.GetForCurrentView("Categories").GetString("Fournitures"), false, ListeCategoriesMouvement[1]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[1].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                new SousCategorie(13, ResourceLoader.GetForCurrentView("Categories").GetString("Voiture"), false, ListeCategoriesMouvement[2]),
                new SousCategorie(14, ResourceLoader.GetForCurrentView("Categories").GetString("Habitation"), false, ListeCategoriesMouvement[2]),
                new SousCategorie(15, ResourceLoader.GetForCurrentView("Categories").GetString("ResponsabiliteCivile"), false, ListeCategoriesMouvement[2]),
                new SousCategorie(16, ResourceLoader.GetForCurrentView("Categories").GetString("Sante"), false, ListeCategoriesMouvement[2]),
                new SousCategorie(17, ResourceLoader.GetForCurrentView("Categories").GetString("Vie"), false, ListeCategoriesMouvement[2]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[2].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(18, ResourceLoader.GetForCurrentView("Categories").GetString("Essence"), false, ListeCategoriesMouvement[3]),
                 new SousCategorie(19, ResourceLoader.GetForCurrentView("Categories").GetString("Entretien"), false, ListeCategoriesMouvement[3]),
                 new SousCategorie(20, ResourceLoader.GetForCurrentView("Categories").GetString("Peage"), false, ListeCategoriesMouvement[3]),
                 new SousCategorie(21, ResourceLoader.GetForCurrentView("Categories").GetString("Reparation"), false, ListeCategoriesMouvement[3]),
                 new SousCategorie(22, ResourceLoader.GetForCurrentView("Categories").GetString("Stationnement"), false, ListeCategoriesMouvement[3]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[3].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(23, ResourceLoader.GetForCurrentView("Categories").GetString("Creche"), false, ListeCategoriesMouvement[4]),
                 new SousCategorie(24, ResourceLoader.GetForCurrentView("Categories").GetString("Nourrice"), false, ListeCategoriesMouvement[4]),
                 new SousCategorie(25, ResourceLoader.GetForCurrentView("Categories").GetString("EtudesEnfants"), false, ListeCategoriesMouvement[4]),
                 new SousCategorie(26, ResourceLoader.GetForCurrentView("Categories").GetString("FraisDiversEnfants"), false, ListeCategoriesMouvement[4]),
                 new SousCategorie(27, ResourceLoader.GetForCurrentView("Categories").GetString("CadeauxEnfants"), false, ListeCategoriesMouvement[4]),
                 new SousCategorie(28, ResourceLoader.GetForCurrentView("Categories").GetString("MedecinEnfants"), false, ListeCategoriesMouvement[4]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[4].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                    new SousCategorie(29, ResourceLoader.GetForCurrentView("Categories").GetString("Cours"), false, ListeCategoriesMouvement[5]),
                    new SousCategorie(30, ResourceLoader.GetForCurrentView("Categories").GetString("FraisScola"), false, ListeCategoriesMouvement[5]),
                    new SousCategorie(31, ResourceLoader.GetForCurrentView("Categories").GetString("Livres"), false, ListeCategoriesMouvement[5]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[5].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(32, ResourceLoader.GetForCurrentView("Categories").GetString("Emprunt"), false, ListeCategoriesMouvement[6]),
                 new SousCategorie(33, ResourceLoader.GetForCurrentView("Categories").GetString("FraisBancaires"), false, ListeCategoriesMouvement[6]),
                 new SousCategorie(34, ResourceLoader.GetForCurrentView("Categories").GetString("Remboursement"), false, ListeCategoriesMouvement[6]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[6].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                new SousCategorie(35, ResourceLoader.GetForCurrentView("Categories").GetString("ImpLoc"), false, ListeCategoriesMouvement[7]),
                new SousCategorie(36, ResourceLoader.GetForCurrentView("Categories").GetString("ImpRev"), false, ListeCategoriesMouvement[7]),
                new SousCategorie(37, ResourceLoader.GetForCurrentView("Categories").GetString("TaxFonc"), false, ListeCategoriesMouvement[7]),
                new SousCategorie(38, ResourceLoader.GetForCurrentView("Categories").GetString("AutrImp"), false, ListeCategoriesMouvement[7]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[7].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                new SousCategorie(39, ResourceLoader.GetForCurrentView("Categories").GetString("AbonnementTV"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(40, ResourceLoader.GetForCurrentView("Categories").GetString("Ameublement"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(41, ResourceLoader.GetForCurrentView("Categories").GetString("Caution"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(42, ResourceLoader.GetForCurrentView("Categories").GetString("Charges"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(43, ResourceLoader.GetForCurrentView("Categories").GetString("Chauffage"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(44, ResourceLoader.GetForCurrentView("Categories").GetString("Deco"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(45, ResourceLoader.GetForCurrentView("Categories").GetString("Demenagement"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(46, ResourceLoader.GetForCurrentView("Categories").GetString("Eau"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(47, ResourceLoader.GetForCurrentView("Categories").GetString("Elec"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(48, ResourceLoader.GetForCurrentView("Categories").GetString("Electromenager"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(49, ResourceLoader.GetForCurrentView("Categories").GetString("Equipement"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(50, ResourceLoader.GetForCurrentView("Categories").GetString("Gaz"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(51, ResourceLoader.GetForCurrentView("Categories").GetString("Hotel"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(52, ResourceLoader.GetForCurrentView("Categories").GetString("Jardin"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(53, ResourceLoader.GetForCurrentView("Categories").GetString("Loyer"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(54, ResourceLoader.GetForCurrentView("Categories").GetString("salarieDomi"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(55, ResourceLoader.GetForCurrentView("Categories").GetString("TelFix"), false, ListeCategoriesMouvement[8]),
                new SousCategorie(56, ResourceLoader.GetForCurrentView("Categories").GetString("TelMob"), false, ListeCategoriesMouvement[8]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[8].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(57, ResourceLoader.GetForCurrentView("Categories").GetString("Bowling"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(58, ResourceLoader.GetForCurrentView("Categories").GetString("Bricolage"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(59, ResourceLoader.GetForCurrentView("Categories").GetString("Cinema"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(60, ResourceLoader.GetForCurrentView("Categories").GetString("Discotheque"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(61, ResourceLoader.GetForCurrentView("Categories").GetString("EquipementLois"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(62, ResourceLoader.GetForCurrentView("Categories").GetString("Informatique"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(63, ResourceLoader.GetForCurrentView("Categories").GetString("Jeux"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(64, ResourceLoader.GetForCurrentView("Categories").GetString("Lecture"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(65, ResourceLoader.GetForCurrentView("Categories").GetString("Musee"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(66, ResourceLoader.GetForCurrentView("Categories").GetString("Parc"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(67, ResourceLoader.GetForCurrentView("Categories").GetString("Spectacle"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(68, ResourceLoader.GetForCurrentView("Categories").GetString("Sport"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(69, ResourceLoader.GetForCurrentView("Categories").GetString("Video"), false, ListeCategoriesMouvement[9]),
                 new SousCategorie(70, ResourceLoader.GetForCurrentView("Categories").GetString("Voyage"), false, ListeCategoriesMouvement[9]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[9].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(71, ResourceLoader.GetForCurrentView("Categories").GetString("Retaite"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(72, ResourceLoader.GetForCurrentView("Categories").GetString("Interets"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(73, ResourceLoader.GetForCurrentView("Categories").GetString("Salaire"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(74, ResourceLoader.GetForCurrentView("Categories").GetString("AllocChom"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(75, ResourceLoader.GetForCurrentView("Categories").GetString("AllocFam"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(76, ResourceLoader.GetForCurrentView("Categories").GetString("Jeu"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(77, ResourceLoader.GetForCurrentView("Categories").GetString("Cadeau"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(78, ResourceLoader.GetForCurrentView("Categories").GetString("Liquide"), false, ListeCategoriesMouvement[10]),
                 new SousCategorie(79, ResourceLoader.GetForCurrentView("Categories").GetString("Prime"), false, ListeCategoriesMouvement[10]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[10].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(80, ResourceLoader.GetForCurrentView("Categories").GetString("Medecin"), false, ListeCategoriesMouvement[11]),
                 new SousCategorie(81, ResourceLoader.GetForCurrentView("Categories").GetString("Hopital"), false, ListeCategoriesMouvement[11]),
                 new SousCategorie(82, ResourceLoader.GetForCurrentView("Categories").GetString("Pharmacie"), false, ListeCategoriesMouvement[11]),
                 new SousCategorie(83, ResourceLoader.GetForCurrentView("Categories").GetString("Mutuelle"), false, ListeCategoriesMouvement[11]),
                 new SousCategorie(84, ResourceLoader.GetForCurrentView("Categories").GetString("Secu"), false, ListeCategoriesMouvement[11]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[11].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                 new SousCategorie(85, ResourceLoader.GetForCurrentView("Categories").GetString("Coiffeur"), false, ListeCategoriesMouvement[12]),
                 new SousCategorie(86, ResourceLoader.GetForCurrentView("Categories").GetString("Habillement"), false, ListeCategoriesMouvement[12]),
                 new SousCategorie(87, ResourceLoader.GetForCurrentView("Categories").GetString("Parfumerie"), false, ListeCategoriesMouvement[12]),
                 new SousCategorie(88, ResourceLoader.GetForCurrentView("Categories").GetString("Onglerie"), false, ListeCategoriesMouvement[12]),
                 new SousCategorie(89, ResourceLoader.GetForCurrentView("Categories").GetString("Massage"), false, ListeCategoriesMouvement[12]),
                 new SousCategorie(90, ResourceLoader.GetForCurrentView("Categories").GetString("Esthetique"), false, ListeCategoriesMouvement[12]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[12].SousCategorieList = new List<SousCategorie>(sousCateg);

            sousCateg = new List<SousCategorie>
            {
                new SousCategorie(91, ResourceLoader.GetForCurrentView("Categories").GetString("Bus"), false, ListeCategoriesMouvement[13]),
                new SousCategorie(92, ResourceLoader.GetForCurrentView("Categories").GetString("Metro"), false, ListeCategoriesMouvement[13]),
                new SousCategorie(93, ResourceLoader.GetForCurrentView("Categories").GetString("Train"), false, ListeCategoriesMouvement[13]),
                new SousCategorie(94, ResourceLoader.GetForCurrentView("Categories").GetString("Tram"), false, ListeCategoriesMouvement[13]),
            };
            foreach (var sousCategory in sousCateg)
            {
                if (liste != null && liste.Contains(sousCategory.Id))
                {
                    sousCategory.IsHidden = true;
                }
            }
            ListeCategoriesMouvement[13].SousCategorieList = new List<SousCategorie>(sousCateg);
        }

        /// <summary>
        /// Ajoute les catégories et sous catégories n'existant qu'en base
        /// </summary>
        private static async Task AjoutCategorieUserFmBdd()
        {
            var categBusiness = new CategorieBusiness();
            await categBusiness.Initialization;

            //ajout des catégories
            var listeCateg = await categBusiness.GetCategoriePerso();
            if (listeCateg != null && listeCateg.Count > 0)
            {
                ListeCategoriesMouvement.AddRange(listeCateg);
            }

            //ajout des sous catégories
            var listeSousCateg = await categBusiness.GetSousCategoriesPerso();
            if (listeSousCateg != null && listeSousCateg.Count > 0)
            {
                foreach (var sousCategory in listeSousCateg)
                {
                    var category = ListeCategoriesMouvement.FirstOrDefault(c => c.Id == sousCategory.IdCategorie && c.IsCategPerso == sousCategory.IsCategPerso);
                    sousCategory.CategorieMere = category;
                    category.SousCategorieList.Add(sousCategory);
                } 
            }
        }

        private static async Task AjoutCategorieUserFmRoaming()
        {

            //ajout des catégories
            var listeCateg = await RoamingCategorieBusiness.GetCategorieRoaming();
            if (listeCateg != null && listeCateg.Count > 0)
            {
                ListeCategoriesMouvement.AddRange(listeCateg);
            }

            //ajout des sous catégories
            var listeSousCateg = await RoamingCategorieBusiness.GetSousCategorieRoaming();
            if (listeSousCateg != null && listeSousCateg.Count > 0)
            {
                foreach (var sousCategory in listeSousCateg)
                {
                    var category = ListeCategoriesMouvement.FirstOrDefault(c => c.Id == sousCategory.IdCategorie && c.IsCategPerso == sousCategory.IsCategPerso);
                    sousCategory.CategorieMere = category;
                    category.SousCategorieList.Add(sousCategory);
                }
            }
        }
        
        /// <summary>
        /// Genere une liste groupées des categories pour les mouvements à partir des catégories et des sous catégories
        /// </summary>
        /// <returns>une liste groupées de categories</returns>
        public static List<GroupInfoList<SousCategorie>> GenerateCategoriesGroup()
        {
            var res = new List<GroupInfoList<SousCategorie>>();

            foreach (var categ in ListeCategoriesMouvement)
            {
                var groupe = new GroupInfoList<SousCategorie> { Key = categ.Libelle };
                groupe.AddRange(categ.SousCategorieList.Where(x =>x.IsHidden == false).ToList());
                if (groupe.Count > 0)
                {
                    res.Add(groupe);
                }
            }
            return res;
        }
        #endregion


        #region roaming

        /// <summary>
        /// Charge en base les mouvements en roaming pui les efface
        /// </summary>
        public static async Task ChargerMouvementRoaming()
        {
           var listeMouvement = new List<Mouvement>(await RoamingMouvementBusiness.GetAllMouvement());

            //si des mouvements sont à charge
            if (listeMouvement.Count > 0)
            {
                var mouvementBusiness = new MouvementBusiness();
                await mouvementBusiness.Initialization;

                var listeIdMouvementAjouter = new List<int>();
                var listeIdRoamingIdBdd = new Dictionary<int,int>();
                foreach (var mouvement in listeMouvement)
                {
                    var idRoaming = mouvement.Id;
                    var id = await mouvementBusiness.SaveMouvementFmRoaming(mouvement);
                    await RoamingMouvementBusiness.SupprimerMouvementSimpleRoaming(idRoaming);
                    
                    if (mouvement.IdMouvementVirement > 0 && mouvement.IdMouvementVirement > 0 && listeIdRoamingIdBdd.ContainsKey(mouvement.IdMouvementVirement))
                    {
                        await mouvementBusiness.AssocierMouvementVirement(listeIdRoamingIdBdd[mouvement.IdMouvementVirement], id);
                    }


                    if (id != 0)
                    {
                        listeIdMouvementAjouter.Add(id);
                        listeIdRoamingIdBdd.Add(idRoaming, id);
                    }
                }

                //on recalcul les soldes
                if (listeIdMouvementAjouter.Count > 0)
                {
                    await mouvementBusiness.RecalculSoldesComptes(listeIdMouvementAjouter);
                }
            }

        }

        #endregion


        #region Echeancier

        /// <summary>
        /// Execute les échéanciers n'ayant pas été éxécuté depuis le dernier démarrage de l'appli
        /// </summary>
        /// <returns>la task</returns>
        public static async Task ControleEcheancier()
        {
            var echeancierBusiness = new EcheancierBusiness();
            await echeancierBusiness.Initialization;
            await echeancierBusiness.ExecuteEcheancier();
        }

        #endregion
    }
}
