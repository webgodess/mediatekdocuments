
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Genre : hérite de Categorie
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// Constructeur du genre
        /// </summary>
        /// <param name="id">Identifiant du genre</param>
        /// <param name="libelle">Libellé du genre</param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
