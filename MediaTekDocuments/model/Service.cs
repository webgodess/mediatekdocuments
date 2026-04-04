namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Service 
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Identifiant du service
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Libellé du service
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// Constructeur du service
        /// </summary>
        /// <param name="id">Identifiant du service</param>
        /// <param name="libelle">Libellé du service</param>
        public Service(string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }
}