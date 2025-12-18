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
            lblTitulo = new Label();
            lvwProductores = new ListView();
            cbxProductores = new ComboBox();
            btnAgregarCarrito = new Button();
            lblFeriaSeleccionada = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(12, 48);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(200, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Lista de productores y sus productos";
            // 
            // lvwProductores
            // 
            lvwProductores.FullRowSelect = true;
            lvwProductores.Location = new Point(12, 95);
            lvwProductores.Name = "lvwProductores";
            lvwProductores.Size = new Size(634, 352);
            lvwProductores.TabIndex = 1;
            lvwProductores.UseCompatibleStateImageBehavior = false;
            lvwProductores.View = View.Details;
            // 
            // cbxProductores
            // 
            cbxProductores.FormattingEnabled = true;
            cbxProductores.Location = new Point(12, 66);
            cbxProductores.Name = "cbxProductores";
            cbxProductores.Size = new Size(323, 23);
            cbxProductores.TabIndex = 2;
            cbxProductores.SelectedIndexChanged += cbxProductores_SelectedIndexChanged;
            // 
            // btnAgregarCarrito
            // 
            btnAgregarCarrito.Location = new Point(12, 465);
            btnAgregarCarrito.Name = "btnAgregarCarrito";
            btnAgregarCarrito.Size = new Size(634, 36);
            btnAgregarCarrito.TabIndex = 3;
            btnAgregarCarrito.Text = "Agregar al Carrito";
            btnAgregarCarrito.UseVisualStyleBackColor = true;
            btnAgregarCarrito.Click += btnAgregarCarrito_Click;
            // 
            // lblFeriaSeleccionada
            // 
            lblFeriaSeleccionada.AutoSize = true;
            lblFeriaSeleccionada.Location = new Point(12, 18);
            lblFeriaSeleccionada.Name = "lblFeriaSeleccionada";
            lblFeriaSeleccionada.Size = new Size(106, 15);
            lblFeriaSeleccionada.TabIndex = 4;
            lblFeriaSeleccionada.Text = "Feria seleccionada:";
            // 
            // ListaProductoresView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(658, 656);
            Controls.Add(lblFeriaSeleccionada);
            Controls.Add(btnAgregarCarrito);
            Controls.Add(cbxProductores);
            Controls.Add(lvwProductores);
            Controls.Add(lblTitulo);
            Name = "ListaProductoresView";
            Text = "Productores";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private ListView lvwProductores;
        private ComboBox cbxProductores;
        private Button btnAgregarCarrito;
        private Label lblFeriaSeleccionada;
    }
}
