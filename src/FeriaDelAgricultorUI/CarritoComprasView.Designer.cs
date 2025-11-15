using System.Drawing;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    partial class CarritoComprasView
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
            this.lvwCarrito = new System.Windows.Forms.ListView();
            this.btnFinalizarCompra = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(103, 15);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Carrito de compras";
            // 
            // lvwCarrito
            // 
            this.lvwCarrito.FullRowSelect = true;
            this.lvwCarrito.HideSelection = false;
            this.lvwCarrito.Location = new System.Drawing.Point(12, 35);
            this.lvwCarrito.Name = "lvwCarrito";
            this.lvwCarrito.Size = new System.Drawing.Size(600, 260);
            this.lvwCarrito.TabIndex = 1;
            this.lvwCarrito.UseCompatibleStateImageBehavior = false;
            this.lvwCarrito.View = System.Windows.Forms.View.Details;
            // 
            // btnFinalizarCompra
            // 
            this.btnFinalizarCompra.Location = new System.Drawing.Point(12, 305);
            this.btnFinalizarCompra.Name = "btnFinalizarCompra";
            this.btnFinalizarCompra.Size = new System.Drawing.Size(150, 30);
            this.btnFinalizarCompra.TabIndex = 2;
            this.btnFinalizarCompra.Text = "Finalizar compra";
            this.btnFinalizarCompra.UseVisualStyleBackColor = true;
            // 
            // CarritoComprasView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.btnFinalizarCompra);
            this.Controls.Add(this.lvwCarrito);
            this.Controls.Add(this.lblTitulo);
            this.Name = "CarritoComprasView";
            this.Text = "Carrito de compras";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private ListView lvwCarrito;
        private Button btnFinalizarCompra;
    }
}
