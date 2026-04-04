
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument (réunit les infomations communes à tous les CommandeDocuments : Livre, Revue, Dvd)
    /// </summary>
    public class CommandeDocument : Commande
    {
        /// <summary>
        /// Nombre d'exemplaires commandés
        /// </summary>
        public int NbExemplaire { get; }

        /// <summary>
        /// Id du livre ou DVD associé à la commande
        /// </summary>
        public string IdLivreDvd { get; }

        /// <summary>
        /// Id de l'étape de suivi de la commande
        /// </summary>
        public string IdSuivi { get; }

        /// <summary>
        /// Constructeur de la commande de document
        /// </summary>
        /// <param name="id">Id de la commande</param>
        /// <param name="nbExemplaire">Nombre d'exemplaires commandés</param>
        /// <param name="idLivreDvd">Id du livre ou DVD</param>
        /// <param name="idSuivi">Id de l'étape de suivi</param>
        /// <param name="dateCommande">Date de la commande</param>
        /// <param name="montant">Montant de la commande</param>
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
