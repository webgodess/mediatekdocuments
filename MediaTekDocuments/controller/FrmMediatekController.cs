using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleurs gérant la logique métier de l'application MediaTekDocuments
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    /// Contrôleur de l'application MediaTekDocuments.
    /// Gère les interactions entre la vue (FrmMediatek) et la couche d'accès aux données.
    /// Permet la gestion des livres, DVD, revues, commandes, abonnements et exemplaires.
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Crée un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet Livre concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerLivre(Livre livre)
        {
            return access.CreerLivre(livre);
        }

        /// <summary>
        /// Modifie un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet Livre concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierLivre(livre);
        }

        /// <summary>
        /// Supprime un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet Livre concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerLivre(Livre livre)
        {
            return access.SupprimerLivre(livre);
        }

        /// <summary>
        /// Crée un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet Dvd concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerDvd(Dvd dvd)
        {
            return access.CreerDvd(dvd);
        }

        /// <summary>
        /// Modifie un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet Dvd concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDvd(dvd);
        }

        /// <summary>
        /// Supprime un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet Dvd concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerDvd(Dvd dvd)
        {
            return access.SupprimerDvd(dvd);
        }

        /// <summary>
        /// Crée une revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet Revue concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerRevue(Revue revue)
        {
            return access.CreerRevue(revue);
        }

        /// <summary>
        /// Modifie une revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet Revue concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierRevue(revue);
        }

        /// <summary>
        /// Supprime une revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet Revue concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerRevue(Revue revue)
        {
            return access.SupprimerRevue(revue);
        }

        /// <summary>
        /// Retourne les commandes d'un livre ou DVD
        /// </summary>
        /// <param name="idLivreDvd">id du livre ou DVD</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandeDocument(string idLivreDvd)
        {
            return access.GetCommandeDocument(idLivreDvd);
        }

        /// <summary>
        /// Retourne l'utilisateur correspondant dans la BDD
        /// </summary>
        /// <param name="login">Login de l'utilisateur concerné</param>
        /// <param name="pwd">Mot de passe de l'utilisateur</param>
        /// <returns>L'utilisateur trouvé, sinon null</returns>
        public Utilisateur GetUtilisateur(string login, string pwd)
        {
            return access.GetUtilisateur(login, pwd);

        }



        /// <summary>
        /// Retourne tous les suivis
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Retourne tous les abonnements
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAbonnements(string idRevues)
        {
            return access.GetAbonnements(idRevues);
        }



        /// <summary>
        /// Retourne toutes les commandes
        /// </summary>
        public List<Commande> GetAllCommandes()
        {
            return access.GetAllCommandes();
        }

        /// <summary>
        /// Retourne tous les abonnements
        /// </summary>
        public List<Abonnement> GetAllAbonnements()
        {
            return access.GetAllAbonnements();
        }

        /// <summary>
        /// Crée une commande dans la bdd
        /// </summary>
        /// <param name="commandedoc">commande à créer</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(CommandeDocument commandedoc)
        {
            return access.CreerCommande(commandedoc);
        }

        /// <summary>
        /// Crée un abonnement dans la bdd
        /// </summary>
        /// <param name="abonnementRevue">abonnement à créer</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerAbonnement(Abonnement abonnementRevue)
        {
            return access.CreerAbonnement(abonnementRevue);
        }

        /// <summary>
        /// Modifie le suivi d'une commande dans la bdd
        /// </summary>
        /// <param name="cmd">commande à modifier</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierSuiviCommande(CommandeDocument cmd)
        {
            return access.ModifierSuiviCommande(cmd);
        }

        /// <summary>
        /// Supprime une commande document dans la bdd (table fille)
        /// </summary>
        /// <param name="id">id de la commande à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(string id)
        {
            return access.SupprimerCommandeDocument(id);
        }

        /// <summary>
        /// Supprime une commande dans la bdd 
        /// </summary>
        /// <param name="id">id de la commande à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommande(string id)
        {
            return access.SupprimerCommande(id);
        }

        /// <summary>
        /// Supprime un abonnement dans la bdd 
        /// </summary>
        /// <param name="id">id de l'abonnement à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerAbonnement(string id)
        {
            return access.SupprimerAbonnement(id);
        }
    }
}
