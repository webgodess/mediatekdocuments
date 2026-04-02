using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{

#pragma warning disable IDE1006 // Naming Styles
    public partial class loginPage : Form
#pragma warning restore IDE1006 // Naming Styles
    {
        
        private readonly FrmMediatekController controller;


        /// <summary>
        /// Constructeur du formulaire
        /// </summary>
        public loginPage()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
            this.Load += loginPage_Load;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void loginPage_Load(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            videChamps();
            
        }

        /// <summary>
        /// Pour vider les champs du formulaire
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        private void videChamps()
#pragma warning restore IDE1006 // Naming Styles
        {
            // on vide le formulaire
            txtLogin.Text = string.Empty;
            txtPwd.Text = string.Empty;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void btnSubmit_Click(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            string login = txtLogin.Text;
            string pwd = txtPwd.Text;

            if (login == "" || pwd == "")
            {
                MessageBox.Show("Veuillez saisir un login et un mot de passe.","Erreur");
                return;
            }

            Utilisateur utilisateur = controller.GetUtilisateur(login, pwd);


            if (utilisateur == null) {
                MessageBox.Show("Login ou mot de passe incorrect.", "Erreur");
                return;
            }


            if(utilisateur.IdService == "3")
            {
                MessageBox.Show("Vous n'avez pas accès aux fonctionnalités de cette application", "Erreur");
                Application.Exit();
                return;

            }

            FrmMediatek frmMediatek = new FrmMediatek(utilisateur);
            frmMediatek.Show();
            this.Hide();
        }
    }
}
