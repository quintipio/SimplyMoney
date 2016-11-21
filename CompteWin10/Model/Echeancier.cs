using System;
using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model de l'échéancier
    /// </summary>
    [Table("echeancier")]
    [XmlRoot("echeancier")]
    public class Echeancier
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("idecheancier")]
        public int Id { get; set; }

        /// <summary>
        /// l'id du compte lié
        /// </summary>
        [Column("idcompte"), NotNull]
        [XmlElement("idCompte")]
        public int IdCompte { get; set; }

        /// <summary>
        /// la date du mouvement
        /// </summary>
        [Column("date"), NotNull]
        [XmlElement("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// la categorie de mouvement
        /// </summary>
        [Column("idtype")]
        [XmlElement("idtype")]
        public int IdType { get; set; }

        /// <summary>
        /// indique si la catégorie est perso ou non
        /// </summary>
        [Column("istypeperso"), NotNull]
        [XmlElement("istypeperso")]
        public bool IsTypePerso { get; set; }

        /// <summary>
        /// le commentaire
        /// </summary>
        [Column("commentaire")]
        [XmlElement("commentaire")]
        public string Commentaire { get; set; }

        /// <summary>
        /// Mode de mouvement
        /// </summary>
        [Column("modemouvement"), NotNull]
        [XmlElement("modemouvement")]
        public int ModeMouvement { get; set; }

        /// <summary>
        /// le credit
        /// </summary>
        [Column("credit")]
        [XmlElement("credit")]
        public double Credit { get; set; }

        /// <summary>
        /// le debit
        /// </summary>
        [Column("debit")]
        [XmlElement("debit")]
        public double Debit { get; set; }
        
        /// <summary>
        /// En cas de virement, le comtpe du virement
        /// </summary>
        [Column("idcomptevirement")]
        [XmlElement("idcompteVirement")]
        public int IdCompteVirement { get; set; }
        
        /// <summary>
        /// la periodicite du mouvement
        /// </summary>
        [Column("idperiodicite")]
        [XmlElement("idperiodicite")]
        public int IdPeriodicite { get; set; }

        /// <summary>
        /// la date limite de l'échéancier
        /// </summary>
        [Column("dateLimite")]
        [XmlElement("datelimite")]
        public DateTime DateLimite { get; set; }

        /// <summary>
        /// la date limite de l'échéancier
        /// </summary>
        [Column("isdateLimite")]
        [XmlElement("isdatelimite")]
        public bool IsDateLimite { get; set; }

        /// <summary>
        /// le nombre de jours pour une périodicité personalisé
        /// </summary>
        [Column("nbjours")]
        [XmlElement("nbjours")]
        public int NbJours { get; set; }
        

        /// <summary>
        /// Affichage du crédit ou du débit
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public string MouvementChiffre { get; set; }

        /// <summary>
        /// l'objet de la sous catégorie de l'échéancier
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public SousCategorie Type { get; set; }


        public override string ToString()
        {
            return Credit + " - " + Debit + " - " + IdType;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(Mouvement other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
