namespace FeriaDelAgricultorUI
{
    partial class CarritoComprasView
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos usados.
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
        /// Inicialización del diseñador.
        /// </summary>
        private void InitializeComponent()
        {
            dgvCarrito = new DataGridView();
            lblTitulo = new Label();
            lblSubTotalTitulo = new Label();
            lblSubTotal = new Label();
            lblImpuestoTitulo = new Label();
            lblImpuesto = new Label();
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            txtDescuento = new TextBox();
            lblDescuentoTitulo = new Label();
            btnEliminarProducto = new Button();
            btnVaciarCarrito = new Button();
            btnProcesarCompra = new Button();
            cbProvincia = new ComboBox();
            cbCanton = new ComboBox();
            cbPuntoFeria = new ComboBox();
            txtDireccionFeria = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).BeginInit();
            SuspendLayout();
            // 
            // dgvCarrito
            // 
            dgvCarrito.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCarrito.Location = new Point(25, 60);
            dgvCarrito.Name = "dgvCarrito";
            dgvCarrito.Size = new Size(620, 250);
            dgvCarrito.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(182, 25);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Carrito de compras";
            // 
            // lblSubTotalTitulo
            // 
            lblSubTotalTitulo.AutoSize = true;
            lblSubTotalTitulo.Location = new Point(25, 330);
            lblSubTotalTitulo.Name = "lblSubTotalTitulo";
            lblSubTotalTitulo.Size = new Size(54, 15);
            lblSubTotalTitulo.TabIndex = 2;
            lblSubTotalTitulo.Text = "Subtotal:";
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Location = new Point(120, 330);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(34, 15);
            lblSubTotal.TabIndex = 3;
            lblSubTotal.Text = "₡0.00";
            // 
            // lblImpuestoTitulo
            // 
            lblImpuestoTitulo.AutoSize = true;
            lblImpuestoTitulo.Location = new Point(25, 355);
            lblImpuestoTitulo.Name = "lblImpuestoTitulo";
            lblImpuestoTitulo.Size = new Size(60, 15);
            lblImpuestoTitulo.TabIndex = 4;
            lblImpuestoTitulo.Text = "Impuesto:";
            // 
            // lblImpuesto
            // 
            lblImpuesto.AutoSize = true;
            lblImpuesto.Location = new Point(120, 355);
            lblImpuesto.Name = "lblImpuesto";
            lblImpuesto.Size = new Size(34, 15);
            lblImpuesto.TabIndex = 5;
            lblImpuesto.Text = "₡0.00";
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalTitulo.Location = new Point(25, 385);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.Size = new Size(46, 19);
            lblTotalTitulo.TabIndex = 6;
            lblTotalTitulo.Text = "Total:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(120, 385);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(45, 19);
            lblTotal.TabIndex = 7;
            lblTotal.Text = "₡0.00";
            // 
            // txtDescuento
            // 
            txtDescuento.Location = new Point(533, 340);
            txtDescuento.Name = "txtDescuento";
            txtDescuento.Size = new Size(99, 23);
            txtDescuento.TabIndex = 8;
            txtDescuento.TextChanged += txtDescuento_TextChanged;
            // 
            // lblDescuentoTitulo
            // 
            lblDescuentoTitulo.AutoSize = true;
            lblDescuentoTitulo.Location = new Point(450, 348);
            lblDescuentoTitulo.Name = "lblDescuentoTitulo";
            lblDescuentoTitulo.Size = new Size(66, 15);
            lblDescuentoTitulo.TabIndex = 9;
            lblDescuentoTitulo.Text = "Descuento:";
            // 
            // btnEliminarProducto
            // 
            btnEliminarProducto.Location = new Point(660, 60);
            btnEliminarProducto.Name = "btnEliminarProducto";
            btnEliminarProducto.Size = new Size(125, 35);
            btnEliminarProducto.TabIndex = 10;
            btnEliminarProducto.Text = "Eliminar producto";
            btnEliminarProducto.UseVisualStyleBackColor = true;
            btnEliminarProducto.Click += btnEliminarProducto_Click;
            // 
            // btnVaciarCarrito
            // 
            btnVaciarCarrito.Location = new Point(660, 110);
            btnVaciarCarrito.Name = "btnVaciarCarrito";
            btnVaciarCarrito.Size = new Size(125, 35);
            btnVaciarCarrito.TabIndex = 11;
            btnVaciarCarrito.Text = "Vaciar carrito";
            btnVaciarCarrito.UseVisualStyleBackColor = true;
            btnVaciarCarrito.Click += btnVaciarCarrito_Click;
            // 
            // btnProcesarCompra
            // 
            btnProcesarCompra.Location = new Point(660, 511);
            btnProcesarCompra.Name = "btnProcesarCompra";
            btnProcesarCompra.Size = new Size(125, 40);
            btnProcesarCompra.TabIndex = 12;
            btnProcesarCompra.Text = "Procesar compra";
            btnProcesarCompra.UseVisualStyleBackColor = true;
            btnProcesarCompra.Click += btnProcesarCompra_Click;
            // 
            // cbProvincia
            // 
            cbProvincia.FormattingEnabled = true;
            cbProvincia.Location = new Point(25, 476);
            cbProvincia.Name = "cbProvincia";
            cbProvincia.Size = new Size(129, 23);
            cbProvincia.TabIndex = 13;
            // 
            // cbCanton
            // 
            cbCanton.FormattingEnabled = true;
            cbCanton.Location = new Point(188, 476);
            cbCanton.Name = "cbCanton";
            cbCanton.Size = new Size(129, 23);
            cbCanton.TabIndex = 14;
            // 
            // cbPuntoFeria
            // 
            cbPuntoFeria.FormattingEnabled = true;
            cbPuntoFeria.Location = new Point(351, 476);
            cbPuntoFeria.Name = "cbPuntoFeria";
            cbPuntoFeria.Size = new Size(130, 23);
            cbPuntoFeria.TabIndex = 15;
            // 
            // txtDireccionFeria
            // 
            txtDireccionFeria.Location = new Point(25, 528);
            txtDireccionFeria.Name = "txtDireccionFeria";
            txtDireccionFeria.ReadOnly = true;
            txtDireccionFeria.Size = new Size(456, 23);
            txtDireccionFeria.TabIndex = 16;
            // 
            // CarritoComprasView
            // 
            ClientSize = new Size(797, 571);
            Controls.Add(txtDireccionFeria);
            Controls.Add(cbPuntoFeria);
            Controls.Add(cbCanton);
            Controls.Add(cbProvincia);
            Controls.Add(btnProcesarCompra);
            Controls.Add(btnVaciarCarrito);
            Controls.Add(btnEliminarProducto);
            Controls.Add(lblDescuentoTitulo);
            Controls.Add(txtDescuento);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalTitulo);
            Controls.Add(lblImpuesto);
            Controls.Add(lblImpuestoTitulo);
            Controls.Add(lblSubTotal);
            Controls.Add(lblSubTotalTitulo);
            Controls.Add(lblTitulo);
            Controls.Add(dgvCarrito);
            Name = "CarritoComprasView";
            Text = "Carrito de compras";
            Load += CarritoComprasView_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCarrito;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubTotalTitulo;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblImpuestoTitulo;
        private System.Windows.Forms.Label lblImpuesto;
        private System.Windows.Forms.Label lblTotalTitulo;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtDescuento;
        private System.Windows.Forms.Label lblDescuentoTitulo;
        private System.Windows.Forms.Button btnEliminarProducto;
        private System.Windows.Forms.Button btnVaciarCarrito;
        private System.Windows.Forms.Button btnProcesarCompra;
        private ComboBox cbProvincia;
        private ComboBox cbCanton;
        private ComboBox cbPuntoFeria;
        private TextBox txtDireccionFeria;
    }
}
