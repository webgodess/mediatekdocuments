
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Login de l'utilisateur
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string Pwd { get; }

        /// <summary>
        /// Identifiant du service de l'utilisateur
        /// </summary>
        public string IdService { get; }

        /// <summary>
        /// Constructeur de l'utilisateur
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur</param>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="pwd">Mot de passe de l'utilisateur</param>
        /// <param name="idService">Identifiant du service</param>
        public Utilisateur(string id, string login, string pwd, string idService)
        {
            Id = id;
            Login = login;
            Pwd = pwd;
            IdService = idService;
        }
    }
}