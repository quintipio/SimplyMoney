using System.Threading.Tasks;
using Windows.Storage;
using CompteWin10.Com;
using CompteWin10.Context;

namespace CompteWin10.Utils
{
    /// <summary>
    /// Classe utilitaire pour le roaming
    /// </summary>
    public static class RoamingUtils
    {
        /// <summary>
        /// Donne en pourcent l'espace occupé par les données dans le roaming
        /// </summary>
        /// <returns>le pourcentage</returns>
        public static async Task<int> GetEspaceRoamingOccupePourcent()
        {
            var espaceMax = (ApplicationData.Current.RoamingStorageQuota > 0)
                ? ApplicationData.Current.RoamingStorageQuota * 1000 : ContexteStatic.RoaminsStorageQuotaBis * 1000;
            var espaceOccupe = await ComFile.GetSizeRoamingFolder();
            return (int)((100 * espaceOccupe) / espaceMax);

        }

        /// <summary>
        /// Indique si la syncrho est autorisée
        /// </summary>
        /// <returns>true si elle peut s'effectuée</returns>
        public static async Task<bool> IsEcritureRoamingAutorise()
        {
            return (await GetEspaceRoamingOccupePourcent() < ContexteStatic.EspaceMaxAutoriseRoaming);
        }
    }
}
