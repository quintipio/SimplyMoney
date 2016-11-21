using System;
using System.Xml.Serialization;
using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model de application
    /// </summary>
    [Table("Application")]
    [XmlRoot("application")]
    public class Application
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Le mode d'utilisation de l'appareil
        /// </summary>
        [Column("mode")]
        [XmlIgnore]
        public int ModeAppareil { get; set; }

        /// <summary>
        /// le numéro de version de l'appli
        /// </summary>
        [Column("version")]
        [XmlIgnore]
        public string Version { get; set; }

        /// <summary>
        /// le numéro de version de l'appli
        /// </summary>
        [Column("idBackGround")]
        [XmlElement("idBackGround")]
        public int IdBackGround { get; set; }

        /// <summary>
        /// la langue de l'appli
        /// </summary>
        [Column("idLangue")]
        [XmlElement("idlangue")]
        public string IdLangue { get; set; }

        /// <summary>
        /// Date du dernier demarrage de l'appli
        /// </summary>
        [Column("datedernierdemarrage")]
        [XmlIgnore]
        public DateTime DateDernierDemarrage { get; set; }

        /// <summary>
        /// Active ou non la synchro du fichier des catégories
        /// </summary>
        [Column("issynchrocategorieactive")]
        [XmlElement("issynchrocategorieactive")]
        public bool IsSynchroCategorieActive { get; set; }

        /// <summary>
        /// Active ou non la synchro du fichier des échéanciers
        /// </summary>
        [Column("issynchroecheancieractive")]
        [XmlElement("issynchroecheancieractive")]
        public bool IsSynchroEcheancierActive { get; set; }
    }
}
