namespace FrmMediatek.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ParutionDansAbonnementTestTrue()
        {
    

            var dateCommande = new DateTime(2026, 03, 26, 0, 0, 0, DateTimeKind.Local);
            var dateFinAbonnement = new DateTime(2026, 04, 22, 0, 0, 0, DateTimeKind.Local);
            var dateParution = new DateTime(2026, 04, 16, 0, 0, 0, DateTimeKind.Local);

            bool result = dateParution >= dateCommande && dateParution <= dateFinAbonnement;
            Assert.True(result);

        }

        [Fact]
        public void ParutionHorsAbonnementTestFalse()
        {
            var dateCommande = new DateTime(2026, 03, 26, 0, 0, 0, DateTimeKind.Local);
            var dateFinAbonnement = new DateTime(2026, 04, 22, 0, 0, 0, DateTimeKind.Local);
            var dateParution = new DateTime(2026, 05, 16, 0, 0, 0, DateTimeKind.Local);

            bool result = dateParution >= dateCommande && dateParution <= dateFinAbonnement;
            Assert.False(result);
        }
    }
}