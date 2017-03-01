using System;
using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model de mouvement
    /// </summary>
    [Table("mouvement")]
    [XmlRoot("mouvement")]
    public class Mouvement
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("idmouvement")]
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
        [Column("idtype"),NotNull]
        [XmlElement("idType")]
        public int IdType { get; set; }

        /// <summary>
        /// indique si la catégorie est perso ou non
        /// </summary>
        [Column("istypeperso"), NotNull]
        [XmlElement("isTypePerso")]
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
        [XmlElement("modeMouvement")]
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
        /// le numéro de chèque / de virement
        /// </summary>
        [Column("numero")]
        [XmlElement("numero")]
        public int Numero { get; set; }

        /// <summary>
        /// Indique si le mouvement est passé
        /// </summary>
        [Column("ispasse")]
        [XmlElement("isPasse")]
        public bool IsPasse { get;set; }

        /// <summary>
        /// l'id du mouvement lié pour un virement
        /// </summary>
        [Column("idmouvementvirement")]
        [XmlElement("idMouvementVirement")]
        public int IdMouvementVirement { get; set; }

        /// <summary>
        /// Variable pour l'affichage dans la la page des mouvements du credit ou du débit
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public string MouvementChiffre { get; set; }

        /// <summary>
        /// l'objet de la sous catégorie du mouvement
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public SousCategorie Type { get; set; }

        /// <summary>
        /// le credit ou le débit (pour les stats)
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public double Chiffre { get; set; }

        /// <summary>
        /// Indique si le mouvement est modifiable ou non
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public bool Lock { get; set; }


        public override string ToString()
        {
            return Credit + " - " + Debit + " - " + IdMouvementVirement + " - " + IdType;
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
