using System.Collections.Generic;
using System.Xml.Serialization;
using CompteWin10.Model;

namespace CompteWin10.Roaming.Model
{
    [XmlRoot("RoamingMouvement")]
    public class RoamingMouvementModel
    {
        [XmlArray("listeMouvement")]
        [XmlArrayItem("mouvement")]
        public List<Mouvement> ListeMouvement { get; set; }

        public RoamingMouvementModel()
        {
            ListeMouvement = new List<Mouvement>();
        }
    }
}
