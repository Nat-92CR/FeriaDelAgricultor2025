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
            this.lblProductor = new System.Windows.Forms.Label();
            this.cboProductores = new System.Windows.Forms.ComboBox();
            this.lvwProductores = new System.Windows.Forms.ListView();
            this.btnAgregarCarrito = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 15);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Lista de productores y sus productos";
            // 
            // lblProductor
            // 
            this.lblProductor.AutoSize = true;
            this.lblProductor.Location = new System.Drawing.Point(12, 34);
            this.lblProductor.Name = "lblProductor";
            this.lblProductor.Size = new System.Drawing.Size(61, 15);
            this.lblProductor.TabIndex = 1;
            this.lblProductor.Text = "Productor:";
            // 
            // cboProductores
            // 
            // cboProductores
            this.cboProductores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductores.FormattingEnabled = true;
            this.cboProductores.Location = new System.Drawing.Point(79, 31);
            this.cboProductores.Name = "cboProductores";
            this.cboProductores.Size = new System.Drawing.Size(250, 23);
            this.cboProductores.TabIndex = 2;
            this.cboProductores.SelectedIndexChanged += new System.EventHandler(this.cboProductores_SelectedIndexChanged);
            // 
            // lvwProductores
            // 
            this.lvwProductores.FullRowSelect = true;
            this.lvwProductores.Location = new System.Drawing.Point(12, 70);
            this.lvwProductores.Name = "lvwProductores";
            this.lvwProductores.Size = new System.Drawing.Size(765, 349);
            this.lvwProductores.TabIndex = 3;
            this.lvwProductores.UseCompatibleStateImageBehavior = false;
            this.lvwProductores.View = System.Windows.Forms.View.Details;
            // 
            // btnAgregarCarrito
            // 
            this.btnAgregarCarrito.Location = new System.Drawing.Point(12, 438);
            this.btnAgregarCarrito.Name = "btnAgregarCarrito";
            this.btnAgregarCarrito.Size = new System.Drawing.Size(200, 23);
            this.btnAgregarCarrito.TabIndex = 4;
            this.btnAgregarCarrito.Text = "Agregar al carrito";
            this.btnAgregarCarrito.UseVisualStyleBackColor = true;
            this.btnAgregarCarrito.Click += new System.EventHandler(this.btnAgregarCarrito_Click);
            // 
            // ListaProductoresView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 510);
            this.Controls.Add(this.btnAgregarCarrito);
            this.Controls.Add(this.lvwProductores);
            this.Controls.Add(this.cboProductores);
            this.Controls.Add(this.lblProductor);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ListaProductoresView";
            this.Text = "Productores";
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.ListView lvwProductores;
        private System.Windows.Forms.Button btnAgregarCarrito;
        private System.Windows.Forms.ComboBox cboProductores;
        private System.Windows.Forms.Label lblProductor;

    }
}
