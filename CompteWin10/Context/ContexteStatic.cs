using System.Collections.Generic;

namespace CompteWin10.Context
{
    /// <summary>
    /// Context Static contenant les infos invariables
    /// </summary>
    public static class ContexteStatic
    {
        /// <summary>
        /// le nom de l'application
        /// </summary>
        public const string NomAppli = "Simply money";

        /// <summary>
        /// adresse de support
        /// </summary>
        public const string Support = "";

        /// <summary>
        /// version de l'application
        /// </summary>
        public const string Version = "1.2.1";

        /// <summary>
        /// nom du développeur
        /// </summary>
        public const string Developpeur = "";

        /// <summary>
        /// Nom du fichier de la base de donnée
        /// </summary>
        public const string Database = "database.db";

        /// <summary>
        /// Clé de chiffrement pour les dossiers en roaming
        /// </summary>
        public const string CleChiffrement = "";

        /// <summary>
        /// Fichier des données de compte et de banque en roaming
        /// </summary>
        public const string FichierCompteRoaming = "DataCompte.dat";

        /// <summary>
        /// Fichier des données de  mouvement en roaming
        /// </summary>
        public const string FichierMouvementRoaming = "DataMouvement.dat";

        /// <summary>
        /// Fichier des données d'échéancier en roaming
        /// </summary>
        public const string FichierEcheancierRoaming = "DataEcheancier.dat";

        /// <summary>
        /// Fichier des données de catégories en roaming
        /// </summary>
        public const string FichierCategorieRoaming = "DataCategorie.dat";

        /// <summary>
        /// Extension pour les fichiers en import ou export
        /// </summary>
        public const string ExtensionImportExport = ".spm";

        /// <summary>
        /// Indique en % l'espace max autorisé à occupé pour que l'appli autorise la synchro
        /// </summary>
        public const int EspaceMaxAutoriseRoaming = 97;

        /// <summary>
        /// pour windows phone 10, il est à 0, donc solution de remplacement
        /// </summary>
        public const ulong RoaminsStorageQuotaBis = 100;

        /// <summary>
        /// La liste des couleurs applicables pour le thème
        /// </summary>
        public static readonly List<uint> ListeCouleur = new List<uint> { 0xFF51B651, 0xFF610000, 0xFF613E00, 0xFF616100, 0xFF0D6100, 0xFF00613F, 0xFF00615D, 0xFF001661, 0xFF4F0061, 0xFF610039 };
    }
}
