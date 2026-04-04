namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Etat (état d'usure d'un document)
    /// </summary>
    public class Etat
    {
        /// <summary>
        /// Id de l'état
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Libellé de l'état
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Constructeur de l'état
        /// </summary>
        /// <param name="id">Id de l'état</param>
        /// <param name="libelle">Libellé de l'état</param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

    }
}