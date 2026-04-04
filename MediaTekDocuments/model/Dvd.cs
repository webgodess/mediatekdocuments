
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Dvd hérite de LivreDvd : contient des propriétés spécifiques aux dvd
    /// </summary>
    public class Dvd : LivreDvd
    {
        /// <summary>
        /// Durée du DVD en minutes
        /// </summary>
        public int Duree { get; }

        /// <summary>
        /// Réalisateur du DVD
        /// </summary>
        public string Realisateur { get; }

        /// <summary>
        /// Synopsis du DVD
        /// </summary>
        public string Synopsis { get; }

        /// <summary>
        /// Constructeur du DVD
        /// </summary>
        /// <param name="id">Id du DVD</param>
        /// <param name="titre">Titre du DVD</param>
        /// <param name="image">Chemin de l'image du DVD</param>
        /// <param name="duree">Durée du DVD en minutes</param>
        /// <param name="realisateur">Réalisateur du DVD</param>
        /// <param name="synopsis">Synopsis du DVD</param>
        /// <param name="idGenre">Id du genre</param>
        /// <param name="genre">Libellé du genre</param>
        /// <param name="idPublic">Id du public</param>
        /// <param name="lePublic">Libellé du public</param>
        /// <param name="idRayon">Id du rayon</param>
        /// <param name="rayon">Libellé du rayon</param>
        public Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Duree = duree;
            this.Realisateur = realisateur;
            this.Synopsis = synopsis;
        }

    }
}
