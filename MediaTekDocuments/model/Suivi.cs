
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi  
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Id du suivi
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Libellé du suivi
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// Constructeur du suivi
        /// </summary>
        /// <param name="id">Id du suivi</param>
        /// <param name="libelle">Libellé du suivi</param>
        public Suivi(string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }
}
