
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement représentant un abonnement à une revue
    /// </summary>
    public class Abonnement
    {
        public string Id { get; }
        public DateTime DateFinAbonnement { get; }
        public string IdRevue { get; }
        public DateTime DateCommande { get; }
        public double Montant { get; }

        public Abonnement(string id, DateTime dateFinAbonnement, string idRevue, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}

