namespace CompteWin10.Model
{
    /// <summary>
    /// Model pour une classe comportant un simple id et un libelle
    /// </summary>
    public class EnumModel
    {

        public int Id { get; set; }

        public string Libelle { get; set; }
        


        public EnumModel(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

        public override string ToString()
        {
            return Libelle;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(EnumModel other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
