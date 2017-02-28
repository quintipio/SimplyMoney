using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    [Table("souscategorie")]
    [XmlRoot("souscategorie")]
    public class SousCategorie
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("idsouscategorie")]
        public int Id { get; set; }

        /// <summary>
        /// le libellé
        /// </summary>
        [Column("libelle"), NotNull]
        [XmlElement("libelle")]
        public string Libelle { get; set; }

        /// <summary>
        /// l'id de la catégorie mère
        /// </summary>
        [Column("idcategorie"), NotNull]
        [XmlElement("idcategorie")]
        public int IdCategorie { get; set; }

        /// <summary>
        /// indique si la sous catégorie appartient à l'utilisateur
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public bool IsSousCategPerso { get; set; }

        /// <summary>
        /// indique si la sous catégorie appartient à l'utilisateur
        /// </summary>
        [Column("iscategorieperso"), NotNull]
        [XmlElement("iscategorieperso")]
        public bool IsCategPerso { get; set; }

        /// <summary>
        /// indique si l'objet peut être modifier
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public bool IsVisibleForModif { get; set; }

        /// <summary>
        /// Indique si l'objet est visible ou non
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Categorie mère de la sous catégorie
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public Categorie CategorieMere { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">l'id</param>
        /// <param name="libelle">le libelle</param>
        /// <param name="isPerso">indique si c'est une sousCategorie personnelle</param>
        /// <param name="categorie">la catégorie mère</param>
        public SousCategorie(int id, string libelle, bool isPerso, Categorie categorie)
        {
            Id = id;
            Libelle = libelle;
            IsSousCategPerso = isPerso;
            IdCategorie = categorie.Id;
            CategorieMere = categorie;
            IsCategPerso = categorie.IsCategPerso;
        }

        public SousCategorie()
        {
            
        }

        public override string ToString()
        {
            return (CategorieMere != null)?CategorieMere.Libelle+" : "+ Libelle:Libelle;
        }
        

        protected bool Equals(SousCategorie other)
        {
            return Id == other.Id && IsSousCategPerso == other.IsSousCategPerso;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ IsSousCategPerso.GetHashCode();
            }
        }
    }
}
