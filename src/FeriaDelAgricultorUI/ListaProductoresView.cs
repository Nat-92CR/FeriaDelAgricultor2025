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

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ListaProductoresView"/>.
        /// </summary>
        /// <param name="productoService">Servicio de productos.</param>
        public ListaProductoresView(ProductoService productoService)
        {
            this.productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));

            InitializeComponent();
            ConfigurarColumnas();
            CargarListaCompleta();
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
        /// Muestra la ventana con la lista de productores y sus productos.
        /// </summary>
        private void btnListaProductores_Click(object sender, EventArgs e)
        {
            // Pasamos el servicio de productos al formulario
            var listaView = new ListaProductoresView(this.productoService);
            listaView.MdiParent = this;
            listaView.Show();
        }

    }
}
