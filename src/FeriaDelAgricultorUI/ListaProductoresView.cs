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
        private readonly CarritoService carritoService;

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
            CargarProductoresEnCombo();   // ← llena el combo
            CargarListaCompleta();        // ← lista todos los productos
        }

        /// <summary>
        /// Configura las columnas del ListView.
        /// </summary>
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
        private void CargarListaCompleta()
        {
            lvwProductores.Items.Clear();

            List<Producto> productos = this.productoService.ObtenerTodos();

            foreach (var producto in productos)
            {
                var item = new ListViewItem(producto.Productor);
                item.SubItems.Add(producto.NombreProducto);
                item.SubItems.Add(producto.Precio.ToString("₡0.00"));
                item.SubItems.Add(producto.Cantidad.ToString());
                item.SubItems.Add(producto.UnidadMedida.ToString());

                lvwProductores.Items.Add(item);
            }
        }

        /// <summary>
        /// Cargar productores en el ComboBox.
        /// </summary>
        private void CargarProductoresEnCombo()
        {
            cbxProductores.Items.Clear();

            cbxProductores.Items.Add("Todos los productores");

            var productores = productoService.ObtenerProductores();

            foreach (var p in productores)
            {
                cbxProductores.Items.Add(p);
            }

            cbxProductores.SelectedIndex = 0;
        }

        /// <summary>
        /// Evento: cuando se selecciona un productor en el combo.
        /// </summary>
        private void cbxProductores_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cbxProductores.SelectedItem.ToString();

            if (seleccionado == "Todos los productores")
            {
                CargarListaCompleta();
                return;
            }

            CargarPorProductor(seleccionado);
        }

        /// <summary>
        /// Cargar productos filtrados por productor.
        /// </summary>
        private void CargarPorProductor(string productor)
        {
            lvwProductores.Items.Clear();

            List<Producto> productos = this.productoService.ObtenerPorProductor(productor);

            foreach (var producto in productos)
            {
                var item = new ListViewItem(producto.Productor);
                item.SubItems.Add(producto.NombreProducto);
                item.SubItems.Add(producto.Precio.ToString("₡0.00"));
                item.SubItems.Add(producto.Cantidad.ToString());
                item.SubItems.Add(producto.UnidadMedida.ToString());
                lvwProductores.Items.Add(item);
            }
        }

        /// <summary>
        /// Agregar producto al carrito (usa el carrito compartido).
        /// </summary>
        private void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            if (lvwProductores.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccione un producto primero.");
                return;
            }

            var item = lvwProductores.SelectedItems[0];

            string productor = item.SubItems[0].Text;
            string nombreProducto = item.SubItems[1].Text;

            var producto = productoService.ObtenerProducto(productor, nombreProducto);

            if (producto == null)
            {
                MessageBox.Show("Error cargando producto.");
                return;
            }

            // 🔹 Aquí SÍ usamos el carrito compartido
            carritoService.AgregarProducto(producto);

            MessageBox.Show("Producto agregado al carrito.");
        }
    }
}
