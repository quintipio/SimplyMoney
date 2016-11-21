
using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model des catégories de mouvement
    /// </summary>
    [Table("categorie")]
    [XmlRoot("categorie")]
    public class Categorie
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("idcategorie")]
        public int Id { get; set; }

        /// <summary>
        /// le nom de la catégorie
        /// </summary>
        [Column("libelle"), NotNull]
        [XmlElement("libelle")]
        public string Libelle { get; set; }

        /// <summary>
        /// indique si elle est crée par l'utilisateur
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public bool IsCategPerso { get; set; }

        /// <summary>
        /// Liste des sous catégories
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public List<SousCategorie> SousCategorieList { get; set; }

        /// <summary>
        /// indique si l'objet peut être modifier
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public bool IsVisibleForModif { get; set; }

        /// <summary>
        /// Constructeur pour une catégorie générale
        /// </summary>
        /// <param name="id">l'id de la catégorie</param>
        /// <param name="libelleCategorie">le libellé de la catégorie</param>
        public Categorie(int id, string libelleCategorie,bool isCategPerso)
        {
            Id = id;
            Libelle = libelleCategorie;
            IsCategPerso = isCategPerso;
            SousCategorieList = new List<SousCategorie>();
        }

        public Categorie()
        {
            SousCategorieList = new List<SousCategorie>();
        }

        public override string ToString()
        {
            return Libelle;
        }

        protected bool Equals(Categorie other)
        {
            return Id == other.Id && IsCategPerso == other.IsCategPerso;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ IsCategPerso.GetHashCode();
            }
        }
    }
}
