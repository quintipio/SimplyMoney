using System.Collections.Generic;
using System.Xml.Serialization;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model de donnée pour la serialization des données (import et export)
    /// </summary>
    [XmlRoot("simplymoneydata")]
    public class SauvegardeModel
    {
        [XmlElement("application")]
        public Application Application { get; set; }

        [XmlArray("listeCategorie")]
        [XmlArrayItem("categorie")]
        public List<Categorie> ListeCategorie { get; set; }

        [XmlArray("listeSousCategorie")]
        [XmlArrayItem("souscategorie")]
        public List<SousCategorie> ListeSousCategorie { get; set; }

        [XmlArray("listeBanque")]
        [XmlArrayItem("banque")]
        public List<Banque> ListeBanque { get; set; }

        [XmlArray("listeEcheancier")]
        [XmlArrayItem("echeancier")]
        public List<Echeancier> ListeEcheancier { get; set; }

        [XmlArray("listeMouvement")]
        [XmlArrayItem("mouvement")]
        public List<Mouvement> ListeMouvement { get; set; }

        [XmlArray("listeSoldeinitial")]
        [XmlArrayItem("soldeitial")]
        public List<SoldeInitial> ListeSoldeInit { get; set; }

        public SauvegardeModel()
        {
            Application = new Application();
            ListeCategorie = new List<Categorie>();
            ListeSousCategorie = new List<SousCategorie>();
            ListeBanque = new List<Banque>();
            ListeEcheancier = new List<Echeancier>();
            ListeMouvement = new List<Mouvement>();
            ListeSoldeInit = new List<SoldeInitial>();

        }
    }
}
