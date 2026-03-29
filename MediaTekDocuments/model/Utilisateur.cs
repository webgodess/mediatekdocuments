
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur
    /// </summary>
    public class Utilisateur
    {
        public string Id { get; }
        public string Login { get;  }
        public string Pwd { get; }
        public string IdService { get;  }
       

        public Utilisateur(string id, string login, string pwd, string idService)
        {
            Id = id;
            Login = login;
            Pwd = pwd;
            IdService = idService;
            
        }
    }
}
