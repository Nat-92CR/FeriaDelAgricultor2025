using System.Drawing;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    partial class MainMenuView
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpia los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.btnFactura = new System.Windows.Forms.Button();
            this.btnListaProductores = new System.Windows.Forms.Button();
            this.btnCarrito = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Location = new System.Drawing.Point(35, 20);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(66, 15);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido";
            // 
            // btnFactura
            // 
            this.btnFactura.Location = new System.Drawing.Point(35, 60);
            this.btnFactura.Name = "btnFactura";
            this.btnFactura.Size = new System.Drawing.Size(129, 33);
            this.btnFactura.TabIndex = 1;
            this.btnFactura.Text = "Ver factura ejemplo";
            this.btnFactura.UseVisualStyleBackColor = true;
            this.btnFactura.Click += new System.EventHandler(this.btnFactura_Click);
            // 
            // btnListaProductores
            // 
            this.btnListaProductores.Location = new System.Drawing.Point(35, 115);
            this.btnListaProductores.Name = "btnListaProductores";
            this.btnListaProductores.Size = new System.Drawing.Size(180, 33);
            this.btnListaProductores.TabIndex = 2;
            this.btnListaProductores.Text = "Lista de productores";
            this.btnListaProductores.UseVisualStyleBackColor = true;
            this.btnListaProductores.Click += new System.EventHandler(this.btnListaProductores_Click);
            // 
            // btnCarrito
            // 
            this.btnCarrito.Location = new System.Drawing.Point(35, 160);
            this.btnCarrito.Name = "btnCarrito";
            this.btnCarrito.Size = new System.Drawing.Size(180, 33);
            this.btnCarrito.TabIndex = 3;
            this.btnCarrito.Text = "Carrito de compras";
            this.btnCarrito.UseVisualStyleBackColor = true;
            this.btnCarrito.Click += new System.EventHandler(this.btnCarrito_Click);
            // 
            // btnReportes
            // 
            this.btnReportes.Location = new System.Drawing.Point(35, 205);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(180, 33);
            this.btnReportes.TabIndex = 4;
            this.btnReportes.Text = "Reportes / estadísticas";
            this.btnReportes.UseVisualStyleBackColor = true;
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            // 
            // MainMenuView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnReportes);
            this.Controls.Add(this.btnCarrito);
            this.Controls.Add(this.btnListaProductores);
            this.Controls.Add(this.btnFactura);
            this.Controls.Add(this.lblBienvenida);
            this.IsMdiContainer = true;
            this.Name = "MainMenuView";
            this.Text = "Menú principal - Feria del Agricultor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
        private Button btnFactura;
        private Button btnListaProductores;
        private Button btnCarrito;
        private Button btnReportes;
    }
}
