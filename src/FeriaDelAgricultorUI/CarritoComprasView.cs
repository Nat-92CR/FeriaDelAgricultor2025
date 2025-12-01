using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Vista encargada de mostrar los productos agregados al carrito,
    /// aplicar descuentos, mostrar totales y procesar la compra real
    /// generando una factura asociada al usuario actual.
    /// </summary>
    public partial class CarritoComprasView : Form
    {
        private readonly CarritoService carritoService;

        /// <summary>
        /// Servicio encargado de generar y guardar facturas en CSV.
        /// </summary>
        private readonly FacturaService facturaService = new FacturaService();

        /// <summary>
        /// Servicio para manejar los puntos de feria (provincia/cantón/feria).
        /// </summary>
        private readonly PuntoFeriaService puntoFeriaService;

        /// <summary>
        /// Punto de feria seleccionado por el usuario.
        /// </summary>
        private PuntoFeria puntoFeriaSeleccionada;

        /// <summary>
        /// Usuario autenticado al que se le generará la factura.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Usuario UsuarioActual { get; set; }

        /// <summary>
        /// Constructor principal del carrito usando el carrito compartido.
        /// </summary>
        public CarritoComprasView(CarritoService carritoService)
        {
            this.carritoService = carritoService ?? throw new ArgumentNullException(nameof(carritoService));

            InitializeComponent();

            // Inicializar el servicio de puntos de feria leyendo el CSV.
            var rutaFeria = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PuntosFeria.csv");
            this.puntoFeriaService = new PuntoFeriaService(rutaFeria);

            // Configurar combos de provincia/cantón/feria.
            ConfigurarCombosUbicacion();
        }

        /// <summary>
        /// Evento Load: carga los productos actuales del carrito.
        /// </summary>
        private void CarritoComprasView_Load(object sender, EventArgs e)
        {
            CargarCarritoEnGrid();
        }

        /// <summary>
        /// Carga los productos del carrito dentro del DataGridView.
        /// </summary>
        private void CargarCarritoEnGrid()
        {
            dgvCarrito.DataSource = null;

            var productos = carritoService.ObtenerProductos();

            var lista = productos.Select(p => new
            {
                Productor = p.Productor,
                Producto = p.NombreProducto,
                Precio = (p.Precio).ToString("₡0"),
                Cantidad = p.Cantidad,
                Subtotal = (p.Precio * p.Cantidad).ToString("₡0"),
                Unidad = p.UnidadMedida.ToString()
            }).ToList();

            dgvCarrito.DataSource = lista;

            ActualizarTotales();
        }

        /// <summary>
        /// Calcula subtotal, descuento, impuesto y total final.
        /// Formatea los valores como moneda entera de Costa Rica.
        /// </summary>
        private void ActualizarTotales()
        {
            decimal subtotal = carritoService.ObtenerTotal();

            decimal descuento = 0;
            if (!string.IsNullOrWhiteSpace(txtDescuento.Text))
            {
                decimal.TryParse(txtDescuento.Text, out descuento);
            }

            if (descuento < 0) descuento = 0;
            if (descuento > subtotal) descuento = subtotal;

            decimal baseGravable = subtotal - descuento;
            decimal impuesto = baseGravable * 0.13m;
            decimal totalFinal = baseGravable + impuesto;

            lblSubTotal.Text = Math.Round(subtotal).ToString("₡0");
            lblImpuesto.Text = Math.Round(impuesto).ToString("₡0");
            lblTotal.Text = Math.Round(totalFinal).ToString("₡0");
        }

        /// <summary>
        /// Configura los combos de Provincia, Cantón y Punto de Feria.
        /// </summary>
        private void ConfigurarCombosUbicacion()
        {
            // Provincias
            var provincias = puntoFeriaService.ObtenerProvincias();
            cbProvincia.DataSource = provincias;
            cbProvincia.SelectedIndex = -1;

            cbCanton.DataSource = null;
            cbPuntoFeria.DataSource = null;
            txtDireccionFeria.Text = string.Empty;
            puntoFeriaSeleccionada = null;

            cbProvincia.SelectedIndexChanged += CbProvincia_SelectedIndexChanged;
            cbCanton.SelectedIndexChanged += CbCanton_SelectedIndexChanged;
            cbPuntoFeria.SelectedIndexChanged += CbPuntoFeria_SelectedIndexChanged;
        }

        private void CbProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProvincia.SelectedItem is string provincia)
            {
                var cantones = puntoFeriaService.ObtenerCantonesPorProvincia(provincia);
                cbCanton.DataSource = cantones;
                cbCanton.SelectedIndex = -1;

                cbPuntoFeria.DataSource = null;
                txtDireccionFeria.Text = string.Empty;
                puntoFeriaSeleccionada = null;
            }
        }

        private void CbCanton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProvincia.SelectedItem is string provincia &&
                cbCanton.SelectedItem is string canton)
            {
                var puntos = puntoFeriaService.ObtenerPuntosPorProvinciaYCanton(provincia, canton);
                cbPuntoFeria.DataSource = puntos;
                cbPuntoFeria.DisplayMember = "Nombre";
                cbPuntoFeria.ValueMember = "Id";
                cbPuntoFeria.SelectedIndex = -1;

                txtDireccionFeria.Text = string.Empty;
                puntoFeriaSeleccionada = null;
            }
        }

        private void CbPuntoFeria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPuntoFeria.SelectedItem is PuntoFeria feria)
            {
                puntoFeriaSeleccionada = feria;
                txtDireccionFeria.Text = feria.DireccionExacta;
            }
        }

        /// <summary>
        /// Elimina el producto seleccionado del carrito.
        /// </summary>
        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            string productor = dgvCarrito.CurrentRow.Cells["Productor"].Value.ToString();
            string producto = dgvCarrito.CurrentRow.Cells["Producto"].Value.ToString();

            carritoService.EliminarProducto(productor, producto);
            CargarCarritoEnGrid();
        }

        /// <summary>
        /// Vacía completamente el carrito de compras.
        /// </summary>
        private void btnVaciarCarrito_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show(
                "¿Desea vaciar el carrito?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                carritoService.VaciarCarrito();
                txtDescuento.Text = string.Empty;
                CargarCarritoEnGrid();
            }
        }

        /// <summary>
        /// Recalcula los totales cuando cambia el descuento.
        /// </summary>
        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            ActualizarTotales();
        }

        // ============================================================
        //                 PROCESAR COMPRA REAL AQUÍ
        // ============================================================

        /// <summary>
        /// Procesa la compra real: genera la factura,
        /// la guarda en CSV y abre la interfaz de factura.
        /// </summary>
        private void btnProcesarCompra_Click(object sender, EventArgs e)
        {
            // 1. Validar usuario
            if (this.UsuarioActual == null)
            {
                MessageBox.Show(
                    "Error: No hay un usuario asociado a la compra.",
                    "Facturación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // 2. Validar selección de feria
            if (puntoFeriaSeleccionada == null)
            {
                MessageBox.Show(
                    "Por favor seleccione la provincia, el cantón y el punto de feria donde desea comprar.",
                    "Ubicación requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 3. Obtener productos del carrito
            var productos = carritoService.ObtenerProductos();
            if (productos == null || productos.Count == 0)
            {
                MessageBox.Show(
                    "El carrito está vacío.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 4. Tomar descuento
            decimal descuento = 0;
            if (!string.IsNullOrWhiteSpace(txtDescuento.Text))
            {
                decimal.TryParse(txtDescuento.Text, out descuento);
            }

            if (descuento < 0) descuento = 0;

            // 5. Dirección y método de pago (puedes ampliarlo después)
            var direccion = new Direccion();
            var metodoPago = MetodoPago.Efectivo;

            // 6. Generar factura real
            Factura factura = facturaService.GenerarFactura(
                this.UsuarioActual,
                direccion,
                metodoPago,
                productos);

            // Aplicar descuento y datos de la feria a la factura
            factura.Descuento = descuento;
            factura.PuntoFeriaId = puntoFeriaSeleccionada.Id;
            factura.PuntoFeriaDescripcion = puntoFeriaSeleccionada.ToString();
            factura.DireccionFeria = puntoFeriaSeleccionada.DireccionExacta;

            // 6.1 Guardar la factura en el archivo CSV (historial)
            facturaService.GuardarFacturaEnCsv(factura);

            // 7. Mostrar factura
            using (var facturaView = new FacturaView(factura))
            {
                facturaView.ShowDialog(this);
            }
        }
    }
}
