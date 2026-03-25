
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
        /// Classe métier CommandeDocument (réunit les infomations communes à tous les CommandeDocuments : Livre, Revue, Dvd)
        /// </summary>
    public class CommandeDocument : Commande
    {
       
        public int NbExemplaire { get; }  // ← majuscule
        public string IdLivreDvd { get; }
        public string IdSuivi { get; }
        
     

        public CommandeDocument(string id, int nbExemplaire,
            string idLivreDvd, string idSuivi,
            DateTime dateCommande, double montant) : base(id, dateCommande, montant)
        {
           
            NbExemplaire = nbExemplaire;
            IdLivreDvd = idLivreDvd;
            IdSuivi = idSuivi;
           
        }
    }
}