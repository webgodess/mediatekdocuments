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
    public class GenreTests
    {
        [TestMethod()]

            public void GenreToStringTestValid() {


                var genre = new Genre("10034", "Horreur");

                string result = "Horreur";

                Assert.AreEqual(result, genre.ToString());
            }
         
    }



}
