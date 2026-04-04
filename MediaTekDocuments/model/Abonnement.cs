
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement représentant un abonnement à une revue
    /// Abonnement hérite de Commande : contient des propriétés spécifiques aux commandes
    /// </summary>
    public class Abonnement: Commande
    {
        /// <summary>
        /// Date de fin de l'abonnement
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }
        /// <summary>
        /// Id de la revue associée à l'abonnement
        /// </summary>
        public string IdRevue { get; set; }

        /// <summary>
        /// Constructeur de l'abonnement
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement</param>
        /// <param name="dateFinAbonnement">Date de fin de l'abonnement</param>
        /// <param name="idRevue">Identifiant de la revue</param>
        /// <param name="dateCommande">Date de la commande</param>
        /// <param name="montant">Montant de l'abonnement</param>
        public Abonnement(string id, DateTime dateFinAbonnement, string idRevue, DateTime dateCommande, double montant): base(id,dateCommande, montant)
        {
            DateFinAbonnement = dateFinAbonnement;
            IdRevue = idRevue;
          
          
        }
    }
}

