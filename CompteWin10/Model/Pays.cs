using System.Collections.Generic;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model du pays
    /// </summary>
    public class Pays
    {
        /// <summary>
        /// L'id du pays
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// le libelle du pays
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// la liste des devies du pays
        /// </summary>
        public List<Devise> Devises { get; set; }

        public Pays(string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
            Devises = new List<Devise>();
        }

        public override string ToString()
        {
            return Libelle;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(Pays other)
        {
            return string.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
