using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace CompteWin10.Strings
{
    /// <summary>
    /// classe fournissant la liste des langues disponibles
    /// </summary>
    public static class ListeLangues
    {
        /// <summary>
        /// structure pour faire une liste des langues
        /// </summary>
        public struct LanguesStruct
        {
            public int id { get; set; }
            public string nom { get; set; }
            public string diminutif { get; set; }

            /// <summary>
            /// Structure pour créer une liste des langues
            /// </summary>
            /// <param name="id">l'id de la langue</param>
            /// <param name="nom">le nom complet de la langue</param>
            /// <param name="diminutif">le nom BCP-47</param>
            public LanguesStruct(int id, string nom, string diminutif)
                : this()
            {
                this.id = id;
                this.nom = nom;
                this.diminutif = diminutif;
            }

            /// <summary>
            /// retourne le nom de la langue de la structure
            /// </summary>
            /// <returns>le nom de la la langue</returns>
            public override string ToString()
            {
                return nom;
            }

            /// <summary>
            /// vérifie l'égalité de la structure
            /// </summary>
            /// <param name="obj">la version à comparer</param>
            /// <returns>true si égal</returns>
            public override bool Equals(object obj)
            {
                if (obj is LanguesStruct)
                {
                    return ((LanguesStruct)obj).id.Equals(id);
                }
                return false;
            }

            /// <summary>
            /// retourne le hascaode de la structure
            /// </summary>
            /// <returns>le hashcode</returns>
            public override int GetHashCode()
            {
                return id.GetHashCode() * nom.GetHashCode() * diminutif.GetHashCode();
            }
        }
        
        /// <summary>
        /// Liste des langues disponibles dans l'application
        /// </summary>
       private static List<LanguesStruct> listeLanguesDispo = new List<LanguesStruct>() {
            new LanguesStruct(1, ResourceLoader.GetForCurrentView().GetString("textEN"), "en"),
             new LanguesStruct(2, ResourceLoader.GetForCurrentView().GetString("textFR"), "fr"),
             new LanguesStruct(3, ResourceLoader.GetForCurrentView().GetString("textES"), "es"),
             new LanguesStruct(4, ResourceLoader.GetForCurrentView().GetString("textPT"), "pt")
        };

        /// <summary>
        /// retourne la liste des langues disponibles dans l'application
        /// </summary>
        /// <returns></returns>
        public static List<LanguesStruct> GetListesLangues()
        {
           return listeLanguesDispo;
        }

        /// <summary>
        /// Retourne l'obje d'une langue à apartir de son Id
        /// </summary>
        /// <param name="id">l'id de la langue</param>
        /// <returns>la langue, sinon un objet vide</returns>
        public static LanguesStruct GetLangueById(string id)
        {
            return listeLanguesDispo.First(languesStruct => languesStruct.diminutif == id);
        }

        /// <summary>
        /// change la langue de l'application
        /// </summary>
        /// <param name="langue">la nouvelle langue à appliquer</param>
        public static void ChangeLangueAppli(LanguesStruct langue)
        {
            string langueTelephone = Windows.System.UserProfile.GlobalizationPreferences.Languages[0];
            string langueAAppliquer = langue.diminutif;
            if(langueTelephone.Contains("-"))
            {
                if(langue.Equals(langueTelephone.Substring(0,langueTelephone.IndexOf('-'))))
                {
                    langueAAppliquer = langueTelephone;
                }
            }
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = langueAAppliquer;
        }
        

        /// <summary>
        /// change la langue en cours d'utilisation à partir de l'id de la langue
        /// </summary>
        /// <param name="idLangue">l'id de la langue à utilisé</param>
        public static void ChangeLangueAppli(string diminutif)
        {
            foreach (var languesStruct in listeLanguesDispo)
            {
                if (languesStruct.diminutif == diminutif)
                {
                    ChangeLangueAppli(languesStruct);
                }
                break;
            }
        }

        /// <summary>
        /// retourne la langue en cours d'utilisation 
        /// </summary>
        /// <returns>retourne la langue en cours d'utilisation</returns>
        public static LanguesStruct GetLangueEnCours()
        {
           var retour = new LanguesStruct();

            //récupération de la langue si elle a déjà été modifié manuellement
           var langueEnCours = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;

            //si aucune déjà en place, recherche dans la liste des langues utilisé du téléphone pour la première qui correspond à celle dispo
            if(string.IsNullOrWhiteSpace(langueEnCours))
            {
                //ont parcours les langues du téléphone
                var fini = false;
                foreach (string langue in Windows.System.UserProfile.GlobalizationPreferences.Languages)
                {
                    if(!fini)
                    {
                        string langueTmp = langue;

                        //ont parcours les langues de l'appli (avec régionalisation)
                        foreach (LanguesStruct l in listeLanguesDispo)
                        {
                            //si ont trouve, ont enregistre et ont arrete
                            if (l.diminutif.Equals(langueTmp))
                            {
                                langueEnCours = l.diminutif;
                                fini = true;
                            }
                        }

                        //si aucun résultat, ont recherche sans régionalisation
                        if (langueTmp.Contains("-") && !fini)
                        {
                            langueTmp = langueTmp.Substring(0, langueTmp.IndexOf('-'));
                            fini = false;
                            //ont parcours les langues de l'appli (sasn régionalisation)
                            foreach (LanguesStruct l in listeLanguesDispo)
                            {
                                //si ont trouve, ont enregistre et ont arrete
                                if (l.diminutif.Equals(langueTmp))
                                {
                                    langueEnCours = l.diminutif;
                                    fini = true;
                                }
                            }
                        }
                    }
                    
                    if(fini)
                    {
                        break;
                    }
                }
            }

            //recherche de la langue en cours dans la liste des structures (avec regionalisation)
            var dejatrouve = false;
            foreach (LanguesStruct l in listeLanguesDispo)
            {
                if (l.diminutif.ToUpper().Equals(langueEnCours.ToUpper()))
                {
                    retour = l;
                    dejatrouve = true;
                    break;
                }
            }

            //(si aucun résultat ont supprime la régionalisation)petit traitement pour pouvoir le retrouver dans la structure
            if (langueEnCours.Contains("-") && !dejatrouve)
            {
                langueEnCours = langueEnCours.Substring(0, langueEnCours.IndexOf('-'));
                //recherche de la langue en cours dans la liste des structures (sans regionalisation)
                foreach (LanguesStruct l in listeLanguesDispo)
                {
                    if (l.diminutif.Equals(langueEnCours))
                    {
                        retour = l;
                        break;
                    }
                }
            }

            //si rien ne correspond c'est que par défaut c'est l'anglais
            if(retour.id == 0)
            {
                retour = listeLanguesDispo[0];
            }
            return retour;
        }


    }
}
