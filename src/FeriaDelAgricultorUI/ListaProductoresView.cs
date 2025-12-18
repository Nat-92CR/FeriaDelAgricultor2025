using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Muestra la lista de productores (puestos) disponibles en el punto de feria seleccionado,
    /// permite filtrar por puesto y agregar productos al carrito.
    /// </summary>
    public partial class ListaProductoresView : Form
    {
        private readonly ProductoService productoService;
        private readonly CarritoService carritoService;
        private readonly string provinciaSeleccionada;
        private readonly PuntoFeria puntoFeriaSeleccionada;

        /// <summary>
        /// Servicio que lee Productores.csv.
        /// </summary>
        private readonly ProductorService productorService;

        /// <summary>
        /// Productores/puestos que pertenecen a este punto de feria.
        /// </summary>
        private readonly List<Productor> productoresDeFeria;

        /// <summary>
        /// Constructor de la vista.
        /// </summary>
        /// <param name="productoService">Servicio de productos (lee Productos.csv).</param>
        /// <param name="carritoService">Carrito compartido.</param>
        /// <param name="provinciaSeleccionada">Provincia elegida.</param>
        /// <param name="puntoFeriaSeleccionada">Feria elegida.</param>
        /// <param name="productor">Opcional, puede ser null (no es necesario en este flujo).</param>
        public ListaProductoresView(
            ProductoService productoService,
            CarritoService carritoService,
            string provinciaSeleccionada,
            PuntoFeria puntoFeriaSeleccionada,
            Productor productor)
        {
            InitializeComponent();

            this.productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            this.carritoService = carritoService ?? throw new ArgumentNullException(nameof(carritoService));
            this.provinciaSeleccionada = provinciaSeleccionada ?? throw new ArgumentNullException(nameof(provinciaSeleccionada));
            this.puntoFeriaSeleccionada = puntoFeriaSeleccionada ?? throw new ArgumentNullException(nameof(puntoFeriaSeleccionada));

            // Cargar productores reales desde CSV
            this.productorService = new ProductorService();

            // Filtrar productores por punto de feria (Id del punto)
            this.productoresDeFeria = productorService.ObtenerPorPuntoFeria(puntoFeriaSeleccionada.Id);

            // UI
            MostrarFeriaSeleccionada();
            ConfigurarColumnas();

            // Cargar combo
            CargarProductoresEnCombo(productor);

            // Cargar lista inicial
            CargarListaCompleta();
        }

        /// <summary>
        /// Muestra en pantalla el punto de feria seleccionado.
        /// </summary>
        private void MostrarFeriaSeleccionada()
        {
            lblFeriaSeleccionada.Text =
                $"Feria seleccionada: {puntoFeriaSeleccionada.Nombre} - {puntoFeriaSeleccionada.Canton}, {puntoFeriaSeleccionada.Provincia}";
        }

        /// <summary>
        /// Configura las columnas del ListView.
        /// </summary>
        private void ConfigurarColumnas()
        {
            lvwProductores.View = View.Details;
            lvwProductores.FullRowSelect = true;
            lvwProductores.GridLines = true;

            lvwProductores.Columns.Clear();
            lvwProductores.Columns.Add("Dueño", 160);
            lvwProductores.Columns.Add("Puesto", 170);
            lvwProductores.Columns.Add("Producto", 170);
            lvwProductores.Columns.Add("Precio", 90);
            lvwProductores.Columns.Add("Cantidad", 90);
            lvwProductores.Columns.Add("Unidad", 90);
        }

        /// <summary>
        /// Carga el ComboBox con los puestos (NombrePuesto) disponibles en esta feria.
        /// </summary>
        private void CargarProductoresEnCombo(Productor productor)
        {
            cbxProductores.Items.Clear();
            cbxProductores.Items.Add("Todos los productores");

            if (productoresDeFeria == null || productoresDeFeria.Count == 0)
            {
                cbxProductores.SelectedIndex = 0;
                MessageBox.Show(
                    "No se encontraron productores asociados a este punto de feria.\n" +
                    "Revise Productores.csv (columna PuntoFeriaId).",
                    "Sin productores",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Agregar puestos reales
            foreach (var p in productoresDeFeria)
            {
                cbxProductores.Items.Add(p.NombrePuesto);
            }

            cbxProductores.SelectedIndex = 0;

            // Si llega un productor (opcional), seleccionarlo
            if (productor != null && !string.IsNullOrWhiteSpace(productor.NombrePuesto))
            {
                int idx = cbxProductores.FindStringExact(productor.NombrePuesto);
                if (idx >= 0) cbxProductores.SelectedIndex = idx;
            }
        }

        /// <summary>
        /// Carga todos los productos de todos los puestos de la feria.
        /// </summary>
        private void CargarListaCompleta()
        {
            lvwProductores.Items.Clear();

            if (productoresDeFeria == null || productoresDeFeria.Count == 0)
                return;

            // Productos (desde Productos.csv)
            List<Producto> productos = productoService.ObtenerTodos();

            // Si no hay productos, avisar claramente (esto es lo que te estaba pasando)
            if (productos == null || productos.Count == 0)
            {
                MessageBox.Show(
                    "No se encontraron productos.\n" +
                    "Verifique que exista Data\\Productos.csv y que se copie a la carpeta bin\\...\\Data.",
                    "Sin productos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            foreach (var prodFeria in productoresDeFeria)
            {
                // Match por NombrePuesto (de Productores.csv) contra Productor (columna 1 de Productos.csv)
                var productosDelPuesto = productos
                    .Where(p => !string.IsNullOrWhiteSpace(p.Productor) &&
                                p.Productor.Equals(prodFeria.NombrePuesto, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var prod in productosDelPuesto)
                {
                    var item = new ListViewItem(prodFeria.Dueno);
                    item.SubItems.Add(prodFeria.NombrePuesto);
                    item.SubItems.Add(prod.NombreProducto);
                    item.SubItems.Add(prod.Precio.ToString("₡0"));
                    item.SubItems.Add(prod.Cantidad.ToString());
                    item.SubItems.Add(prod.UnidadMedida.ToString());

                    lvwProductores.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Evento: filtra la lista cuando cambia el puesto seleccionado.
        /// </summary>
        private void cbxProductores_SelectedIndexChanged(object sender, EventArgs e)
        {
            // "Todos"
            if (cbxProductores.SelectedIndex <= 0)
            {
                CargarListaCompleta();
                return;
            }

            string puestoSeleccionado = cbxProductores.SelectedItem?.ToString() ?? string.Empty;

            var productor = productoresDeFeria
                .FirstOrDefault(p => p.NombrePuesto.Equals(puestoSeleccionado, StringComparison.OrdinalIgnoreCase));

            if (productor != null)
                CargarPorProductor(productor);
        }

        /// <summary>
        /// Carga en el ListView únicamente los productos del puesto indicado.
        /// </summary>
        private void CargarPorProductor(Productor productor)
        {
            lvwProductores.Items.Clear();

            var productos = productoService.ObtenerTodos()
                .Where(p => !string.IsNullOrWhiteSpace(p.Productor) &&
                            p.Productor.Equals(productor.NombrePuesto, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var prod in productos)
            {
                var item = new ListViewItem(productor.Dueno);
                item.SubItems.Add(productor.NombrePuesto);
                item.SubItems.Add(prod.NombreProducto);
                item.SubItems.Add(prod.Precio.ToString("₡0"));
                item.SubItems.Add(prod.Cantidad.ToString());
                item.SubItems.Add(prod.UnidadMedida.ToString());

                lvwProductores.Items.Add(item);
            }
        }

        /// <summary>
        /// Agrega al carrito el producto seleccionado del ListView.
        /// </summary>
        private void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            if (lvwProductores.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            var item = lvwProductores.SelectedItems[0];

            string puesto = item.SubItems[1].Text;
            string productoNombre = item.SubItems[2].Text;

            var producto = productoService.ObtenerProducto(puesto, productoNombre);

            if (producto == null)
            {
                MessageBox.Show("No se pudo cargar el producto seleccionado.");
                return;
            }

            // Agrega una unidad (o la cantidad definida en tu modelo actual).
            carritoService.AgregarProducto(producto);

            MessageBox.Show("Producto agregado al carrito.");
        }
    }
}
