using System;

namespace CompteWin10.Utils
{
    /// <summary>
    /// Utils des dates
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// retourne l'objet DateTime à une heure précise
        /// </summary>
        /// <returns>La date précise</returns>
        public static DateTime GetMaintenant()
        {
            //return new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
            return DateTime.Now;
        }


        /// <summary>
        /// Ajoute un nombre de jours à une date pour obtenir une nouvelle date
        /// </summary>
        /// <param name="date">la date de base</param>
        /// <param name="nbJours">le nombre de jours à ajouter</param>
        /// <returns>la nouvelle date</returns>
        public static DateTime AjouterJours(DateTime date, int nbJours)
        {
            return date.AddDays(nbJours);
        }

        /// <summary>
        /// Ajoute un nombre de mois à une date pour obtenir une nouvelle date
        /// </summary>
        /// <param name="date">la date de base</param>
        /// <param name="nbMois">le nombre de mois à ajouter</param>
        /// <returns>la nouvelle date</returns>
        public static DateTime AjouterMois(DateTime date, int nbMois)
        {
            return date.AddMonths(nbMois);
        }

        /// <summary>
        /// Ajoute un nombre d'années à une date pour obtenir une nouvelle date
        /// </summary>
        /// <param name="date">la date de base</param>
        /// <param name="nbAnnee">le nombre d'années à ajouter</param>
        /// <returns>la nouvelle date</returns>
        public static DateTime AjouterAnnee(DateTime date, int nbAnnee)
        {
            return date.AddYears(nbAnnee);
        }


        /// <summary>
        /// soustrait à une date un nombre de jours
        /// </summary>
        /// <param name="date">la date max</param>
        /// <param name="nbJours">le nombre de jours à soustraire</param>
        /// <returns>la dateMin</returns>
        public static DateTime SoustraireJours(DateTime date, int nbJours)
        {
            return date.Subtract(TimeSpan.FromDays(nbJours));
        }
        



        /// <summary>
        /// Converti uen chaine de caractère ex : "01/08/2008" en DateTime
        /// </summary>
        /// <param name="date">La date à convertir</param>
        /// <returns>La DateTime</returns>
        public static DateTime StringEnDate(string date)
        {
            return Convert.ToDateTime(date);
        }

        /// <summary>
        /// Arrondir une date
        /// </summary>
        /// <param name="date">la date à modifier</param>
        /// <returns>la date</returns>
        public static DateTime ArrondirJour(DateTime date)
        {
            return new DateTime(date.Year,date.Month,date.Day,23,59,59);
        }

    }
}
