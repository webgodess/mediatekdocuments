namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Identifiant du document
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Titre du document
        /// </summary>
        public string Titre { get; }

        /// <summary>
        /// Chemin de l'image du document
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// Identifiant du genre du document
        /// </summary>
        public string IdGenre { get; }

        /// <summary>
        /// Libellé du genre du document
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// Identifiant du public du document
        /// </summary>
        public string IdPublic { get; }

        /// <summary>
        /// Libellé du public du document
        /// </summary>
        public string Public { get; }

        /// <summary>
        /// Identifiant du rayon du document
        /// </summary>
        public string IdRayon { get; }

        /// <summary>
        /// Libellé du rayon du document
        /// </summary>
        public string Rayon { get; }

        /// <summary>
        /// Constructeur du document
        /// </summary>
        /// <param name="id">Identifiant du document</param>
        /// <param name="titre">Titre du document</param>
        /// <param name="image">Chemin de l'image du document</param>
        /// <param name="idGenre">Identifiant du genre</param>
        /// <param name="genre">Libellé du genre</param>
        /// <param name="idPublic">Identifiant du public</param>
        /// <param name="lePublic">Libellé du public</param>
        /// <param name="idRayon">Identifiant du rayon</param>
        /// <param name="rayon">Libellé du rayon</param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}