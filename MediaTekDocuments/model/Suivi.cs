namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi  
    /// </summary>
    public class Suivi 
    {
        public string Id { get; }
        public string Libelle { get; }
        public Suivi(string id, string libelle) 
        {
            Id = id;
            Libelle = libelle;
        }
    }
}