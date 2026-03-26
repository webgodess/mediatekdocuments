
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement représentant un abonnement à une revue
    /// Abonnement hérite de Commande : contient des propriétés spécifiques aux commandes
    /// </summary>
    public class Abonnement: Commande
    {
        public DateTime DateFinAbonnement { get; set; }
        public string IdRevue { get; set; }

        public Abonnement(string id, DateTime dateFinAbonnement, string idRevue, DateTime dateCommande, double montant): base(id,dateCommande, montant)
        {
            DateFinAbonnement = dateFinAbonnement;
            IdRevue = idRevue;
          
          
        }
    }
}

