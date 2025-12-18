using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    public partial class MainMenuView : Form
    {
        // Usuario que inició sesión
        private readonly Usuario usuario;

        private readonly FacturaService facturaService;
        private readonly ProductoService productoService;

        // Carrito compartido entre vistas
        private readonly CarritoService carritoService;

        private readonly PuntoFeriaService puntoFeriaService;

        /// <summary>
        /// Este constructor recibe el usuario luego del login.
        /// </summary>
        /// <param name="usuario">Usuario autenticado.</param>
        public MainMenuView(Usuario usuario)
        {
            InitializeComponent();

            // Se instancia aquí.
            this.usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            this.facturaService = new FacturaService();
            this.productoService = new ProductoService();
            this.carritoService = new CarritoService();   // carrito único
            this.puntoFeriaService = new PuntoFeriaService();

            ConfigurarMensajeBienvenida();

            // ✅ Al iniciar sesión NO debe poder ver factura automáticamente
            btnFactura.Enabled = false;

            // ✅ Si el Designer o alguien enganchó Load a btnFactura_Click, lo quitamos
            this.Load -= btnFactura_Click;

            // ✅ Cargamos un Load real que NO abre factura: solo habilita/deshabilita el botón
            this.Load += MainMenuView_Load;
        }

        /// <summary>
        /// Evento Load del menú principal.
        /// Aquí NO se abre nada automáticamente.
        /// Solo se actualiza el estado del botón "Ver última factura".
        /// </summary>
        private void MainMenuView_Load(object sender, EventArgs e)
        {
            ActualizarEstadoBotonFactura();
        }

        /// <summary>
        /// Habilita o deshabilita el botón de "Ver última factura" dependiendo
        /// de si existe al menos una factura para el usuario actual.
        /// </summary>
        private void ActualizarEstadoBotonFactura()
        {
            if (usuario == null)
            {
                btnFactura.Enabled = false;
                return;
            }

            const string nombreArchivo = "Facturas.csv";

            if (!File.Exists(nombreArchivo))
            {
                btnFactura.Enabled = false;
                return;
            }

            string usuarioTextoActual = $"{usuario.Name} {usuario.LastName}".Trim();

            bool existeFacturaUsuario = File.ReadAllLines(nombreArchivo)
                .Skip(1)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                .Any(p => p.Length >= 2 && p[1] == usuarioTextoActual);

            btnFactura.Enabled = existeFacturaUsuario;
        }

        /// <summary>
        /// Configura el mensaje de bienvenida según el tipo de usuario.
        /// </summary>
        private void ConfigurarMensajeBienvenida()
        {
            if (usuario == null)
            {
                lblBienvenida.Text = "Bienvenido al sistema Feria del Agricultor.";
                return;
            }

            // Nombre del usuario
            string nombre = $"{usuario.Name} {usuario.LastName}".Trim();

            // Texto según tipo de usuario
            if (usuario.TipoUsuario == TipoUsuario.Cliente)
            {
                lblBienvenida.Text =
                    $"¡Bienvenid@ {nombre}! Has iniciado sesión como CLIENTE.";
                // Color de fondo para clientes
                this.BackColor = Color.LightSkyBlue;
            }
            else if (usuario.TipoUsuario == TipoUsuario.Productor)
            {
                lblBienvenida.Text =
                    $"¡Hola {nombre}! Has iniciado sesión como PRODUCTOR.";
                // Color de fondo para productores
                this.BackColor = Color.Moccasin;
            }
            else
            {
                lblBienvenida.Text = $"Bienvenid@ {nombre} ";
            }
        }

        /// <summary>
        /// Muestra la última factura real registrada en el archivo Facturas.csv.
        /// </summary>
        private void btnFactura_Click(object sender, EventArgs e)
        {
            // Validar usuario
            if (this.usuario == null)
            {
                MessageBox.Show(
                    "No hay un usuario autenticado. Vuelva a iniciar sesión.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                btnFactura.Enabled = false;
                return;
            }

            const string nombreArchivo = "Facturas.csv";

            if (!File.Exists(nombreArchivo))
            {
                MessageBox.Show(
                    "Todavía no hay facturas registradas en el sistema.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                btnFactura.Enabled = false;
                return;
            }

            var lineas = File.ReadAllLines(nombreArchivo)
                             .Skip(1) // omitir encabezado
                             .Where(l => !string.IsNullOrWhiteSpace(l))
                             .ToList();

            if (lineas.Count == 0)
            {
                MessageBox.Show(
                    "Todavía no hay facturas registradas en el sistema.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                btnFactura.Enabled = false;
                return;
            }

            // ✅ Tomar la última factura DEL USUARIO ACTUAL (no del sistema)
            string usuarioTextoActual = $"{usuario.Name} {usuario.LastName}".Trim();

            var ultimaLineaUsuario = lineas
                .Where(l =>
                {
                    var parts = l.Split(';');
                    return parts.Length >= 2 && parts[1] == usuarioTextoActual;
                })
                .LastOrDefault();

            if (ultimaLineaUsuario == null)
            {
                MessageBox.Show(
                    "Aún no hay una factura registrada para este cliente.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                btnFactura.Enabled = false;
                return;
            }

            var ultimaPartes = ultimaLineaUsuario.Split(';');
            if (ultimaPartes.Length < 10)
            {
                MessageBox.Show(
                    "No se pudo leer la última factura (formato inválido).",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Clave para agrupar todas las líneas de esa misma factura:
            // utilizamos Fecha + Usuario + TotalFactura
            var fechaTexto = ultimaPartes[0];
            var usuarioTexto = ultimaPartes[1];
            var totalFacturaTexto = ultimaPartes[9];

            // Filtramos todas las líneas que pertenezcan a esa factura
            var lineasFactura = lineas
                .Select(l => l.Split(';'))
                .Where(p =>
                    p.Length >= 10 &&
                    p[0] == fechaTexto &&
                    p[1] == usuarioTexto &&
                    p[9] == totalFacturaTexto)
                .ToList();

            if (!lineasFactura.Any())
            {
                MessageBox.Show(
                    "No se pudieron reconstruir los detalles de la factura.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Reconstruimos la factura en memoria
            var factura = new Factura
            {
                Cliente = this.usuario,
                MetodoPago = MetodoPago.Efectivo
            };

            decimal subtotalConDescuento = 0m;
            decimal impuesto = 0m;
            decimal totalFactura = 0m;

            decimal sumaBrutaLineas = 0m;

            foreach (var partes in lineasFactura)
            {
                string productor = partes[2];
                string nombreProducto = partes[3];

                int cantidad = int.Parse(partes[4], CultureInfo.InvariantCulture);
                decimal precioUnitario = decimal.Parse(partes[5], CultureInfo.InvariantCulture);
                decimal totalLinea = decimal.Parse(partes[6], CultureInfo.InvariantCulture);

                subtotalConDescuento = decimal.Parse(partes[7], CultureInfo.InvariantCulture);
                impuesto = decimal.Parse(partes[8], CultureInfo.InvariantCulture);
                totalFactura = decimal.Parse(partes[9], CultureInfo.InvariantCulture);

                sumaBrutaLineas += precioUnitario * cantidad;

                var producto = new Producto
                {
                    Productor = productor,
                    NombreProducto = nombreProducto,
                    Cantidad = cantidad,
                    Precio = precioUnitario,
                    UnidadMedida = UnidadMedida.Unidades // por simplicidad
                };

                factura.Productos.Add(producto);
            }

            var descuento = sumaBrutaLineas - subtotalConDescuento;
            if (descuento < 0)
            {
                descuento = 0;
            }

            factura.Descuento = descuento;

            // Mostrar la factura reconstruida
            var facturaForm = new FacturaView(factura)
            {
                MdiParent = this
            };

            facturaForm.Show();
        }

        /// <summary>
        /// Muestra la ventana con la lista de productores y sus productos.
        /// </summary>
        private void btnListaProductores_Click(object sender, EventArgs e)
        {
            var seleccionView = new SeleccionPuntoFeriaView(
                this.usuario,
                this.puntoFeriaService,
                this.productoService,
                this.carritoService)
            {
                MdiParent = this
            };

            seleccionView.Show();
        }

        /// <summary>
        /// Muestra la ventana de carrito de compras.
        /// </summary>
        private void btnCarrito_Click(object sender, EventArgs e)
        {
            var view = new CarritoComprasView(carritoService)
            {
                UsuarioActual = this.usuario,
                MdiParent = this
            };

            view.Show();
        }

        /// <summary>
        /// Muestra la ventana de reportes y estadísticas.
        /// </summary>
        private void btnReportes_Click(object sender, EventArgs e)
        {
            var view = new ReporteEstadisticasView
            {
                MdiParent = this
            };

            view.Show();
        }
    }
}
