
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier LivreDvd hérite de Document
    /// </summary>
    public abstract class LivreDvd : Document
    {
        /// <summary>
        /// Constructeur de LivreDvd
        /// </summary>
        /// <param name="id">Id du document</param>
        /// <param name="titre">Titre du document</param>
        /// <param name="image">Chemin de l'image du document</param>
        /// <param name="idGenre">Id du genre</param>
        /// <param name="genre">Libellé du genre</param>
        /// <param name="idPublic">Id du public</param>
        /// <param name="lePublic">Libellé du public</param>
        /// <param name="idRayon">Id du rayon</param>
        /// <param name="rayon">Libellé du rayon</param>
        protected LivreDvd(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
        }

    }
}
