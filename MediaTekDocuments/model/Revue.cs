namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue : Document
    {
        /// <summary>
        /// Périodicité de la revue
        /// </summary>
        public string Periodicite { get; set; }

        /// <summary>
        /// Délai de mise à disposition de la revue en jours
        /// </summary>
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Constructeur de la revue
        /// </summary>
        /// <param name="id">Id de la revue</param>
        /// <param name="titre">Titre de la revue</param>
        /// <param name="image">Chemin de l'image de la revue</param>
        /// <param name="idGenre">Id du genre</param>
        /// <param name="genre">Libellé du genre</param>
        /// <param name="idPublic">Id du public</param>
        /// <param name="lePublic">Libellé du public</param>
        /// <param name="idRayon">Id du rayon</param>
        /// <param name="rayon">Libellé du rayon</param>
        /// <param name="periodicite">Périodicité de la revue</param>
        /// <param name="delaiMiseADispo">Délai de mise à disposition en jours</param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            DelaiMiseADispo = delaiMiseADispo;
        }

    }
}