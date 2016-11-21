namespace CompteWin10.Model
{
    /// <summary>
    /// Model des types de mouvements pour les mouvements
    /// </summary>
    public class TypeMouvement
    {
        public int Id { get; set; }

        public string Libelle { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">l'id</param>
        /// <param name="libelle">le nom du type de mouvement</param>
        public TypeMouvement(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

    }
}
