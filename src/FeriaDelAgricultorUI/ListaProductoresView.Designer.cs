using System.Drawing;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    partial class ListaProductoresView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lvwProductores = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(206, 15);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Lista de productores y sus productos";
            // 
            // lvwProductores
            // 
            this.lvwProductores.FullRowSelect = true;
            this.lvwProductores.HideSelection = false;
            this.lvwProductores.Location = new System.Drawing.Point(12, 35);
            this.lvwProductores.Name = "lvwProductores";
            this.lvwProductores.Size = new System.Drawing.Size(600, 300);
            this.lvwProductores.TabIndex = 1;
            this.lvwProductores.UseCompatibleStateImageBehavior = false;
            this.lvwProductores.View = System.Windows.Forms.View.Details;
            // 
            // ListaProductoresView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.lvwProductores);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ListaProductoresView";
            this.Text = "Productores";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private ListView lvwProductores;
    }
}
