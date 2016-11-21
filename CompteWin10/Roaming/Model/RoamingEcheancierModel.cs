using System.Collections.Generic;
using System.Xml.Serialization;
using CompteWin10.Model;

namespace CompteWin10.Roaming.Model
{
    /// <summary>
    /// Model pour la suavegarde en xml des échéanciers
    /// </summary>
    [XmlRoot("RoamingEcheancier")]
    public class RoamingEcheancierModel
    {
        [XmlArray("listeEcheancier")]
        [XmlArrayItem("echeancier")]
        public List<Echeancier> ListeEcheancier { get; set; }

        public RoamingEcheancierModel()
        {
            ListeEcheancier = new List<Echeancier>();
        }
    }
}
