namespace MediaTekDocuments.view
{
    partial class FormulaireAlerte
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.listBxAboEx = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(325, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Abonnements expirant dans moins d\'un mois";
            // 
            // listBxAboEx
            // 
            this.listBxAboEx.FormattingEnabled = true;
            this.listBxAboEx.ItemHeight = 20;
            this.listBxAboEx.Location = new System.Drawing.Point(49, 106);
            this.listBxAboEx.Name = "listBxAboEx";
            this.listBxAboEx.Size = new System.Drawing.Size(615, 264);
            this.listBxAboEx.TabIndex = 1;
            // 
            // FormulaireAlerte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBxAboEx);
            this.Controls.Add(this.label1);
            this.Name = "FormulaireAlerte";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBxAboEx;
    }
}