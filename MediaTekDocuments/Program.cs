using MediaTekDocuments.view;
using System;
using System.Windows.Forms;



namespace MediaTekDocuments
{
    /// <summary>
    /// Application Gestion de MediaTekDocuments
    /// </summary>
    internal class NamespaceDoc
    {
    }

    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // on change de Application.Run(new FrmMediatek()) à Application.Run(new loginPage())
            Application.Run(new loginPage());
        }
    }
}
