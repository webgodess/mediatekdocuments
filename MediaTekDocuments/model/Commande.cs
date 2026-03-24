
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
        /// Classe métier Commande (réunit les infomations communes à toutes les Commandes)
        /// </summary>
    public class Commande
    {
        public string Id { get; }
        public DateTime DateCommande { get; }
        public double Montant { get; }
        public Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}