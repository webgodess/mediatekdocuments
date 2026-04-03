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
    public class RayonTests
    {
        [TestMethod()]
        public void RayonToStringTestValid()
        {
            var rayon = new Rayon("10101","Sciences");

            string result = "Sciences";

            Assert.AreEqual(result, rayon.ToString());
        }
    }
}