
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Livre hérite de LivreDvd : contient des propriétés spécifiques aux livres
    /// </summary>
    public class Livre : LivreDvd
    {
        /// <summary>
        /// ISBN du livre
        /// </summary>
        public string Isbn { get; }

        /// <summary>
        /// Auteur du livre
        /// </summary>
        public string Auteur { get; }

        /// <summary>
        /// Collection du livre
        /// </summary>
        public string Collection { get; }

        /// <summary>
        /// Constructeur du livre
        /// </summary>
        /// <param name="id">Id du livre</param>
        /// <param name="titre">Titre du livre</param>
        /// <param name="image">Chemin de l'image du livre</param>
        /// <param name="isbn">ISBN du livre</param>
        /// <param name="auteur">Auteur du livre</param>
        /// <param name="collection">Collection du livre</param>
        /// <param name="idGenre">Id du genre</param>
        /// <param name="genre">Libellé du genre</param>
        /// <param name="idPublic">Id du public</param>
        /// <param name="lePublic">Libellé du public</param>
        /// <param name="idRayon">Id du rayon</param>
        /// <param name="rayon">Libellé du rayon</param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Isbn = isbn;
            this.Auteur = auteur;
            this.Collection = collection;
        }



    }
}
