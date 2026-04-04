
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg.Sig;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire d'alerte affichant les abonnements arrivant à expiration
    /// </summary>
    public partial class FormulaireAlerte : Form
    {
        private readonly List<Abonnement> abonnementsExpirants;
        private readonly List<Revue> lesRevues;


        /// <summary>
        /// Constructeur du formulaire
        /// </summary>
        /// <param name="abonnementsExpirants">Liste des abonnements arrivant à expiration</param>
        /// <param name="lesRevues">Liste de toutes les revues</param>
        public FormulaireAlerte(List<Abonnement> abonnementsExpirants, List<Revue> lesRevues)
        {
            InitializeComponent();
            this.abonnementsExpirants = abonnementsExpirants;
            this.lesRevues = lesRevues;
            this.Load += FormulaireAlerte_Load;


        }
        private void FormulaireAlerte_Load(object sender, EventArgs e)
        {
            // vider la liste

            listBxAboEx.Items.Clear();


            foreach (Abonnement aboex in abonnementsExpirants)
            {
                // Pour trouver la revue selon l'Id
                Revue revue = lesRevues.Find(r => r.Id == aboex.IdRevue);

                if (revue != null)
                {


                    listBxAboEx.Items.Add($"{revue.Titre} - {aboex.DateFinAbonnement:dd/MM/yyyy}");
                }

            }
        }





    }




}
