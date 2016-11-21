using System.Collections.Generic;
using System.Xml.Serialization;
using CompteWin10.Model;

namespace CompteWin10.Roaming.Model
{
    /// <summary>
    /// Model des données sérialisé pour le roaming des données de bases
    /// </summary>
    [XmlRoot("RoamingCompte")]
    public class RoamingCompteModel
    {
        [XmlArray("listeBanque")]
        [XmlArrayItem("banque")]
        public List<Banque> ListeBanque { get; set; }

        public RoamingCompteModel()
        {
            ListeBanque = new List<Banque>();
        }
    }
}
