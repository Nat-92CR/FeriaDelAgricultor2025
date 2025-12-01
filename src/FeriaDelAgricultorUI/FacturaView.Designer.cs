using System.Drawing;
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
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">
        /// true si los recursos administrados se deben desechar; false en caso contrario.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblClienteTitulo = new Label();
            lblClienteNombre = new Label();
            lblFechaTitulo = new Label();
            lblFecha = new Label();
            lblMetodoPagoTitulo = new Label();
            lblMetodoPago = new Label();
            lvwProductos = new ListView();
            lblSubtotal = new Label();
            lblImpuesto = new Label();
            lblTotal = new Label();
            btnCerrar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(25, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(177, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Factura de compra";
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Location = new Point(25, 55);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(47, 15);
            lblClienteTitulo.TabIndex = 1;
            lblClienteTitulo.Text = "Cliente:";
            // 
            // lblClienteNombre
            // 
            lblClienteNombre.AutoSize = true;
            lblClienteNombre.Location = new Point(90, 55);
            lblClienteNombre.Name = "lblClienteNombre";
            lblClienteNombre.Size = new Size(107, 15);
            lblClienteNombre.TabIndex = 2;
            lblClienteNombre.Text = "<Nombre Cliente>";
            // 
            // lblFechaTitulo
            // 
            lblFechaTitulo.AutoSize = true;
            lblFechaTitulo.Location = new Point(25, 80);
            lblFechaTitulo.Name = "lblFechaTitulo";
            lblFechaTitulo.Size = new Size(41, 15);
            lblFechaTitulo.TabIndex = 3;
            lblFechaTitulo.Text = "Fecha:";
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(90, 80);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(93, 15);
            lblFecha.TabIndex = 4;
            lblFecha.Text = "<dd/MM/yyyy>";
            // 
            // lblMetodoPagoTitulo
            // 
            lblMetodoPagoTitulo.AutoSize = true;
            lblMetodoPagoTitulo.Location = new Point(25, 105);
            lblMetodoPagoTitulo.Name = "lblMetodoPagoTitulo";
            lblMetodoPagoTitulo.Size = new Size(98, 15);
            lblMetodoPagoTitulo.TabIndex = 5;
            lblMetodoPagoTitulo.Text = "Método de pago:";
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Location = new Point(130, 105);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(92, 15);
            lblMetodoPago.TabIndex = 6;
            lblMetodoPago.Text = "<MetodoPago>";
            // 
            // lvwProductos
            // 
            lvwProductos.Location = new Point(25, 140);
            lvwProductos.Name = "lvwProductos";
            lvwProductos.Size = new Size(834, 220);
            lvwProductos.TabIndex = 7;
            lvwProductos.UseCompatibleStateImageBehavior = false;
            // 
            // lblSubtotal
            // 
            lblSubtotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(584, 400);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(69, 15);
            lblSubtotal.TabIndex = 8;
            lblSubtotal.Text = "Subtotal: ₡0";
            // 
            // lblImpuesto
            // 
            lblImpuesto.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblImpuesto.AutoSize = true;
            lblImpuesto.Location = new Point(584, 425);
            lblImpuesto.Name = "lblImpuesto";
            lblImpuesto.Size = new Size(75, 15);
            lblImpuesto.TabIndex = 9;
            lblImpuesto.Text = "Impuesto: ₡0";
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(584, 450);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(86, 19);
            lblTotal.TabIndex = 10;
            lblTotal.Text = "Total: ₡0.00";
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCerrar.Location = new Point(25, 440);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.TabIndex = 11;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FacturaView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(904, 485);
            Controls.Add(btnCerrar);
            Controls.Add(lblTotal);
            Controls.Add(lblImpuesto);
            Controls.Add(lblSubtotal);
            Controls.Add(lvwProductos);
            Controls.Add(lblMetodoPago);
            Controls.Add(lblMetodoPagoTitulo);
            Controls.Add(lblFecha);
            Controls.Add(lblFechaTitulo);
            Controls.Add(lblClienteNombre);
            Controls.Add(lblClienteTitulo);
            Controls.Add(lblTitulo);
            Name = "FacturaView";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Factura - Feria del Agricultor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblClienteTitulo;
        private Label lblClienteNombre;
        private Label lblFechaTitulo;
        private Label lblFecha;
        private Label lblMetodoPagoTitulo;
        private Label lblMetodoPago;
        private ListView lvwProductos;
        private Label lblSubtotal;
        private Label lblImpuesto;
        private Label lblTotal;
        private Button btnCerrar;
    }
}
