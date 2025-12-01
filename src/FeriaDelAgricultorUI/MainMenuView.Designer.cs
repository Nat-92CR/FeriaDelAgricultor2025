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
            lblBienvenida = new Label();
            btnFactura = new Button();
            btnListaProductores = new Button();
            btnCarrito = new Button();
            btnReportes = new Button();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Location = new Point(35, 20);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(66, 15);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido";
            // 
            // btnFactura
            // 
            btnFactura.Location = new Point(35, 213);
            btnFactura.Name = "btnFactura";
            btnFactura.Size = new Size(180, 33);
            btnFactura.TabIndex = 5;
            btnFactura.Text = "Ver última factura";
            btnFactura.Click += btnFactura_Click;
            // 
            // btnListaProductores
            // 
            btnListaProductores.Location = new Point(35, 64);
            btnListaProductores.Name = "btnListaProductores";
            btnListaProductores.Size = new Size(180, 33);
            btnListaProductores.TabIndex = 2;
            btnListaProductores.Text = "Lista de productores";
            btnListaProductores.UseVisualStyleBackColor = true;
            btnListaProductores.Click += btnListaProductores_Click;
            // 
            // btnCarrito
            // 
            btnCarrito.Location = new Point(35, 113);
            btnCarrito.Name = "btnCarrito";
            btnCarrito.Size = new Size(180, 33);
            btnCarrito.TabIndex = 3;
            btnCarrito.Text = "Carrito de compras";
            btnCarrito.UseVisualStyleBackColor = true;
            btnCarrito.Click += btnCarrito_Click;
            // 
            // btnReportes
            // 
            btnReportes.Location = new Point(35, 165);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(180, 33);
            btnReportes.TabIndex = 4;
            btnReportes.Text = "Reportes / estadísticas";
            btnReportes.UseVisualStyleBackColor = true;
            btnReportes.Click += btnReportes_Click;
            // 
            // MainMenuView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(800, 450);
            Controls.Add(btnReportes);
            Controls.Add(btnCarrito);
            Controls.Add(btnListaProductores);
            Controls.Add(btnFactura);
            Controls.Add(lblBienvenida);
            IsMdiContainer = true;
            Name = "MainMenuView";
            Text = "Menú principal - Feria del Agricultor";
            Load += btnFactura_Click;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
        private Button btnFactura;
        private Button btnListaProductores;
        private Button btnCarrito;
        private Button btnReportes;
    }
}
