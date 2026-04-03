using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class PublicTests
    {
        [TestMethod()]
        public void PublicToStringTestValid()
        {
            var publicTest = new Public("20001","Ados");

        string result = "Ados";

        Assert.AreEqual(result, publicTest.ToString());
        }


}

}

   
   