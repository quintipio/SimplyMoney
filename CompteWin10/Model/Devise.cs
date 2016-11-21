using System.Collections.Generic;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model d'une devise
    /// </summary>
    public class Devise
    {
        /// <summary>
        /// l'id de la devise
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// le nom complet de la devise
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// le signe de la devise
        /// </summary>
        public string Signe { get; set; }

        /// <summary>
        /// le taux de change de la devise par rapport au dollar
        /// </summary>
        public double TauxChangeEuro { get; set; }

        /// <summary>
        /// L'id du pays
        /// </summary>
        public List<string> IdPaysListe { get; set; }

        public Devise()
        {
            
        }

        public Devise(int id, string libelle, string signe, double tauxChangeEuro,List<string> idPaysListe)
        {
            Id = id;
            Libelle = libelle;
            Signe = signe;
            TauxChangeEuro = tauxChangeEuro;
            IdPaysListe = idPaysListe;
        }

        public override string ToString()
        {
            return Signe;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(Devise other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
