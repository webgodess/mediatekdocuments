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
using Org.BouncyCastle.Bcpg.Sig;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        // Ajouter les 3 BindingSources pour Livres :
        private readonly BindingSource bdgLivresGenresEditAdd = new BindingSource();
        private readonly BindingSource bdgLivresPublicsEditAdd = new BindingSource();
        private readonly BindingSource bdgLivresRayonsEditAdd = new BindingSource();

        // Ajouter les 3 BindingSources pour Dvd :
        private readonly BindingSource bdgDvdGenresEditAdd = new BindingSource();
        private readonly BindingSource bdgDvdPublicsEditAdd = new BindingSource();
        private readonly BindingSource bdgDvdRayonsEditAdd = new BindingSource();

        // Ajouter les 3 BindingSources pour Revues :
        private readonly BindingSource bdgRevuesGenresEditAdd = new BindingSource();
        private readonly BindingSource bdgRevuesPublicsEditAdd = new BindingSource();
        private readonly BindingSource bdgRevuesRayonsEditAdd = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        ///  Dès l'ouverture de l'application, ouvrir une petite 
        ///  fenêtre d'alerte rappelant la liste des revues expirant dans moins d'un mois
        /// </summary>
        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            // on apelle notre methode RecupererAbonnementsExpirants() 
            // pour recuperer la liste des abonnements expirants

            List<Abonnement> abonnementsExpirants = RecupererAbonnementsExpirants();
            List<Revue> revues = controller.GetAllRevues();
            // verifier que la liste n'est pas vide
            if (abonnementsExpirants.Count >0)
            {
                FormulaireAlerte alerte = new FormulaireAlerte(abonnementsExpirants, revues);
                alerte.ShowDialog();
                
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// _Enter est un événement qui se déclenche quand l'utilisateur clique sur un onglet
        /// et que cet onglet devient actif.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

            // rempli les combobox pour l'ajout ou modification d'un livre
            RemplirComboCategorie(controller.GetAllGenres(), bdgLivresGenresEditAdd, cbxLivresGenresEditAdd);
            RemplirComboCategorie(controller.GetAllPublics(), bdgLivresPublicsEditAdd, cbxLivresPublicsEditAdd);
            RemplirComboCategorie(controller.GetAllRayons(), bdgLivresRayonsEditAdd, cbxLivresRayonsEditAdd);

            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Met les champs en mode édition ou lecture seule
        /// </summary>
        /// <param name="modeEdition">true = édition, false = lecture</param>
        private void ModeEditionLivres(bool modeEdition)
        {
            txbLivresTitre.ReadOnly = !modeEdition;
            txbLivresAuteur.ReadOnly = !modeEdition;
            txbLivresCollection.ReadOnly = !modeEdition;
            txbLivresIsbn.ReadOnly = !modeEdition;
            txbLivresImage.ReadOnly = !modeEdition;
            btnValiderLivre.Visible = modeEdition;
            btnAnnulerLivre.Visible = modeEdition;
            btnAjoutLivre.Visible = !modeEdition;
            btnModifierLivre.Visible = !modeEdition;
            btnSupprimerLivre.Visible = !modeEdition;
            cbxLivresGenres.Enabled = !modeEdition;
            cbxLivresPublics.Enabled = !modeEdition;
            cbxLivresRayons.Enabled = !modeEdition;
            txbLivresTitreRecherche.ReadOnly = !modeEdition;
            txbLivresGenre.Visible = !modeEdition;
            txbLivresPublic.Visible = !modeEdition;
            txbLivresRayon.Visible = !modeEdition;
            cbxLivresGenresEditAdd.Visible = modeEdition;
            cbxLivresPublicsEditAdd.Visible = modeEdition;
            cbxLivresRayonsEditAdd.Visible = modeEdition;

            txbLivresNumRecherche.Enabled = !modeEdition;
            txbLivresTitreRecherche.Enabled = !modeEdition;
            btnLivresNumRecherche.Enabled = !modeEdition;
            btnLivresAnnulGenres.Enabled = !modeEdition;
            btnLivresAnnulRayons.Enabled = !modeEdition;
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        private void cbxLivresGenresEditAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            // to be filled?
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Passe en mode édition pour l'ajout d'un nouveau livre.
        /// Vide les champs .
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnAjoutLivre
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>

        private void btnAjoutLivre_Click(object sender, EventArgs e)
        {   // Vide les champs
            VideLivresInfos();
            // Passe en mode édition
            ModeEditionLivres(true);
            // On peut mettre un id 
            txbLivresNumero.ReadOnly = false;
            txbLivresNumero.Focus();


        }

        /// <summary>
        /// Modifie un livre.
        /// On passe en mode edition
        /// L'id du livre reste en lecture seule.
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnModifierLivre
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>

        private void btnModifierLivre_Click(object sender, EventArgs e)
        {   // on passe en mode édition
            ModeEditionLivres(true);
            //on ne peut changer l'id
            txbLivresNumero.ReadOnly = true;
        }


        /// <summary>
        /// Supprime le livre sélectionné après confirmation.
        /// Impossible si le livre possède des exemplaires ou commandes.
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnSupprimerLivre
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>

        private void btnSupprimerLivre_Click(object sender, EventArgs e)
        {
            // bdgLivresListe.List : la liste de tous les livres affichés dans le datagrid
            // bdgLivresListe.Position : l'index de la ligne sélectionnée dans le datagrid
            // (Livre) : cast: on dit "cet objet est un Livre"
            // Si on cliques sur la ligne 3 du datagrid Position = 3
            // List[3] = le livre à la ligne 3
            // livre = ce Livre
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            if (livre != null)
            {

                // demande de confirmation
                //result = DialogResult.Yes  si l'utilisateur clique Oui
                //result = DialogResult.No   si l'utilisateur clique Non

                DialogResult result = MessageBox.Show(
                $"Voulez-vous supprimer {livre.Titre} ?",
                "Confirmation",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // on fait appel au controller pour supprimer
                    // controller.SupprimerLivre(livre) envoie la demande de suppression
                    // à la base de données.
                    // retourne true  si la suppression a réussi
                    // retourne false si impossible (exemplaires ou commandes liés)
                    if (controller.SupprimerLivre(livre))
                    {
                        // retourne tous les livres à partir de la BDD
                        lesLivres = controller.GetAllLivres();
                        // Affiche la liste complète des livres
                        // et annule toutes les recherches et filtres
                        RemplirLivresListeComplete();
                        MessageBox.Show($"{livre.Titre} supprimé avec succès !");


                    }
                    else
                    {
                        MessageBox.Show($"Impossible de supprimer {livre.Titre}, il possède des exemplaires ou commandes.");
                    }
                }

            }
        }

        /// <summary>
        /// Valide les Ajouts ou les modifications saisies
        /// </summary>
        /// <param name="sender"></param>
        /// sender = l'objet qui a déclenché l'événement
        ///  → ici c'est le bouton btnValiderLivre
        /// e = les informations sur l'événement
        ///     → ici c'est un simple clic
        /// <param name="e"></param>
        private void btnValiderLivre_Click(object sender, EventArgs e)
        {
            // (Genre) est un cast ici
            // sans ce cast genre est juste un "object"
            // On ne peut pas accéder à
            // genre.Id ou genre.Libelle 

            Genre genre = (Genre)cbxLivresGenresEditAdd.SelectedItem;
            Public lePublic = (Public)cbxLivresPublicsEditAdd.SelectedItem;
            Rayon rayon = (Rayon)cbxLivresRayonsEditAdd.SelectedItem;

            if (txbLivresNumero.Text.Equals("") ||
        txbLivresTitre.Text.Equals(""))
            {
                MessageBox.Show("Numéro et titre obligatoires !", "Erreur");
                return;
            }

            if (genre == null || lePublic == null || rayon == null)
            {
                MessageBox.Show("Genre, Public et Rayon obligatoires !",
                                "Erreur");
                return;
            }

            if (!txbLivresNumero.Text.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro ne doit contenir que des chiffres !", "Erreur");
                return;
            }

            Livre livre = new Livre(
txbLivresNumero.Text,
txbLivresTitre.Text,
txbLivresImage.Text,
txbLivresIsbn.Text,
txbLivresAuteur.Text,
    txbLivresCollection.Text,
    genre.Id,
    genre.Libelle,
    lePublic.Id,
    lePublic.Libelle,
    rayon.Id,
    rayon.Libelle

    );
            // Si txbLivresNumero.ReadOnly = true
            // On est en mode Modification
            // Il faut appeler ModifierLivre()

            if (txbLivresNumero.ReadOnly)
            {
                // si la modification a reussi
                // ModifierLivre(livre) retourne un bool
                // if (true)  → succès → afficher liste
                // if (false) → échec  → afficher erreur


                if (controller.ModifierLivre(livre))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    ModeEditionLivres(false);
                    MessageBox.Show("Livre modifié avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification !",
                                    "Erreur");
                }
            }
            else
            {
                // c'est le bloc Ajout
                Livre livreExistant = lesLivres.Find(x => x.Id.Equals(txbLivresNumero.Text));

                if (livreExistant != null)
                {
                    MessageBox.Show(
                        $"Le numéro {txbLivresNumero.Text} existe déjà !",
                        "Erreur");
                    return;
                }

                if (controller.CreerLivre(livre))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    ModeEditionLivres(false);
                    MessageBox.Show("Livre ajouté avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout !", "Erreur");
                }
            }

        }

        /// <summary>
        /// Annule les Ajouts ou les modifications saisies
        /// </summary>
        /// <param name="sender"></param>
        /// sender = l'objet qui a déclenché l'événement
        ///  → ici c'est le bouton btnAnnulerLivre
        /// e = les informations sur l'événement
        ///     → ici c'est un simple clic
        /// <param name="e"></param>

        private void btnAnnulerLivre_Click(object sender, EventArgs e)
        {
            ModeEditionLivres(false);
            RemplirLivresListeComplete();
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour 
        /// le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);

            // rempli les combobox pour l'ajout ou modification d'un dvd
            RemplirComboCategorie(controller.GetAllGenres(), bdgDvdGenresEditAdd, cbxDvdGenresEditAdd);
            RemplirComboCategorie(controller.GetAllPublics(), bdgDvdPublicsEditAdd, cbxDvdPublicsEditAdd);
            RemplirComboCategorie(controller.GetAllRayons(), bdgDvdRayonsEditAdd, cbxDvdRayonsEditAdd);

            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Met les champs en mode édition ou lecture seule
        /// </summary>
        /// <param name="modeEdition">true = édition, false = lecture</param>
        private void ModeEditionDvd(bool modeEdition)
        {
            txbDvdTitre.ReadOnly = !modeEdition;
            txbDvdRealisateur.ReadOnly = !modeEdition;
            txbDvdDuree.ReadOnly = !modeEdition;
            txbDvdSynopsis.ReadOnly = !modeEdition;
            txbDvdImage.ReadOnly = !modeEdition;
            btnValiderDvd.Visible = modeEdition;
            btnAnnulerDvd.Visible = modeEdition;
            btnAjoutDvd.Visible = !modeEdition;
            btnModifierDvd.Visible = !modeEdition;
            btnSupprimerDvd.Visible = !modeEdition;
            cbxDvdGenres.Enabled = !modeEdition;
            cbxDvdPublics.Enabled = !modeEdition;
            cbxDvdRayons.Enabled = !modeEdition;

            txbDvdGenre.Visible = !modeEdition;
            txbDvdPublic.Visible = !modeEdition;
            txbDvdRayon.Visible = !modeEdition;
            cbxDvdGenresEditAdd.Visible = modeEdition;
            cbxDvdPublicsEditAdd.Visible = modeEdition;
            cbxDvdRayonsEditAdd.Visible = modeEdition;

            txbDvdNumRecherche.Enabled = !modeEdition;
            txbDvdTitreRecherche.Enabled = !modeEdition;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Passe en mode édition pour l'ajout d'un nouveau dvd.
        /// Vide les champs .
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnAjoutDvd
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>
        private void btnAjoutDvd_Click(object sender, EventArgs e)
        {
            // Vide les champs
            VideDvdInfos();
            // Passe en mode édition
            ModeEditionDvd(true);
            // On peut mettre un id 
            txbDvdNumero.ReadOnly = false;
            txbDvdNumero.Focus();
        }

        /// <summary>
        /// Supprime le dvd sélectionné après confirmation.
        /// Impossible si le dvd possède des exemplaires ou commandes.
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnSupprimerDvd
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>

        private void btnSupprimerDvd_Click(object sender, EventArgs e)
        {
            // bdgDvdListe.List : la liste de tous les dvd affichés dans le datagrid
            // bdgDvdListe.Position : l'index de la ligne sélectionnée dans le datagrid
            // (Dvd) : cast: on dit "cet objet est un Dvd"
            // Si on cliques sur la ligne 3 du datagrid Position = 3
            // List[3] = le dvd à la ligne 3
            // dvd = ce Dvd
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            if (dvd != null)
            {

                // demande de confirmation
                //result = DialogResult.Yes  si l'utilisateur clique Oui
                //result = DialogResult.No   si l'utilisateur clique Non

                DialogResult result = MessageBox.Show(
                $"Voulez-vous supprimer {dvd.Titre} ?",
                "Confirmation",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // fais appel au controller pour supprimer
                    // controller.SupprimerDvd(dvd) envoie la demande de suppression
                    // à la base de données.
                    // retourne true  si la suppression a réussi
                    // retourne false si impossible (exemplaires ou commandes liés)
                    if (controller.SupprimerDvd(dvd))
                    {
                        // retourne tous les dvd à partir de la BDD
                        lesDvd = controller.GetAllDvd();
                        // Affiche la liste complète des dvd
                        // et annule toutes les recherches et filtres
                        RemplirDvdListeComplete();
                        MessageBox.Show($"{dvd.Titre} supprimé avec succès !");


                    }
                    else
                    {
                        MessageBox.Show($"Impossible de supprimer {dvd.Titre}, il possède des exemplaires ou commandes.");
                    }
                }

            }
        }


        /// <summary>
        /// Modifie un dvd.
        /// On passe en mode edition
        /// L'id du dvd reste en lecture seule.
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnModifierDvd
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>

        private void btnModifierDvd_Click(object sender, EventArgs e)
        {   // on passe en mode édition
            ModeEditionDvd(true);
            //on ne peut changer l'id
            txbDvdNumero.ReadOnly = true;
        }

        /// <summary>
        /// Valide les Ajouts ou les modifications saisies
        /// </summary>
        /// <param name="sender"></param>
        /// sender = l'objet qui a déclenché l'événement
        ///  → ici c'est le bouton btnValiderDvd
        /// e = les informations sur l'événement
        ///     → ici c'est un simple clic
        /// <param name="e"></param>

        private void btnValiderDvd_Click(object sender, EventArgs e)
        {
            // (Genre) est un cast ici
            // sans ce cast genre est juste un "object"
            // On ne peut pas accéder à
            // genre.Id ou genre.Libelle 

            Genre genre = (Genre)cbxDvdGenresEditAdd.SelectedItem;
            Public lePublic = (Public)cbxDvdPublicsEditAdd.SelectedItem;
            Rayon rayon = (Rayon)cbxDvdRayonsEditAdd.SelectedItem;

            if (txbDvdNumero.Text.Equals("") || txbDvdTitre.Text.Equals(""))
            {
                MessageBox.Show("Numéro et titre obligatoires !", "Erreur");
                return;
            }

            if (genre == null || lePublic == null || rayon == null)
            {
                MessageBox.Show("Genre, Public et Rayon obligatoires !",
                                "Erreur");
                return;
            }

            // parse the input string to integer
            // we use int.TryParse when we are unsure about the format or validity of the input string,
            // such as when processing user input from a text box or reading data from a file

            if (!int.TryParse(txbDvdDuree.Text, out int duree))
            {
                MessageBox.Show("La durée doit être un nombre !", "Erreur");
                return;
            }

            Dvd dvd = new Dvd(
                txbDvdNumero.Text,
                txbDvdTitre.Text,
                txbDvdImage.Text,
                duree,
                txbDvdRealisateur.Text,
                txbDvdSynopsis.Text,
                genre.Id,
                genre.Libelle,
                lePublic.Id,
                lePublic.Libelle,
                rayon.Id,
                rayon.Libelle
            );

            // Si txbDvdNumero.ReadOnly = true
            // On est en mode MODIFICATION
            // Il faut appeler ModifierDvd()

            if (txbDvdNumero.ReadOnly)
            {
                // si la modification a reussi
                // ModifierDvd(dvd) retourne un bool
                // if (true)  → succès → afficher liste
                // if (false) → échec  → afficher erreur

                if (controller.ModifierDvd(dvd))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    ModeEditionDvd(false);
                    MessageBox.Show("Dvd modifié avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification !",
                                    "Erreur");
                }
            }
            else
            {
                Dvd dvdExistant = lesDvd.Find(x => x.Id.Equals(txbDvdNumero.Text));

                if (dvdExistant != null)
                {
                    MessageBox.Show(
                        $"Le numéro {txbDvdNumero.Text} existe déjà !",
                        "Erreur");
                    return;
                }

                if (controller.CreerDvd(dvd))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    ModeEditionDvd(false);
                    MessageBox.Show("Dvd ajouté avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout !", "Erreur");
                }
            }

        }

        /// <summary>
        /// Annule les Ajouts ou les modifications saisies
        /// </summary>
        /// <param name="sender"></param>
        /// sender = l'objet qui a déclenché l'événement
        ///  → ici c'est le bouton btnAnnulerDvd
        /// e = les informations sur l'événement
        ///     → ici c'est un simple clic
        /// <param name="e"></param>

        private void btnAnnulerDvd_Click(object sender, EventArgs e)
        {
            ModeEditionDvd(false);
            RemplirDvdListeComplete();

        }


        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);


            // Combos édition / ajout pour les separer les bindings
            RemplirComboCategorie(controller.GetAllGenres(), bdgRevuesGenresEditAdd, cbxRevueGenresEditAdd);
            RemplirComboCategorie(controller.GetAllPublics(), bdgRevuesPublicsEditAdd, cbxRevuePublicsEditAdd);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRevuesRayonsEditAdd, cbxRevueRayonsEditAdd);

            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Met les champs en mode édition ou lecture seule
        /// </summary>
        /// <param name="modeEdition">true = édition, false = lecture</param>
        private void ModeEditionRevue(bool modeEdition)
        {
            // Champs texte
            txbRevuesTitre.ReadOnly = !modeEdition;
            txbRevuesPeriodicite.ReadOnly = !modeEdition;
            txbRevuesDateMiseADispo.ReadOnly = !modeEdition;
            txbRevuesImage.ReadOnly = !modeEdition;

            // Boutons Valider/Annuler
            btnValiderRevue.Visible = modeEdition;
            btnAnnulerRevue.Visible = modeEdition;

            // Boutons Ajout/Modifier/Supprimer
            btnAjoutRevue.Visible = !modeEdition;
            btnModifierRevue.Visible = !modeEdition;
            btnSupprimerRevue.Visible = !modeEdition;

            // Textbox Genre/Public/Rayon (lecture)
            txbRevuesGenre.Visible = !modeEdition;
            txbRevuesPublic.Visible = !modeEdition;
            txbRevuesRayon.Visible = !modeEdition;

            // Combos EditAdd (édition)
            cbxRevueGenresEditAdd.Visible = modeEdition;
            cbxRevuePublicsEditAdd.Visible = modeEdition;
            cbxRevueRayonsEditAdd.Visible = modeEdition;

            // Désactiver recherche en mode édition
            cbxRevuesGenres.Enabled = !modeEdition;
            cbxRevuesPublics.Enabled = !modeEdition;
            cbxRevuesRayons.Enabled = !modeEdition;
            txbRevuesNumRecherche.Enabled = !modeEdition;
            txbRevuesTitreRecherche.Enabled = !modeEdition;
            btnRevuesNumRecherche.Enabled = !modeEdition;
            btnRevuesAnnulGenres.Enabled = !modeEdition;
            btnRevuesAnnulPublics.Enabled = !modeEdition;
            btnRevuesAnnulRayons.Enabled = !modeEdition;
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Passe en mode édition pour l'ajout d'une nouvelle revue.
        /// Vide les champs .
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnAjoutRevue
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>
        private void btnAjoutRevue_Click(object sender, EventArgs e)
        {
            // Vide les champs
            VideRevuesInfos();
            // Passe en mode édition
            ModeEditionRevue(true);
            // On peut mettre un id 
            txbRevuesNumero.ReadOnly = false;
            txbRevuesNumero.Focus();
        }

        /// <summary>
        /// Modifie une revue.
        /// On passe en mode edition
        /// L'id de la revue reste en lecture seule.
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnModifierRevue
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>
        private void btnModifierRevue_Click(object sender, EventArgs e)
        {
            // on passe en mode édition
            ModeEditionRevue(true);
            //on ne peut changer l'id
            txbRevuesNumero.ReadOnly = true;
        }

        /// <summary>
        /// Supprime la revue sélectionnée après confirmation.
        /// Impossible si la revue possède des exemplaires ou commandes.
        /// </summary>
        /// <param name="sender">
        /// L'objet qui a déclenché l'événement
        /// → ici c'est le bouton btnSupprimerRevue
        /// </param>
        /// <param name="e">
        /// Les informations sur l'événement
        /// → ici c'est un simple clic sur le bouton
        /// </param>
        private void btnSupprimerRevue_Click(object sender, EventArgs e)
        {
            // bdgRevuesListe.List : la liste de toutes les revues affichées dans le datagrid
            // bdgRevuesListe.Position : l'index de la ligne sélectionnée dans le datagrid
            // (Revue) : cast: on dit "cet objet est une Revue"
            // Si on clique sur la ligne 3 du datagrid Position = 3
            // List[3] = la revue à la ligne 3
            // revue = cette Revue
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            if (revue != null)
            {
                // demande de confirmation
                // result = DialogResult.Yes  si l'utilisateur clique Oui
                // result = DialogResult.No   si l'utilisateur clique Non
                DialogResult result = MessageBox.Show(
                    $"Voulez-vous supprimer {revue.Titre} ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // fais appel au controller pour supprimer
                    // controller.SupprimerRevue(revue) envoie la demande de suppression
                    // à la base de données.
                    // retourne true  si la suppression a réussi
                    // retourne false si impossible (exemplaires ou commandes liés)
                    if (controller.SupprimerRevue(revue))
                    {
                        // retourne toutes les revues à partir de la BDD
                        lesRevues = controller.GetAllRevues();
                        // Affiche la liste complète des revues
                        // et annule toutes les recherches et filtres
                        RemplirRevuesListeComplete();
                        MessageBox.Show($"{revue.Titre} supprimée avec succès !");
                    }
                    else
                    {
                        MessageBox.Show($"Impossible de supprimer {revue.Titre}, elle possède des exemplaires ou commandes.");
                    }
                }
            }

        }

        /// <summary>
        /// Valide les Ajouts ou les modifications saisies
        /// </summary>
        /// <param name="sender"></param>
        /// sender = l'objet qui a déclenché l'événement
        ///  → ici c'est le bouton btnValiderRevue
        /// e = les informations sur l'événement
        ///     → ici c'est un simple clic
        /// <param name="e"></param>

        private void btnValiderRevue_Click(object sender, EventArgs e)
        {
            // (Genre) est un cast ici
            // sans ce cast genre est juste un "object"
            // On ne peut pas accéder à
            // genre.Id ou genre.Libelle
            Genre genre = (Genre)cbxRevueGenresEditAdd.SelectedItem;
            Public lePublic = (Public)cbxRevuePublicsEditAdd.SelectedItem;
            Rayon rayon = (Rayon)cbxRevueRayonsEditAdd.SelectedItem;

            // 1. Vérification champs obligatoires
            if (txbRevuesNumero.Text.Equals("") ||
                txbRevuesTitre.Text.Equals(""))
            {
                MessageBox.Show("Numéro et titre obligatoires !", "Erreur");
                return;
            }

            // 2. Vérification combos obligatoires
            if (genre == null || lePublic == null || rayon == null)
            {
                MessageBox.Show("Genre, Public et Rayon obligatoires !", "Erreur");
                return;
            }

            // 3. Vérification que le délai est un nombre
            // on utilise int.TryParse car DelaiMiseADispo est un int
            // et l'utilisateur peut saisir n'importe quoi
            if (!int.TryParse(txbRevuesDateMiseADispo.Text, out int delaiMiseADispo))
            {
                MessageBox.Show("Le délai de mise à disposition doit être un nombre !", "Erreur");
                return;
            }

            // on converti d'abord en int
            if (!int.TryParse(txbRevuesNumero.Text, out int idInt))
            {
                MessageBox.Show("Le numéro doit être numérique.", "Erreur");
                return;
            }
            String idConvertiString = idInt.ToString("D5");
            MessageBox.Show($"Texte saisi : '{txbRevuesNumero.Text}'\nidInt : {idInt}\nidConvertiString : '{idConvertiString}'");


            // Création de l'objet Revue
            Revue revue = new Revue(
                idConvertiString,
                txbRevuesTitre.Text,
                txbRevuesImage.Text,
                genre.Id,
                genre.Libelle,
                lePublic.Id,
                lePublic.Libelle,
                rayon.Id,
                rayon.Libelle,
                txbRevuesPeriodicite.Text,
                delaiMiseADispo
            );

            // Si txbRevuesNumero.ReadOnly = true
            // On est en mode MODIFICATION
            // Il faut appeler ModifierRevue()
            if (txbRevuesNumero.ReadOnly)
            {
                // si la modification a reussi
                // ModifierRevue(revue) retourne un bool
                // if (true)  → succès → afficher liste
                // if (false) → échec  → afficher erreur
                if (controller.ModifierRevue(revue))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    ModeEditionRevue(false);
                    MessageBox.Show("Revue modifiée avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification !", "Erreur");
                }
            }
            else
            {
                // 4. Vérification id existant (mode ajout seulement)

                Revue revueExistante = lesRevues.Find(x => x.Id.Equals((idConvertiString)));
                if (revueExistante != null)
                {
                    MessageBox.Show(
                        $"Le numéro {txbRevuesNumero.Text} existe déjà !",
                        "Erreur");
                    return;
                }

                if (controller.CreerRevue(revue))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    ModeEditionRevue(false);
                    MessageBox.Show("Revue ajoutée avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout !", "Erreur");
                }
            }
        }

        /// <summary>
        /// Annule les Ajouts ou les modifications saisies
        /// </summary>
        /// <param name="sender"></param>
        /// sender = l'objet qui a déclenché l'événement
        ///  → ici c'est le bouton btnAnnulerRevue
        /// e = les informations sur l'événement
        ///     → ici c'est un simple clic
        /// <param name="e"></param>
        private void btnAnnulerRevue_Click(object sender, EventArgs e)
        {
            ModeEditionRevue(false);
            RemplirRevuesListeComplete();
        }


        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }


        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }


        #endregion

        #region Onglet Commandes Livres
        private readonly BindingSource bdgCommandesLivresListe = new BindingSource();
        private readonly BindingSource bdgSuivisListe = new BindingSource();
        private readonly BindingSource bdgLivresCommandesCombo = new BindingSource();
        private List<Livre> lesLivresCommandes = new List<Livre>();
        private List<Suivi> lesSuivis = new List<Suivi>();
        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();
        private List<CommandeDocument> toutesCommandeslivres = new List<CommandeDocument>();
        private bool chargementCommandesLivres = false;


        //utilise pour le combobox des livres : bdgLivresCommandesCombo = new BindingSource();
        // utilise pour le datagridview of bookorders bdgCommandesLivresListe

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheCommandeLivresInfos(Livre livre)
        {

            txtAuteurLivreCommandes.Text = livre.Auteur;
            txtCollectionLivreCommandes.Text = livre.Collection;
            txtIsbnLivreCommandes.Text = livre.Isbn;
            txtGenreLivreCommandes.Text = livre.Genre;
            txtPublicLivreCommandes.Text = livre.Public;
            txtRayonLivreCommandes.Text = livre.Rayon;
            txtTitreLivreCommandes.Text = livre.Titre;
            txtIdLivreCommandes.Text = livre.Id;
            string image = livre.Image;
            try
            {
                pcbImageLivreCommandes.Image = Image.FromFile(image);
            }
            catch
            {
                pcbImageLivreCommandes.Image = null;
            }
            //  Les commandes du livre selectionne s'affichent
            AfficheCommandesLivreListe();
        }


        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivreCommandesInfos()
        {
            txtAuteurLivreCommandes.Text = "";
            txtCollectionLivreCommandes.Text = "";
            txtIsbnLivreCommandes.Text = "";
            txtGenreLivreCommandes.Text = "";
            txtPublicLivreCommandes.Text = "";
            txtRayonLivreCommandes.Text = "";
            txtTitreLivreCommandes.Text = "";
            txtIdLivreCommandes.Text = "";
            pcbImageLivreCommandes.Image = null;
            bdgCommandesLivresListe.DataSource = null;
            dgvListeLivreCommandes.DataSource = null;

        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// Remplit le dategrid des commandes avec la liste reçue en paramètre
        /// </summary>
        ///  /// <param name=lesCommandesLivres"">liste des commandes de livres</param>


        private void RemplirCommandesLivreListe(List<CommandeDocument> lesCommandesLivres)
        {
            if (lesCommandesLivres != null)
            {
                dgvListeLivreCommandes.AutoGenerateColumns = false;
                bdgCommandesLivresListe.DataSource = lesCommandesLivres;
                dgvListeLivreCommandes.DataSource = bdgCommandesLivresListe;

                dgvListeLivreCommandes.Columns["colDate"].DisplayIndex = 0;
                dgvListeLivreCommandes.Columns["colMontant"].DisplayIndex = 1;
                dgvListeLivreCommandes.Columns["colNbExemplaires"].DisplayIndex = 2;
                dgvListeLivreCommandes.Columns["colSuivi"].DisplayIndex = 3;

                dgvListeLivreCommandes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                foreach (DataGridViewRow row in dgvListeLivreCommandes.Rows)
                {
                    string idSuivi = row.Cells["colSuivi"].Value?.ToString();
                    if (idSuivi != null)
                    {
                        Suivi suivi = lesSuivis.Find(x => x.Id == idSuivi);
                        if (suivi != null)
                        {
                            row.Cells["colSuivi"].Value = suivi.Libelle;
                        }
                    }
                }
            }
            else
            {
                bdgCommandesLivresListe.DataSource = null;
                dgvListeLivreCommandes.DataSource = null;
            }
        }
        
        /// <summary>
        /// Récupère et affiche les commandes d'un livre
        /// </summary>
        private void AfficheCommandesLivreListe()
        {
            // id du livre selectionne
            string idLivreDvd = txtIdLivreCommandes.Text;

            //recupere les lignes de commandedocument pour ce livre
            List<CommandeDocument> commandesDoc = controller.GetCommandeDocument(idLivreDvd);
            // toutes les commandes de la table commande
            List<Commande> commandes = controller.GetAllCommandes();
            List<CommandeDocument> nouvelleCommandes = new List<CommandeDocument>();

            foreach (CommandeDocument cd in commandesDoc)
            {
                // On cherche dans commandes la commande qui a le même id que cd
                Commande commande = commandes.Find(c => c.Id == cd.Id);

                if (commande != null)
                {
                    CommandeDocument nouvelleCommande = new CommandeDocument(
                        cd.Id,
                        cd.NbExemplaire,
                        cd.IdLivreDvd,
                        cd.IdSuivi,
                        commande.DateCommande,
                        commande.Montant
                    );

                    nouvelleCommandes.Add(nouvelleCommande);
                }
            }

            lesCommandesLivres = nouvelleCommandes;
            RemplirCommandesLivreListe(lesCommandesLivres);
        }


        /// <summary>
        /// Quand on clique sur "Commandes de Livres" on veut charger la liste des livres
        /// et charger la liste des étapes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void tabCommandesLivres_Enter(object sender, EventArgs e)
        {
            chargementCommandesLivres = true;

            VideLivreCommandesInfos();
            // charge les livres
            lesLivresCommandes = controller.GetAllLivres();

            // bindSource est l'intermediare entre la liste et
            // le composant visuel 
            //Sans BindingSource en cas de modification le ComboBox ne se met pas à jour !



            // Remplit ComboBox livres 

            //On met les données dans la BindingSource
            bdgLivresCommandesCombo.DataSource = lesLivresCommandes;

            // On relie le ComboBox à la BindingSource
            cbxTitreLivreCommandes.DataSource = bdgLivresCommandesCombo;
            cbxTitreLivreCommandes.DisplayMember = "Titre";
            cbxTitreLivreCommandes.SelectedIndex = -1;
            // charge les etapes 
            lesSuivis = controller.GetAllSuivis();

            // Remplit ComboBox etapes
            bdgSuivisListe.DataSource = lesSuivis;
            cbxEtapeLivresCommandes.DataSource = bdgSuivisListe;
            cbxEtapeLivresCommandes.DisplayMember = "Libelle";
            cbxEtapeLivresCommandes.SelectedIndex = -1;
            cbxEtapeLivresCommandes.Text = "";
            chargementCommandesLivres = false;
        }


        /// <summary>
        /// Filtre sur l'Id selectionne dans le comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTitreLivreCommandes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chargementCommandesLivres) return;

            if (cbxTitreLivreCommandes.SelectedIndex >= 0)
            {
                // on fait un cast pour acceder a livre, si pas de cast on 
                //peut pas acceder a livre.Titre
                Livre livre = (Livre)cbxTitreLivreCommandes.SelectedItem;


                if (livre != null)
                {
                    AfficheCommandeLivresInfos(livre);

                }
                else
                {
                    MessageBox.Show("livre introuvable");
                }
            }


        }


        /// <summary>
        /// Enregistrement d'une nouvelle commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();

        private void btnEnregistrerLivreCommandes_Click(object sender, EventArgs e)
        {
            // on genere un id

            List<Commande> toutesCommandes = controller.GetAllCommandes();


            if (!txtIdLivreCommandes.Text.Equals(""))
            {
                try
                {
                    string livreId = txtIdLivreCommandes.Text;
                    DateTime dateCommande = dtpNouvelleLivreCommandes.Value;
                    if (!double.TryParse(txtboxMontantLivresCommandes.Text, out double montant))
                    {
                        MessageBox.Show("Le montant doit être un nombre valide.", "Erreur");
                        return;
                    }
                    if (!int.TryParse(txtboxQuantiteLivresCommandes.Text, out int nbExemplaire))
                    {
                        MessageBox.Show("La quantité doit être un nombre valide.", "Erreur");
                        return;
                    }

                    // une nouvelle commande est toujours en cours 
                    //l'id de en cours est 1000

                    string idSuivi = "10000";


                    // Genere un nouvel id s'il n'y a aucune commande l'id est 00001 

                    string id = toutesCommandes.Count > 0
                ? (toutesCommandes.Max(x => int.Parse(x.Id)) + 1).ToString("D5")
                : "00001";

                    CommandeDocument commandedoc = new CommandeDocument(
                        id,
                        nbExemplaire,
                        livreId,
                        idSuivi,
                        dateCommande,
                        montant);

                    if (controller.CreerCommande(commandedoc))
                    {
                        AfficheCommandesLivreListe();
                    }
                    else
                    {
                        MessageBox.Show("Erreur dans la création de la commande", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("Montant et quantité doivent être numériques", "Information");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un livre !", "Information");
            }
        }


        private void btnSupprimerLivresCommandes_Click(object sender, EventArgs e)
        {
            // on verifie qu'une commande est selectionnee 
            if (bdgCommandesLivresListe.Position < 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            // on recupere l'objet selectionne et on fait un cast pour dire 
            // cet objet est une CommandeDocument

            CommandeDocument commandesLivres =
                (CommandeDocument)bdgCommandesLivresListe.List[bdgCommandesLivresListe.Position];

            if (commandesLivres == null)
            {
                MessageBox.Show("Aucune commande sélectionnée.", "Information");
                return;
            }

            // impossible de supprimer une commande deja livree
            if (commandesLivres.IdSuivi == "10002")
            {
                MessageBox.Show(
                    "Impossible de supprimer une commande déjà livrée.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Demande de confirmation
            DialogResult result = MessageBox.Show(
                $"Voulez-vous supprimer la commande {commandesLivres.Id} ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {


                // supprimer dans commande (table mère)
                bool suppression1 = controller.SupprimerCommande(commandesLivres.Id);

                if (suppression1)
                {
                    //  Recharge la liste depuis la BDD
                    AfficheCommandesLivreListe();
                    MessageBox.Show(
                        $"Commande {commandesLivres.Id} supprimée avec succès !",
                        "Succès",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Erreur lors de la suppression de la commande {commandesLivres.Id}.",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnModifierLivresCommandes_Click(object sender, EventArgs e)
        {
            string en_cours = "10000";
            string relancee = "10001";
            string livree = "10002";
            string reglee = "10003";

            // on verifie qu'une commande est selectionnee 
            if (bdgCommandesLivresListe.Position < 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            // on recupere l'objet selectionne et on fait un cast pour dire 
            // cet objet est une CommandeDocument

            CommandeDocument commandesLivres =
                (CommandeDocument)bdgCommandesLivresListe.List[bdgCommandesLivresListe.Position];


            if (cbxEtapeLivresCommandes != null && commandesLivres != null)
            {
                Suivi nouveauSuivi = (Suivi)cbxEtapeLivresCommandes.SelectedItem;
                string ancienIdSuivi = commandesLivres.IdSuivi;
                string nouveauIdSuivi = nouveauSuivi.Id;


                if (int.Parse(nouveauIdSuivi) > int.Parse(ancienIdSuivi))
                {
                    if (int.Parse(nouveauIdSuivi) == int.Parse(reglee) && int.Parse(ancienIdSuivi) != int.Parse(livree))
                    {
                        MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée. ", "Erreur");
                        return;
                    }


                    CommandeDocument commandeModifiee = new CommandeDocument(
                        commandesLivres.Id,
                        commandesLivres.NbExemplaire,
                        commandesLivres.IdLivreDvd,
                        nouveauIdSuivi,
                        commandesLivres.DateCommande,
                        commandesLivres.Montant
                        );
                    // envoi la methode au controller
                    if (controller.ModifierSuiviCommande(commandeModifiee))
                    {
                        AfficheCommandesLivreListe();
                    }
                    else
                    {
                        MessageBox.Show("Erreur dans la création de la commande", "Erreur");

                    }
                }
                else
                {
                    MessageBox.Show("une commande ne peut pas revenir à une étape précédente", "Erreur");


                }



            }



        }


        #endregion

        #region Onglet Commandes Dvd
        private readonly BindingSource bdgCommandesDvdListe = new BindingSource();
        private readonly BindingSource bdgSuivisDvdListe = new BindingSource();
        private readonly BindingSource bdgDvdCommandesCombo = new BindingSource();
        private List<Dvd> lesDvdCommandes = new List<Dvd>();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        private List<CommandeDocument> toutesCommandesdvd = new List<CommandeDocument>();
        private bool chargementCommandesDvd = false;

        //bdgCommandesDvdListe : pour le datagridview des commandes


        /// <summary>
        /// Affichage des informations du Dvd sélectionné
        /// </summary>
        /// <param name="Dvd">le Dvd</param>
        private void AfficheCommandeDvdInfos(Dvd dvd)
        {

            txtRealisateurDvdCommandes.Text = dvd.Realisateur;
            // .ToString car Text attend un string alors qu'il s'agit d'un int
            txtDureeDvdCommandes.Text = dvd.Duree.ToString();
            txtSynopsisDvdCommandes.Text = dvd.Synopsis;
            cbxGenreDvdCommandes.Text = dvd.Genre;
            cbxPublicDvdCommandes.Text = dvd.Public;
            cbxRayonDvdCommandes.Text = dvd.Rayon;
            txtTitreDvdCommandes.Text = dvd.Titre;
            txtIdDvdCommandes.Text = dvd.Id;
            string image = dvd.Image;
            try
            {
                pcbImageDvdCommandes.Image = Image.FromFile(image);
            }
            catch
            {
                pcbImageDvdCommandes.Image = null;
            }
            //  Les commandes s'affichent
            AfficheCommandesDvdListe();
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdCommandesInfos()
        {

            txtRealisateurDvdCommandes.Text = "";
            txtDureeDvdCommandes.Text = "";
            txtSynopsisDvdCommandes.Text = "";
            cbxGenreDvdCommandes.Text = "";
            cbxPublicDvdCommandes.Text = "";
            cbxRayonDvdCommandes.Text = "";
            txtTitreDvdCommandes.Text = "";
            txtIdDvdCommandes.Text = "";
            string image = "";
            pcbImageDvdCommandes.Image = null;
            bdgCommandesDvdListe.DataSource = null;
            dgvListeDvdCommandes.DataSource = null;

        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// Remplit le dategrid des commandes avec la liste reçue en paramètre
        /// </summary>
        ///  /// <param name=lesCommandesDvd"">liste des commandes de dvd</param>


        private void RemplirCommandesDvdListe(List<CommandeDocument> lesCommandesDvd)
        {
            if (lesCommandesDvd != null)
            {
                dgvListeDvdCommandes.AutoGenerateColumns = false;
                bdgCommandesDvdListe.DataSource = lesCommandesDvd;
                dgvListeDvdCommandes.DataSource = bdgCommandesDvdListe;

                dgvListeDvdCommandes.Columns["colDateDvd"].DisplayIndex = 0;
                dgvListeDvdCommandes.Columns["colMontantDvd"].DisplayIndex = 1;
                dgvListeDvdCommandes.Columns["colExemplairesDvd"].DisplayIndex = 2;
                dgvListeDvdCommandes.Columns["colEtapeDvd"].DisplayIndex = 3;

                dgvListeDvdCommandes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                foreach (DataGridViewRow row in dgvListeDvdCommandes.Rows)
                {
                    string idSuivi = row.Cells["colEtapeDvd"].Value?.ToString();
                    if (idSuivi != null)
                    {
                        Suivi suivi = lesSuivis.Find(x => x.Id == idSuivi);
                        if (suivi != null)
                        {
                            row.Cells["colEtapeDvd"].Value = suivi.Libelle;
                        }
                    }
                }
            }
            else
            {
                bdgCommandesDvdListe.DataSource = null;
                dgvListeDvdCommandes.DataSource = null;
            }
        }
        /// <summary>
        /// Récupère et affiche les commandes d'un dvd
        /// </summary>
        private void AfficheCommandesDvdListe()
        {
            // id du livre selectionne
            string idLivreDvd = txtIdDvdCommandes.Text;

            //recupere les lignes de commandedocument pour ce dvd
            List<CommandeDocument> commandesDoc = controller.GetCommandeDocument(idLivreDvd);
            // toutes les commandes de la table commande
            List<Commande> commandes = controller.GetAllCommandes();
            List<CommandeDocument> nouvelleCommandes = new List<CommandeDocument>();

            foreach (CommandeDocument cd in commandesDoc)
            {
                // On cherche dans commandes la commande qui a le même id que cd
                Commande commande = commandes.Find(c => c.Id == cd.Id);

                if (commande != null)
                {
                    CommandeDocument nouvelleCommande = new CommandeDocument(
                        cd.Id,
                        cd.NbExemplaire,
                        cd.IdLivreDvd,
                        cd.IdSuivi,
                        commande.DateCommande,
                        commande.Montant
                    );

                    nouvelleCommandes.Add(nouvelleCommande);
                }
            }

            lesCommandesDvd = nouvelleCommandes;
            RemplirCommandesDvdListe(lesCommandesDvd);
        }



        /// <summary>
        /// Quand on clique sur "Commandes de Dvd" on veut charger la liste des dvd
        /// et charger la liste des étapes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void tabCommandesDvd_Enter(object sender, EventArgs e)
        {
            chargementCommandesDvd = true;

            VideDvdCommandesInfos();
            // charge les dvd
            lesDvdCommandes = controller.GetAllDvd();

            // bindSource est l'intermediare entre la liste et
            // le composant visuel 
            //Sans BindingSource en cas de modification le ComboBox ne se met pas à jour !



            // Remplit ComboBox dvd 

            //On met les données dans la BindingSource
            bdgDvdCommandesCombo.DataSource = lesDvdCommandes;

            // On relie le ComboBox à la BindingSource
            cbxTitreDvdCommandes.DataSource = bdgDvdCommandesCombo;
            cbxTitreDvdCommandes.DisplayMember = "Titre";
            cbxTitreDvdCommandes.SelectedIndex = -1;
            // charge les etapes 
            lesSuivis = controller.GetAllSuivis();

            // Remplit ComboBox etapes
            bdgSuivisListe.DataSource = lesSuivis;
            cbxEtapeDvdCommandes.DataSource = bdgSuivisListe;
            cbxEtapeDvdCommandes.DisplayMember = "Libelle";
            cbxEtapeDvdCommandes.SelectedIndex = -1;
            cbxEtapeDvdCommandes.Text = "";
            chargementCommandesDvd = false;
        }


        /// <summary>
        /// Filtre sur l'Id selectionne dans le comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTitreDvdCommandes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chargementCommandesDvd) return;



            if (cbxTitreDvdCommandes.SelectedIndex >= 0)
            {
                // on fait un cast pour acceder a Dvd, si pas de cast on 
                //peut pas acceder a Dvd.Titre
                Dvd dvd = (Dvd)cbxTitreDvdCommandes.SelectedItem;


                if (dvd != null)
                {
                    AfficheCommandeDvdInfos(dvd);

                }
                else
                {
                    MessageBox.Show("dvd introuvable");
                }
            }


        }



        /// <summary>
        /// Enregistrement d'une nouvelle commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();

        private void btnEnregistrerDvdCommandes_Click(object sender, EventArgs e)
        {
            // on genere un id

            List<Commande> toutesCommandes = controller.GetAllCommandes();


            if (!txtIdDvdCommandes.Text.Equals(""))
            {
                try
                {
                    string dvdId = txtIdDvdCommandes.Text;
                    DateTime dateCommande = dtpNouvelleDvdCommandes.Value;
                    if (!double.TryParse(txtboxMontantDvdCommandes.Text, out double montant))
                    {
                        MessageBox.Show("Le montant doit être un nombre valide.", "Erreur");
                        return;
                    }
                    if (!int.TryParse(txtboxQuantiteDvdCommandes.Text, out int nbExemplaire))
                    {
                        MessageBox.Show("La quantité doit être un nombre valide.", "Erreur");
                        return;
                    }

                    // une nouvelle commande est toujours en cours 
                    //l'id de en cours est 1000

                    string idSuivi = "10000";


                    // Genere un nouvel id s'il n'y a aucune commande l'id est 00001 

                    string id = toutesCommandes.Count > 0
                ? (toutesCommandes.Max(x => int.Parse(x.Id)) + 1).ToString("D5")
                : "00001";

                    CommandeDocument commandedoc = new CommandeDocument(
                        id,
                        nbExemplaire,
                        dvdId,
                        idSuivi,
                        dateCommande,
                        montant);

                    if (controller.CreerCommande(commandedoc))
                    {
                        AfficheCommandesDvdListe();
                    }
                    else
                    {
                        MessageBox.Show("Erreur dans la création de la commande", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("Montant et quantité doivent être numériques", "Information");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un dvd !", "Information");
            }
        }


        private void btnSupprimerDvdCommandes_Click(object sender, EventArgs e)
        {
            // on verifie qu'une commande est selectionnee 
            if (bdgCommandesDvdListe.Position < 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            // on recupere l'objet selectionne et on fait un cast pour dire 
            // cet objet est une CommandeDocument

            CommandeDocument commandesDvd =
                (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];

            if (commandesDvd == null)
            {
                MessageBox.Show("Aucune commande sélectionnée.", "Information");
                return;
            }

            // impossible de supprimer une commande deja livree
            if (commandesDvd.IdSuivi == "10002")
            {
                MessageBox.Show(
                    "Impossible de supprimer une commande déjà livrée.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Demande de confirmation
            DialogResult result = MessageBox.Show(
                $"Voulez-vous supprimer la commande {commandesDvd.Id} ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {


                // supprimer dans commande (table mère)
               bool suppression = controller.SupprimerCommande(commandesDvd.Id);

                if (suppression)
                {
                    //  Recharge la liste depuis la BDD
                    AfficheCommandesDvdListe();
                    MessageBox.Show(
                        $"Commande {commandesDvd.Id} supprimée avec succès !",
                        "Succès",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Erreur lors de la suppression de la commande {commandesDvd.Id}.",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnModifierDvdCommandes_Click(object sender, EventArgs e)
        {
            string en_cours = "10000";
            string relancee = "10001";
            string livree = "10002";
            string reglee = "10003";

            // on verifie qu'une commande est selectionnee 
            if (bdgCommandesDvdListe.Position < 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            // on recupere l'objet selectionne et on fait un cast pour dire 
            // cet objet est une CommandeDocument

            CommandeDocument commandesDvd =
                (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];


            if (cbxEtapeDvdCommandes != null && commandesDvd != null)
            {
                Suivi nouveauSuivi = (Suivi)cbxEtapeDvdCommandes.SelectedItem;
                string ancienIdSuivi = commandesDvd.IdSuivi;
                string nouveauIdSuivi = nouveauSuivi.Id;


                if (int.Parse(nouveauIdSuivi) > int.Parse(ancienIdSuivi))
                {
                    if (int.Parse(nouveauIdSuivi) == int.Parse(reglee) && int.Parse(ancienIdSuivi) != int.Parse(livree))
                    {
                        MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée. ", "Erreur");
                        return;
                    }


                    CommandeDocument commandeModifiee = new CommandeDocument(
                        commandesDvd.Id,
                        commandesDvd.NbExemplaire,
                        commandesDvd.IdLivreDvd,
                        nouveauIdSuivi,
                        commandesDvd.DateCommande,
                        commandesDvd.Montant
                        );
                    // envoi la methode au controller
                    if (controller.ModifierSuiviCommande(commandeModifiee))
                    {
                        AfficheCommandesDvdListe();
                    }
                    else
                    {
                        MessageBox.Show("Erreur dans la création de la commande", "Erreur");

                    }
                }
                else
                {
                    MessageBox.Show("une commande ne peut pas revenir à une étape précédente", "Erreur");


                }



            }
        }


        #endregion
        #region Onglet Commandes Revues

        private readonly BindingSource bdgCommandesRevuesDatagrid = new BindingSource();
        private readonly BindingSource bdgCommandesRevuesCombobox = new BindingSource();
        private List<Revue> lesRevuesCommandes = new List<Revue>();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        private List<Exemplaire> lesCommandesRevuesExemplaires = new List<Exemplaire>();
        private bool chargementCommandesRevues = false;


        /// <summary>
        /// Quand on clique sur "Commandes de Revues" on veut charger la liste des revues

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void tabCommandesRevues_Enter(object sender, EventArgs e)
        {
            chargementCommandesRevues = true;

            VideRevueCommandesInfos();

            // charge les revues
            lesRevuesCommandes = controller.GetAllRevues();

            // bindSource est l'intermediare entre la liste et
            // le composant visuel 
            //Sans BindingSource en cas de modification le ComboBox ne se met pas à jour !



            // Remplit ComboBox revues 

            //On met les données dans la BindingSource
            bdgCommandesRevuesCombobox.DataSource = lesRevuesCommandes;

            // On relie le ComboBox à la BindingSource

            cbxTitreRevuesCommande.DataSource = bdgCommandesRevuesCombobox;
            cbxTitreRevuesCommande.DisplayMember = "Titre";
            cbxTitreRevuesCommande.SelectedIndex = -1;


            chargementCommandesRevues = false;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la revue
        /// </summary>
        private void VideRevueCommandesInfos()
        {
            txtPeriodiciteRevuesCommandes.Text = "";
            txtDelaiRevuesCommandes.Text = "";
            txtIdRevuesCommandes.Text = "";
            txtGenreRevuesCommandes.Text ="";
            txtPublicRevuesCommandes.Text = "";
            txtRayonRevuesCommandes.Text = "";
            txtTitreRevuesCommandes.Text = "";
            pcbRevuesImage.Image = null;
            dgvListeRevuesAbo.DataSource = null;
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheCommandeRevuesInfos(Revue revue)
        {

            txtPeriodiciteRevuesCommandes.Text = revue.Periodicite;
            txtDelaiRevuesCommandes.Text = revue.DelaiMiseADispo.ToString();
            txtIdRevuesCommandes.Text = revue.Id;
            txtGenreRevuesCommandes.Text = revue.Genre;
            txtPublicRevuesCommandes.Text = revue.Public;
            txtRayonRevuesCommandes.Text = revue.Rayon;
            txtTitreRevuesCommandes.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
            // affiche les abonnements de cette revue
            AfficheAbonnementRevuesListe();
        }


        /// <summary>
        /// Remplit le dategrid des Abonnements avec la liste reçue en paramètre
        /// </summary>
        ///  /// <param name="lesAbonnements">liste des abonnements</param>


        private void RemplirAbonnementsRevuesListe(List<Abonnement> lesAbonnements)
        {
            
            if (lesAbonnements != null)
            {
                dgvListeRevuesAbo.AutoGenerateColumns = false;
                bdgCommandesRevuesDatagrid.DataSource = lesAbonnements;
                dgvListeRevuesAbo.DataSource = bdgCommandesRevuesDatagrid;

                dgvListeRevuesAbo.Columns["colDateAbo"].DisplayIndex = 0;
                dgvListeRevuesAbo.Columns["colMontantAbo"].DisplayIndex = 1;
                dgvListeRevuesAbo.Columns["colFinAbo"].DisplayIndex = 2;


                dgvListeRevuesAbo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }
            else
            {
                bdgCommandesRevuesDatagrid.DataSource = null;
                dgvListeRevuesAbo.DataSource = null;
            }
        }

        /// <summary>
        /// Récupère et affiche les abonnement d'une revue
        /// </summary>
        private void AfficheAbonnementRevuesListe()
        {
            // id de la revue selectionne
            string idRevue = txtIdRevuesCommandes.Text;

            //recupere les lignes d'abonnement pour cette revue
            List<Abonnement> abonnements = controller.GetAbonnements(idRevue);

            

            // tous les abonnements de la table abonnement
            List<Abonnement> tousLesAbonnementsDeLaRevue = new List<Abonnement>();
            // toutes les commandes
            List<Commande> commandes = controller.GetAllCommandes();

            foreach (Abonnement abo in abonnements)
            {
                // On cherche dans commandes la commande qui a le même id que abo
                Commande commande = commandes.Find(c => c.Id == abo.Id);

                if (commande != null)
                {
                    Abonnement nouvelAbo = new Abonnement(
                        abo.Id,
                        abo.DateFinAbonnement,
                        abo.IdRevue,
                        // ces proprietes sont heritees de commande
                        commande.DateCommande,
                        commande.Montant

                    );

                    tousLesAbonnementsDeLaRevue.Add(nouvelAbo);
                 
                }
            }

            lesAbonnements = tousLesAbonnementsDeLaRevue.OrderByDescending(a => a.DateCommande).ToList();
            RemplirAbonnementsRevuesListe(lesAbonnements);


        }

        /// <summary>
        /// Filtre sur l'Id selectionne dans le comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTitreRevuesCommande_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chargementCommandesRevues) return;

          

            if (cbxTitreRevuesCommande.SelectedIndex >= 0)
            {
                // on fait un cast pour acceder a Revue, si pas de cast on 
                //peut pas acceder a Revue.Titre
                Revue revue = (Revue)cbxTitreRevuesCommande.SelectedItem;


                if (revue != null)
                {
                    AfficheCommandeRevuesInfos(revue);
                    


                }
                else
                {
                    MessageBox.Show("revue introuvable");
                }
            }


        }


        /// <summary>
        /// Enregistrement d'un nouvel Abonnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

       

        private void btnEnregistrerRevuesAbo_Click(object sender, EventArgs e)
        {


            List<Commande> toutesLesCommandes = controller.GetAllCommandes();

            string idRevue = txtIdRevuesCommandes.Text;

            if (!idRevue.Equals(""))
            {
                try
                {

                    DateTime dateCommande = dtpDateCommandeNouvelAboRevues.Value;
                    DateTime dateFin = dtpDateFinNouvelAboRevues.Value;
                    if (!double.TryParse(txtboxMontantRevuesAbo.Text, out double montant))
                    {
                        MessageBox.Show("Le montant doit être un nombre valide.", "Erreur");
                        return;
                    }

                    if (dateFin <= dateCommande)
                    {
                        MessageBox.Show("La date de fin d'abonnement doit être postérieure à la date de commande.", "Erreur");
                        return;
                    }

                    // Generer un nouvel id

                    string idAbo = toutesLesCommandes.Count > 0 ? (toutesLesCommandes.Max(x => int.Parse(x.Id)) + 1).ToString("D5")
                : "00001";

                    
                    // Creer un nouvel objet Abonnement
                    Abonnement nouvelAbonnement = new Abonnement(
        idAbo,
       dateFin,
       idRevue,
        dateCommande,
        montant);

                   

                    // Appeller le controlleur
                    if (controller.CreerAbonnement(nouvelAbonnement))
                    {
                        AfficheAbonnementRevuesListe();
                    }
                    else
                    {
                        MessageBox.Show("Erreur dans la création de l'abonnement", "Erreur");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur");

                }

            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une revue", "Information");
            }


        }

        /// <summary>
        /// Retourne vrai si la date de parution de l'exemplaire est entre les 2 autres dates
        /// </summary>

        private bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        private void btnSupprimerRevuesAbo_Click(object sender, EventArgs e)
        {
            // on verifie qu'un abonnement est selectionnee 
            if (bdgCommandesRevuesDatagrid.Position < 0)
            {
                MessageBox.Show("Veuillez sélectionner un abonnement.", "Information");
                return;
            }

            // on recupere l'objet selectionne et on fait un cast pour dire 
            // cet objet est un abonnement

            Abonnement abonnementRevues =
                 (Abonnement)bdgCommandesRevuesDatagrid.List[bdgCommandesRevuesDatagrid.Position];

            if (abonnementRevues == null)
            {
                MessageBox.Show("Aucun abonnement sélectionné.", "Information");
                return;
            }

            // impossible de supprimer un abonnement ayant des exemplaires
            List<Exemplaire> exemplairesDeRevue = controller.GetExemplairesRevue(abonnementRevues.IdRevue);
            
            DateTime dateCommande = abonnementRevues.DateCommande;
            DateTime dateFin = abonnementRevues.DateFinAbonnement;
            foreach (Exemplaire exempl in exemplairesDeRevue)
            {
                if (ParutionDansAbonnement(dateCommande, dateFin, exempl.DateAchat))
                {
                    MessageBox.Show("Des exemplaires sont  rattachés à cet abonnement. Impossible de le supprimer. ", "Erreur");
                    return;
                }
               
                  
            }

            // Demande de confirmation
            DialogResult result = MessageBox.Show(
                $"Voulez-vous supprimer l'abonnement {abonnementRevues.Id} ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {


                // supprimer dans commande (table mère)
                bool suppression2 = controller.SupprimerAbonnement(abonnementRevues.Id);

                if (suppression2)
                {
                    //  Recharge la liste depuis la BDD
                    AfficheAbonnementRevuesListe();
                    MessageBox.Show(
                        $"Abonnement {abonnementRevues.Id} supprimée avec succès !",
                        "Succès",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Erreur lors de la suppression de l'abonnement {abonnementRevues.Id}.",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Retourne les abonnements qui expirent dans moins de 30 jours
        /// </summary>
        public List<Abonnement> RecupererAbonnementsExpirants()
        {

            List<Abonnement> abonnementsExpirants = new List<Abonnement>();
            // on recupere tous les abonnements    
            List<Abonnement> tousAbonnements = controller.GetAllAbonnements();


            DateTime aujourdhui = DateTime.Today;
            DateTime dansUnMois = aujourdhui.AddDays(30);

            //on filtre ceux qui expirent dans < 30 jours
            foreach (Abonnement abo in tousAbonnements)
            {
                if (abo.DateFinAbonnement >= aujourdhui && abo.DateFinAbonnement < dansUnMois)
                {
                    abonnementsExpirants.Add(abo);
                }
            }

            //on trie par date (ordre chronologique)


            abonnementsExpirants = abonnementsExpirants.OrderBy(a => a.DateFinAbonnement).ToList();

            return abonnementsExpirants;
        }


        #endregion


    }
}




