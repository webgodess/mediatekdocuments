using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests
{
    [TestClass()]
    public class ParutionDansAboTests
    {

        [TestMethod()]

        public void ParutionDansAbonnementTestTrue()
        {
            var dateCommande = new DateTime(2026, 03, 26, 0, 0, 0, DateTimeKind.Local);
            var dateFinAbonnement = new DateTime(2026, 04, 22, 0, 0, 0, DateTimeKind.Local);
            var dateParution = new DateTime(2026, 04, 16, 0, 0, 0, DateTimeKind.Local);

            bool result = dateParution >= dateCommande && dateParution <= dateFinAbonnement;
            Assert.IsTrue(result);
        }

        [TestMethod()]

        public void ParutionHorsAbonnementTestFalse()
        {
            var dateCommande = new DateTime(2026, 03, 26, 0, 0, 0, DateTimeKind.Local);
            var dateFinAbonnement = new DateTime(2026, 04, 22, 0, 0, 0, DateTimeKind.Local);
            var dateParution = new DateTime(2026, 05, 16, 0, 0, 0, DateTimeKind.Local);

            bool result = dateParution >= dateCommande && dateParution <= dateFinAbonnement;
            Assert.IsFalse(result);
        }

    }
    }
