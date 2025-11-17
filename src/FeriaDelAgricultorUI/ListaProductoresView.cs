using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Vista que muestra la lista de productores y los productos que vende cada uno.
    /// </summary>
    public partial class ListaProductoresView : Form
    {
        private readonly ProductoService productoService;
        private readonly CarritoService carritoService;   // 👈 campo para el carrito

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ListaProductoresView"/>.
        /// </summary>
        /// <param name="productoService">Servicio de productos.</param>
        /// <param name="carritoService">Servicio de carrito compartido.</param>
        public ListaProductoresView(ProductoService productoService, CarritoService carritoService)
        {
            this.productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            this.carritoService = carritoService ?? throw new ArgumentNullException(nameof(carritoService));

            InitializeComponent();
            ConfigurarColumnas();
            CargarProductoresEnCombo();
            CargarListaCompleta();
        }


        private void ConfigurarColumnas()
        {
            lvwProductores.Columns.Clear();

            lvwProductores.Columns.Add("Productor", 150);
            lvwProductores.Columns.Add("Producto", 150);
            lvwProductores.Columns.Add("Precio", 80);
            lvwProductores.Columns.Add("Cantidad", 80);
            lvwProductores.Columns.Add("Unidad", 80);
        }

        /// <summary>
        /// Carga todos los productores y sus productos en el ListView.
        /// </summary>
        private void CargarListaCompleta(string productorFiltro = null)
        {
            lvwProductores.Items.Clear();

            List<Producto> productos = this.productoService.ObtenerTodos();

            foreach (var producto in productos)
            {
                if (!string.IsNullOrEmpty(productorFiltro) &&
                    !string.Equals(producto.Productor, productorFiltro, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // saltamos productos de otros productores
                }

                var item = new ListViewItem(producto.Productor);
                item.SubItems.Add(producto.NombreProducto);
                item.SubItems.Add(producto.Precio.ToString("₡0.00"));
                item.SubItems.Add(producto.Cantidad.ToString());
                item.SubItems.Add(producto.UnidadMedida.ToString());

                lvwProductores.Items.Add(item);
            }
        }

        private void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            // 1. Validar que haya un producto seleccionado
            if (lvwProductores.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecciona un producto primero.",
                    "Carrito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var item = lvwProductores.SelectedItems[0];

            // Columnas: 0 = Productor, 1 = Producto, 2 = Precio, 3 = Cantidad, 4 = Unidad
            string productor = item.SubItems[0].Text;
            string nombreProducto = item.SubItems[1].Text;
            string precioTexto = item.SubItems[2].Text.Replace("₡", "").Trim();

            if (!decimal.TryParse(precioTexto, out decimal precio))
            {
                MessageBox.Show("No se pudo leer el precio del producto.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int cantidad = 1;

            var producto = new Producto
            {
                NombreProducto = nombreProducto,
                Precio = precio,
                Productor = productor,
                Cantidad = cantidad,
                UnidadMedida = UnidadMedida.Unidades
            };

            carritoService.AgregarProducto(producto, cantidad);

            MessageBox.Show($"{nombreProducto} se agregó al carrito.",
                "Carrito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CargarProductoresEnCombo()
        {
            cboProductores.Items.Clear();
            cboProductores.Items.Add("Todos los productores");

            var productos = this.productoService.ObtenerTodos();
            var productoresUnicos = new HashSet<string>();

            foreach (var p in productos)
            {
                if (!string.IsNullOrWhiteSpace(p.Productor))
                {
                    productoresUnicos.Add(p.Productor);
                }
            }

            foreach (var nombreProd in productoresUnicos)
            {
                cboProductores.Items.Add(nombreProd);
            }

            cboProductores.SelectedIndex = 0;
        }

        private void cboProductores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductores.SelectedIndex <= 0)
            {
                // 0 = "Todos los productores"
                CargarListaCompleta();
            }
            else
            {
                string productorSeleccionado = cboProductores.SelectedItem.ToString();
                CargarListaCompleta(productorSeleccionado);
            }
        }
    }
}
