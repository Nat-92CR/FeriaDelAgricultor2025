using System;
using System.Windows.Forms;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Vista que muestra los detalles de una factura generada
    /// en la Feria del Agricultor.
    /// </summary>
    public partial class FacturaView : Form
    {
        /// <summary>
        /// Factura que se mostrará en la vista.
        /// </summary>
        private readonly Factura factura;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="FacturaView"/>.
        /// </summary>
        /// <param name="factura">Factura que se va a visualizar.</param>
        public FacturaView(Factura factura)
        {
            InitializeComponent();
            this.factura = factura ?? throw new ArgumentNullException(nameof(factura));
            this.LlenarFactura();
        }

        /// <summary>
        /// Llena la interfaz con la información de la factura
        /// (cliente, fecha, método de pago, montos y productos).
        /// </summary>
        private void LlenarFactura()
        {
            // Datos del encabezado
            this.lblClienteNombre.Text =
                $"{this.factura.Cliente.Name} {this.factura.Cliente.LastName}";

            this.lblFecha.Text = this.factura.Fecha.ToString("dd/MM/yyyy HH:mm");
            this.lblMetodoPago.Text = this.factura.MetodoPago.ToString();

            // Cálculos de montos
            var subtotal = this.factura.ObtenerSubtotal();
            var impuesto = this.factura.ObtenerImpuesto();
            var total = this.factura.ObtenerTotal();

            this.lblSubtotal.Text = $"Subtotal: ₡{subtotal:N2}";
            this.lblImpuesto.Text = $"Impuesto ({this.factura.PorcentajeImpuesto}%): ₡{impuesto:N2}";
            this.lblTotal.Text = $"Total: ₡{total:N2}";

            // Configuración del ListView de productos
            this.lvwProductos.Items.Clear();
            this.lvwProductos.Columns.Clear();

            this.lvwProductos.View = View.Details;
            this.lvwProductos.FullRowSelect = true;
            this.lvwProductos.GridLines = true;

            this.lvwProductos.Columns.Add("Producto", 200);
            this.lvwProductos.Columns.Add("Precio", 100);
            this.lvwProductos.Columns.Add("Cantidad", 100);
            this.lvwProductos.Columns.Add("Unidad", 80);
            this.lvwProductos.Columns.Add("Productor", 120);
            this.lvwProductos.Columns.Add("Subtotal", 120);
            this.lvwProductos.Columns.Add("Total línea", 120);
            foreach (var producto in this.factura.Productos)
            {
                var item = new ListViewItem(producto.NombreProducto);

                // Total SIN impuesto
                var totalLinea = producto.Precio * producto.Cantidad;

                item.SubItems.Add($"₡{producto.Precio:N0}");
                item.SubItems.Add(producto.Cantidad.ToString());
                item.SubItems.Add(producto.UnidadMedida.ToString());
                item.SubItems.Add(producto.Productor);

                // Subtotal sin impuesto (igual a totalLinea)
                item.SubItems.Add($"₡{totalLinea:N0}");

                // Total de la línea (sin impuestos)
                item.SubItems.Add($"₡{totalLinea:N0}");

                this.lvwProductos.Items.Add(item);
            }
        }

        /// <summary>
        /// Cierra la ventana de factura cuando se presiona el botón Cerrar.
        /// </summary>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
