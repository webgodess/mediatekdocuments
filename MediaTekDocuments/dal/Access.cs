using System;
using System.Collections.Generic;
using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// ajout pour utiliser App.config
using System.Configuration;



namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        ///  méthode HTTP pour delete
        ///  </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// Constante de "champs"
        /// </summary>
        private const string CHAMPS = "champs=";
      
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                // pour recuperer les clés du App.config
                string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
                string login = ConfigurationManager.AppSettings["ApiLogin"];
                string pwd = ConfigurationManager.AppSettings["ApiPwd"];

                // on remplace dans l'authentification string
                authenticationString = $"{login}:{pwd}";

                api = ApiRest.GetInstance(apiUrl, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne tous les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne tous les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne tous les suivis à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivis;
        }


        /// <summary>
        /// Retourne toutes les commandes à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Commande</returns>
        public List<Commande> GetAllCommandes()
        {
            List<Commande> lesCommandes = TraitementRecup<Commande>(GET, "commande", null);
            return lesCommandes;
        }


        /// <summary>
        /// Retourne tous les abonnement à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAllAbonnements()
        {
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement", null);
            return lesAbonnements;
        }



        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }


        /// <summary>
        /// Retourne les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste d'objets Abonnements</returns>
        public List<Abonnement> GetAbonnements(string idRevue)
        {
            String jsonIdRevue = convertToJson("idRevue", idRevue);
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement/" + jsonIdRevue, null);
            return lesAbonnements;
        }

        /// <summary>
        /// Retourne toutes les commandes d'un livre ou DVD à partir de la BDD
        /// </summary>
        /// <param name="idLivreDvd">id du livre ou DVD concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandeDocument(string idLivreDvd)
        {
            String jsonIdLivre = convertToJson("idLivreDvd", idLivreDvd);
            List<CommandeDocument> lesCommandesLives = TraitementRecup<CommandeDocument>
                (GET, "commandedocument/" + jsonIdLivre, null);
            return lesCommandesLives;
        }



        /// <summary>
        /// Retourne un utilisateur de la BDD selon son login et son mot de passe 
        /// </summary>
        /// <param name="login">login de l'utilisateur' concerné</param>
        /// <param name="pwd">mot de passe de l'utilisateur</param>
        /// <returns>L'objet utilisateur sinon null</returns>
        public Utilisateur GetUtilisateur(string login, string pwd)
        {
            String jsonLogin = convertToJson("login", login);
            List<Utilisateur> lesUtilisateurs = TraitementRecup<Utilisateur>(GET, "utilisateur/" + jsonLogin, null);
            return lesUtilisateurs.Find(u => u.Login == login && u.Pwd == pwd);
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", CHAMPS + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ecriture d'une commande de document en base de données
        /// Insère dans la table commande ET dans la table commandedocument
        /// </summary>
        /// <param name="commandedoc">commande à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerCommande(CommandeDocument commandedoc)
        {
            try
            {
                // 1. Insérer dans commande
                var infoCommande = new Dictionary<string, Object>
        {
            {"id", commandedoc.Id},
            {"dateCommande", commandedoc.DateCommande},
            {"montant", commandedoc.Montant}
        };
                String jsonCommande = JsonConvert.SerializeObject(
                    infoCommande, new CustomDateTimeConverter());
                List<CommandeDocument> liste1 = TraitementRecup<CommandeDocument>
                    (POST, "commande", CHAMPS + jsonCommande);

                // 2. Insérer dans commandedocument
                var infoCommandeDoc = new Dictionary<string, Object>
        {
            {"id", commandedoc.Id},
            {"nbExemplaire", commandedoc.NbExemplaire},
            {"idLivreDvd", commandedoc.IdLivreDvd},
            {"idSuivi", commandedoc.IdSuivi}
        };
                String jsonCommandeDoc = JsonConvert.SerializeObject(infoCommandeDoc);
                List<CommandeDocument> liste2 = TraitementRecup<CommandeDocument>
                    (POST, "commandedocument", CHAMPS + jsonCommandeDoc);

                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'un livre en base de données
        /// </summary>
        /// <param name="livre">livre à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerLivre(Livre livre)
        {

            try
            {
                var infoDocument = new Dictionary<string, Object>
           {
             {"id",livre.Id },
            {"titre",livre.Titre },
            {"image",livre.Image },
            {"idGenre",livre.IdGenre },
            {"idPublic",livre.IdPublic },
            {"idRayon",livre.IdRayon },
           };

                // Insértion dans document d'abord
                // on utilise pas CustomDateTimeConverter() car un livre ne contient PAS de date !
                String jsonDocument = JsonConvert.SerializeObject(infoDocument);
                List<Livre> liste1 = TraitementRecup<Livre>(POST, "document", CHAMPS + jsonDocument);

                // Insértion dans livres_dvd, on ne transmet que l'id vu que livre_dvd n'a pas de propriétés supplémentaires
                String jsonLivreDvd = convertToJson("id", livre.Id);
                List<Livre> liste2 = TraitementRecup<Livre>(POST, "livres_dvd", CHAMPS + jsonLivreDvd);

                // Insértion dans livre

                var infoLivre = new Dictionary<string, Object> {
                    {"id",livre.Id},
                    {"isbn",livre.Isbn },
            {"auteur",livre.Auteur },
            {"collection",livre.Collection }

                };
                String jsonLivre = JsonConvert.SerializeObject(infoLivre);
                List<Livre> liste3 = TraitementRecup<Livre>(POST, "livre", CHAMPS + jsonLivre);


                return (liste1 != null && liste2 != null && liste3 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un livre en base de données
        /// </summary>
        /// <param name="livre">livre à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierLivre(Livre livre)
        {


            try
            {
                // Modification dans document
                var infoDocument = new Dictionary<string, Object>
           {

            {"titre",livre.Titre },
            {"image",livre.Image },
            {"idGenre",livre.IdGenre },
            {"idPublic",livre.IdPublic },
            {"idRayon",livre.IdRayon },
           };
                String jsonDocument = JsonConvert.SerializeObject(infoDocument);
                List<Livre> liste1 = TraitementRecup<Livre>(PUT, "document/" + livre.Id, CHAMPS + jsonDocument);
                // Modification dans livres_dvd
                // Modification dans livre
                var infoLivre = new Dictionary<string, Object> {
                    {"isbn",livre.Isbn },
            {"auteur",livre.Auteur },
            {"collection",livre.Collection }

                };
                String jsonLivre = JsonConvert.SerializeObject(infoLivre);
                List<Livre> liste2 = TraitementRecup<Livre>(PUT, "livre/" + livre.Id, CHAMPS + jsonLivre);


                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un livre en base de données
        /// </summary>
        /// <param name="livre">livre à supprimer</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerLivre(Livre livre)
        {

            try
            {
                // On verifie les exemplaires
                // GET exemplaire avec id = livre.Id
                // Si liste non vide → return false

                String jsonIdLivre = convertToJson("id", livre.Id);
                List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdLivre, null);


                if (lesExemplaires != null && lesExemplaires.Count > 0)
                {
                    return false;
                }

                // Vérifier commandes
                // GET commande avec id = livre.Id
                // Si liste non vide → return false

                List<Commande> lesCommandes = TraitementRecup<Commande>
                (GET, "commande/" + jsonIdLivre, null);

                if (lesCommandes != null && lesCommandes.Count > 0)
                {
                    return false;
                }

                // Suppression dans livre , on commence par les enfants puis les parents

                List<Livre> liste1 = TraitementRecup<Livre>
                    (DELETE, "livre/" + jsonIdLivre, null);

                // Suppression dans livres_dvd
                List<Livre> liste2 = TraitementRecup<Livre>
                   (DELETE, "livres_dvd/" + jsonIdLivre, null);

                // Suppression dans document
                // Suppression dans livres_dvd
                List<Livre> liste3 = TraitementRecup<Livre>
                   (DELETE, "document/" + jsonIdLivre, null);

                return (liste1 != null && liste2 != null && liste3 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'un dvd en base de données
        /// </summary>
        /// <param name="dvd">dvd à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerDvd(Dvd dvd)
        {

            try
            {
                var infoDocument = new Dictionary<string, Object>
       {
         {"id",dvd.Id },
        {"titre",dvd.Titre },
        {"image",dvd.Image },
        {"idGenre",dvd.IdGenre },
        {"idPublic",dvd.IdPublic },
        {"idRayon", dvd.IdRayon },
       };

                // Insértion dans document d'abord
                // on utilise pas CustomDateTimeConverter() car un dvd ne contient PAS de date !
                String jsonDocument = JsonConvert.SerializeObject(infoDocument);
                List<Dvd> liste1 = TraitementRecup<Dvd>(POST, "document", CHAMPS + jsonDocument);

                // Insértion dans livres_dvd, on ne transmet que l'id vu que livre_dvd n'a pas de propriétés supplémentaires
                String jsonLivreDvd = convertToJson("id", dvd.Id);
                List<Dvd> liste2 = TraitementRecup<Dvd>(POST, "livres_dvd", CHAMPS + jsonLivreDvd);

                // Insértion dans dvd

                var infoDvd = new Dictionary<string, Object> {
                {"id",dvd.Id},
                {"duree",dvd.Duree },
        {"realisateur",dvd.Realisateur },
        {"synopsis",dvd.Synopsis }

            };
                String jsonDvd = JsonConvert.SerializeObject(infoDvd);
                List<Dvd> liste3 = TraitementRecup<Dvd>(POST, "dvd", CHAMPS + jsonDvd);


                return (liste1 != null && liste2 != null && liste3 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un dvd en base de données
        /// </summary>
        /// <param name="dvd">dvd à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierDvd(Dvd dvd)
        {


            try
            {
                // Modification dans document
                var infoDocument = new Dictionary<string, Object>
    {

     {"titre",dvd.Titre },
     {"image",dvd.Image },
     {"idGenre",dvd.IdGenre },
     {"idPublic",dvd.IdPublic },
     {"idRayon",dvd.IdRayon },
    };
                String jsonDocument = JsonConvert.SerializeObject(infoDocument);
                List<Dvd> liste1 = TraitementRecup<Dvd>(PUT, "document/" + dvd.Id, CHAMPS + jsonDocument);
                // Modification dans livres_dvd
                //Modification dans dvd
                var infoDvd = new Dictionary<string, Object> {

        {"duree",dvd.Duree },
{"realisateur",dvd.Realisateur },
{"synopsis",dvd.Synopsis }

         };
                String jsonDvd = JsonConvert.SerializeObject(infoDvd);
                List<Dvd> liste2 = TraitementRecup<Dvd>(PUT, "dvd/" + dvd.Id, CHAMPS + jsonDvd);


                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un dvd en base de données
        /// </summary>
        /// <param name="dvd">dvd à supprimer</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerDvd(Dvd dvd)
        {

            try
            {
                // Vérifier exemplaires
                // GET exemplaire avec id = dvd.Id
                // Si liste non vide → return false

                String jsonIdDvd = convertToJson("id", dvd.Id);
                List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDvd, null);


                if (lesExemplaires != null && lesExemplaires.Count > 0)
                {
                    return false;
                }

                // Vérifier commandes
                // GET commande avec id = dvd.Id
                // Si liste non vide → return false

                List<JObject> lesCommandes = TraitementRecup<JObject>
                (GET, "commande/" + jsonIdDvd, null);

                if (lesCommandes != null && lesCommandes.Count > 0)
                {
                    return false;
                }

                // Suppression dans dvd

                List<Dvd> liste1 = TraitementRecup<Dvd>
                    (DELETE, "dvd/" + jsonIdDvd, null);

                // Suppression dans livres_dvd
                List<Dvd> liste2 = TraitementRecup<Dvd>
                   (DELETE, "livres_dvd/" + jsonIdDvd, null);

                // Suppression dans document
                // Suppression dans livres_dvd
                List<Dvd> liste3 = TraitementRecup<Dvd>
                   (DELETE, "document/" + jsonIdDvd, null);

                return (liste1 != null && liste2 != null && liste3 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'une revue en base de données
        /// </summary>
        /// <param name="revue">revue à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerRevue(Revue revue)
        {
            try
            {
                var infoDocument = new Dictionary<string, Object>
        {
            {"id",    revue.Id },
            {"titre", revue.Titre },
            {"image", revue.Image },
            {"idGenre",  revue.IdGenre },
            {"idPublic", revue.IdPublic },
            {"idRayon",  revue.IdRayon }
        };
                String jsonDocument = JsonConvert.SerializeObject(infoDocument);
                List<Revue> liste1 = TraitementRecup<Revue>(POST, "document", CHAMPS + jsonDocument);


                var infoRevue = new Dictionary<string, Object>
        {
            {"id",              revue.Id },
            {"periodicite",     revue.Periodicite },
            {"delaiMiseADispo", revue.DelaiMiseADispo }
        };
                String jsonRevue = JsonConvert.SerializeObject(infoRevue);
                List<Revue> liste2 = TraitementRecup<Revue>(POST, "revue", CHAMPS + jsonRevue);

                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'une revue en base de données
        /// </summary>
        /// <param name="revue">revue à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierRevue(Revue revue)
        {
            try
            {
                var infoDocument = new Dictionary<string, Object>
        {
            {"titre",    revue.Titre },
            {"image",    revue.Image },
            {"idGenre",  revue.IdGenre },
            {"idPublic", revue.IdPublic },
            {"idRayon",  revue.IdRayon }
        };
                String jsonDocument = JsonConvert.SerializeObject(infoDocument);
                List<Revue> liste1 = TraitementRecup<Revue>(PUT, "document/" + revue.Id, CHAMPS + jsonDocument);

                var infoRevue = new Dictionary<string, Object>
        {
            {"periodicite",     revue.Periodicite },
            {"delaiMiseADispo", revue.DelaiMiseADispo }
        };
                String jsonRevue = JsonConvert.SerializeObject(infoRevue); 
                List<Revue> liste2 = TraitementRecup<Revue>(PUT, "revue/" + revue.Id, CHAMPS + jsonRevue); 

                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une revue en base de données
        /// </summary>
        /// <param name="revue">revue à supprimer</param>
        /// <returns>true si la suppression a pu se faire</returns>

        public bool SupprimerRevue(Revue revue)
        {
            try
            {
                String jsonIdRevue = convertToJson("id", revue.Id);

                // Vérifier exemplaires
                List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>
                    (GET, "exemplaire/" + jsonIdRevue, null);
                if (lesExemplaires != null && lesExemplaires.Count > 0)
                    return false;

                // Vérifier commandes
                List<JObject> lesCommandes = TraitementRecup<JObject>
                    (GET, "commande/" + jsonIdRevue, null);
                if (lesCommandes != null && lesCommandes.Count > 0)
                    return false;

                // Suppression dans revue
                List<Revue> liste1 = TraitementRecup<Revue>
                    (DELETE, "revue/" + jsonIdRevue, null);

                
                // Suppression dans document
                List<Revue> liste2 = TraitementRecup<Revue>
                    (DELETE, "document/" + jsonIdRevue, null);

                return (liste1 != null && liste2 != null); // ✅ 2 listes seulement
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification de l'étape de suivi d'une commande en base de données
        /// </summary>
        /// <param name="cmd">commande dont le suivi est à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierSuiviCommande(CommandeDocument cmd)
        {
            try
            {
                // On modifie SEULEMENT idSuivi
                // dans commandedocument
                var infoCommande = new Dictionary<string, Object>
        {
            {"idSuivi", cmd.IdSuivi}
        };

                String jsonCommande = JsonConvert.SerializeObject(infoCommande);

                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>
                    (PUT, "commandedocument/" + cmd.Id, CHAMPS + jsonCommande);

                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Supprime dans commandedocument
        /// </summary>
        public bool SupprimerCommandeDocument(string id)
        {
            try
            {
                String jsonId = convertToJson("id", id);
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>
                    (DELETE, "commandedocument/" + jsonId, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Supprime dans commande
        /// </summary>
        public bool SupprimerCommande(string id)
        {
            try
            {
                String jsonId = convertToJson("id", id);
                List<Commande> liste = TraitementRecup<Commande>
                    (DELETE, "commande/" + jsonId, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ecriture d'un abonnement en base de données
        /// </summary>
        /// <param name="abonnement">abonnement à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {

            try
            {
                var infoCommande = new Dictionary<string, Object>
        {
            {"id",abonnement.Id},
            {"dateCommande",abonnement.DateCommande},
            {"montant",abonnement.Montant}

        };

                // est necessaire ici car il convertis les dates : CustomDateTimeConverter()

                String jsonCommande = JsonConvert.SerializeObject(infoCommande, new CustomDateTimeConverter());
                // inserer d'abord dans la classe mere Commande
                List<Abonnement> liste1 = TraitementRecup<Abonnement>(POST, "commande", CHAMPS + jsonCommande);

                var infoAbonnement = new Dictionary<string, Object>
    {
        {"id",abonnement.Id},
       {"dateFinAbonnement",abonnement.DateFinAbonnement },
       {"idRevue",abonnement.IdRevue}
    };

                String jsonAbonnement = JsonConvert.SerializeObject(infoAbonnement, new CustomDateTimeConverter());
                // inserer dans l'abonnement 
                List<Abonnement> liste2 = TraitementRecup<Abonnement>(POST, "abonnement", CHAMPS + jsonAbonnement);
                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Supprime dans Abonnement
        /// </summary>
        public bool SupprimerAbonnement(string id)
        {

            try
            {
                String jsonId = convertToJson("id", id);


                // Suppression dans abonnement, on commence par les enfants puis les parents

                List<Abonnement> liste1 = TraitementRecup<Abonnement>
                    (DELETE, "abonnement/" + jsonId, null);

                // Suppression dans commande
                List<Commande> liste2 = TraitementRecup<Commande>
                   (DELETE, "commande/" + jsonId, null);


                return (liste1 != null && liste2 != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }



        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
