using System.Collections.Generic;
using System.Xml.Serialization;
using CompteWin10.Model;

namespace CompteWin10.Roaming.Model
{
    /// <summary>
    /// Model pour la sérialisation des catégories
    /// </summary>
    [XmlRoot("RoamingCategorie")]
    public class RoamingCategorieModel
    {
        [XmlArray("listeCategorie")]
        [XmlArrayItem("categorie")]
        public List<Categorie> ListeCategorie{ get; set; }

        [XmlArray("listeSousCategorie")]
        [XmlArrayItem("souscategorie")]
        public List<SousCategorie> ListeSousCategorie { get; set; }

        public RoamingCategorieModel()
        {
            ListeCategorie = new List<Categorie>();
            ListeSousCategorie = new List<SousCategorie>();
        }
    }
}
