using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    partial class FacturaView
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpia los recursos que se estén utilizando.
        /// </summary>
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
            lblClienteNombre = new Label();
            lblFecha = new Label();
            lblMetodoPago = new Label();
            lblTotal = new Label();
            lvwProductos = new ListView();
            SuspendLayout();
            // 
            // lblClienteNombre
            // 
            lblClienteNombre.AutoSize = true;
            lblClienteNombre.Location = new Point(12, 9);
            lblClienteNombre.Name = "lblClienteNombre";
            lblClienteNombre.Size = new Size(47, 15);
            lblClienteNombre.TabIndex = 0;
            lblClienteNombre.Text = "Cliente:";
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(12, 30);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(41, 15);
            lblFecha.TabIndex = 1;
            lblFecha.Text = "Fecha:";
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Location = new Point(12, 51);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(98, 15);
            lblMetodoPago.TabIndex = 2;
            lblMetodoPago.Text = "Método de pago:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(12, 72);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(35, 15);
            lblTotal.TabIndex = 3;
            lblTotal.Text = "Total:";
            // 
            // lvwProductos
            // 
            lvwProductos.FullRowSelect = true;
            lvwProductos.Location = new Point(12, 100);
            lvwProductos.Name = "lvwProductos";
            lvwProductos.Size = new Size(560, 250);
            lvwProductos.TabIndex = 4;
            lvwProductos.UseCompatibleStateImageBehavior = false;
            lvwProductos.View = View.Details;
            // 
            // FacturaView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(lvwProductos);
            Controls.Add(lblTotal);
            Controls.Add(lblMetodoPago);
            Controls.Add(lblFecha);
            Controls.Add(lblClienteNombre);
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "FacturaView";
            Text = "Factura";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblClienteNombre;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblMetodoPago;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ListView lvwProductos;
    }
}
