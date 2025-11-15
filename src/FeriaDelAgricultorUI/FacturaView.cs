using System;
using System.Windows.Forms;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Vista que muestra los detalles de una factura.
    /// </summary>
    public partial class FacturaView : Form
    {
        private readonly Factura factura;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="FacturaView"/>.
        /// </summary>
        /// <param name="factura">Factura que se mostrará.</param>
        public FacturaView(Factura factura)
        {
            InitializeComponent();
            this.factura = factura;
            this.LlenarFactura();
        }

        /// <summary>
        /// Llena la interfaz con la información de la factura.
        /// </summary>
        private void LlenarFactura()
        {
            this.lblClienteNombre.Text =
                $"{this.factura.Cliente.Name} {this.factura.Cliente.LastName}";

            this.lblFecha.Text = this.factura.Fecha.ToString("dd/MM/yyyy");
            this.lblMetodoPago.Text = this.factura.MetodoPago.ToString();
            this.lblTotal.Text = $"₡{this.factura.ObtenerTotal()}";

            this.lvwProductos.Items.Clear();
            this.lvwProductos.Columns.Clear();

            this.lvwProductos.Columns.Add("Producto", 200);
            this.lvwProductos.Columns.Add("Precio", 100);
            this.lvwProductos.Columns.Add("Cantidad", 100);
            this.lvwProductos.Columns.Add("Productor", 100);

            foreach (var producto in this.factura.Productos)
            {
                var item = new ListViewItem(producto.NombreProducto);
                item.SubItems.Add($"₡{producto.Precio}");
                item.SubItems.Add($"{producto.Cantidad} {producto.UnidadMedida}");
                item.SubItems.Add(producto.Productor);

                this.lvwProductos.Items.Add(item);
            }
        }
    }
}
