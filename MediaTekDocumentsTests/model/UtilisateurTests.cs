using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class UtilisateurTests
    {
        [TestMethod()]
        public void UtilisateurConstructeurTest()
        {
            string id = "37";
            string login = "Jerome";
            string pwd = "Cookies123";
            string idService = "20200";
            var utilisateur = new Utilisateur(id,login,pwd,idService);

            Assert.AreEqual(id, utilisateur.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(login, utilisateur.Login, "devrait réussir : login valorisé");
            Assert.AreEqual(pwd, utilisateur.Pwd, "devrait réussir : pwd valorisé");
            Assert.AreEqual(idService, utilisateur.IdService, "devrait réussir : idService valorisé");

        }
    }
}