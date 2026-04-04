
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande (réunit les infomations communes à toutes les Commandes)
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Id de la commande
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Date de la commande
        /// </summary>
        public DateTime DateCommande { get; }

        /// <summary>
        /// Montant de la commande
        /// </summary>
        public double Montant { get; }

        /// <summary>
        /// Constructeur de la commande
        /// </summary>
        /// <param name="id">Id de la commande</param>
        /// <param name="dateCommande">Date de la commande</param>
        /// <param name="montant">Montant de la commande</param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}
