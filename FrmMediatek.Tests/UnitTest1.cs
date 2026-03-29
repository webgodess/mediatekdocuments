namespace FrmMediatek.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ParutionDansAbonnementTestTrue()
        {
            DateTime dateCommande = new DateTime(2026, 03, 26);
            DateTime dateFinAbonnement = new DateTime(2026, 04, 22);
            DateTime dateParution = new DateTime(2026, 04, 16);

            bool result = dateParution >= dateCommande && dateParution <= dateFinAbonnement;
            Assert.True(result);

        }

        [Fact]
        public void ParutionHorsAbonnementTestFalse()
        {
            DateTime dateCommande = new DateTime(2026, 03, 26);
            DateTime dateFinAbonnement = new DateTime(2026, 04, 22);
            DateTime dateParution = new DateTime(2026, 05, 16); 

            bool result = dateParution >= dateCommande && dateParution <= dateFinAbonnement;
            Assert.False(result);
        }
    }
}