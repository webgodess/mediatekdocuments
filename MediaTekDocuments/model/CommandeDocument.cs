
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
        /// Classe métier CommandeDocument (réunit les infomations communes à tous les CommandeDocuments : Livre, Revue, Dvd)
        /// </summary>
    public class CommandeDocument
    {
        public string Id { get; }
        public int NbExemplaire { get; }  // ← majuscule
        public string IdLivreDvd { get; }
        public string IdSuivi { get; }
        public DateTime DateCommande { get; }
        public double Montant { get; }

        public CommandeDocument(string id, int nbExemplaire,
            string idLivreDvd, string idSuivi,
            DateTime dateCommande, double montant)
        {
            Id = id;
            NbExemplaire = nbExemplaire;
            IdLivreDvd = idLivreDvd;
            IdSuivi = idSuivi;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}