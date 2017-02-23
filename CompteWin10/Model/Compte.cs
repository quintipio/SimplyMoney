using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model des comptes
    /// </summary>
    [Table("compte")]
    [XmlRoot("compte")]
    public class Compte
    {

        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("idcompte")]
        public int Id { get; set; }

        /// <summary>
        /// le nom du compte
        /// </summary>
        [ Column("nom"), NotNull]
        [XmlElement("nom")]
        public string Nom { get; set; }

        /// <summary>
        /// l'id de la banque
        /// </summary>
        [Column("idbanque"), NotNull]
        [XmlElement("idbanque")]
        public int IdBanque { get; set; }

        /// <summary>
        /// le solde de départ du compte
        /// </summary>
        [Column("solde")]
        [XmlElement("solde")]
        public double Solde { get; set; }

        /// <summary>
        /// la devise du compte
        /// </summary>
        [Column("iddevise")]
        [XmlElement("iddevise")]
        public int IdDevise { get; set; }

        /// <summary>
        /// La devise à afficher dans le résumé des comptes
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public string DeviseToAffiche { get; set; }

        public Compte()
        {
            
        }

        public Compte(Compte compte)
        {
            Id = compte.Id;
            Nom = compte.Nom;
            IdBanque = compte.IdBanque;
            Solde = compte.Solde;
            IdDevise = compte.IdDevise;
            DeviseToAffiche = compte.DeviseToAffiche;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Compte) obj);
        }
        protected bool Equals(Compte other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
