using SQLite;

namespace CompteWin10.Model
{
    /// <summary>
    /// Model pour enregistré les soldes intitials des comptes (sert en cas de dépannage pour recalculer la solde finale)
    /// </summary>
    [Table("soldeinitial")]
    public class SoldeInitial
    {
        /// <summary>
        /// l'id du model
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        public int Id { get; set; }

        /// <summary>
        /// le solde initial du compte
        /// </summary>
        [Column("soldeinit"), NotNull]
        public double SoldeInit { get; set; }

        /// <summary>
        /// l'id du compte
        /// </summary>
        [Column("idcompte"), NotNull]
        public int IdCompte { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public SoldeInitial()
        {
            
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="idCompte">l'id du compte</param>
        /// <param name="soldeInitial">le solde initial</param>
        public SoldeInitial(int idCompte, double soldeInitial)
        {
            SoldeInit = soldeInitial;
            IdCompte = idCompte;
        }

        public override string ToString()
        {
            return IdCompte + " : " + SoldeInit;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(SoldeInitial other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
