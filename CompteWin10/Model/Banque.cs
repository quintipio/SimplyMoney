using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model des banques
    /// </summary>
    [Table("banque")]
    [XmlRoot("Banque")]
    public class Banque
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"),NotNull]
        [XmlElement("idBanque")]

        public int Id { get; set; }

        /// <summary>
        /// le nom
        /// </summary>
        [ Column("nom"), NotNull]
        [XmlElement("nomBanque")]
        public string Nom { get; set; }

        /// <summary>
        /// l'id de la devise de la monnaie
        /// </summary>
        [Column("devise"), NotNull]
        [XmlElement("deviseBanque")]
        public int IdDevise { get; set; }

        /// <summary>
        /// l'id du pays
        /// </summary>
        [Column("pays"), NotNull]
        [XmlElement("paysBanque")]
        public string IdPays { get; set; }

        /// <summary>
        /// La liste des comptes associé à la banque (sert uniquement pour la serialization)
        /// </summary>
        [XmlArray("listeCompte")]
        [XmlArrayItem("compte")]
        [Ignore]
        public List<Compte> ListeCompte { get; set; }

        [XmlIgnore]
        [Ignore]
        public string SoldeBanque { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Banque) obj);
        }
        
        protected bool Equals(Banque other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
