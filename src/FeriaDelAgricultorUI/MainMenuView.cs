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

            ConfigurarMensajeBienvenida();
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
                return;
            }

            // Tomar la última línea registrada
            var ultimaPartes = lineas.Last().Split(';');
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
                // partes:
                // 0 Fecha
                // 1 Usuario
                // 2 Productor
                // 3 Producto
                // 4 Cantidad
                // 5 PrecioUnitario
                // 6 TotalLinea
                // 7 SubtotalFactura
                // 8 ImpuestoFactura
                // 9 TotalFactura

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

            // Calcular descuento aproximado a partir de los montos guardados
            // sumaBrutaLineas = suma de precio * cantidad
            // subtotalConDescuento = sumaBrutaLineas - descuento
            // => descuento = sumaBrutaLineas - subtotalConDescuento
            var descuento = sumaBrutaLineas - subtotalConDescuento;
            if (descuento < 0)
            {
                descuento = 0;
            }

            factura.Descuento = descuento;

            // Mostrar la factura reconstruida
            var facturaForm = new FacturaView(factura)
            {
                MdiParent = this // porque MainMenuView es MDI container
            };

            facturaForm.Show();
        }

        /// <summary>
        /// Muestra la ventana con la lista de productores y sus productos.
        /// </summary>
        private void btnListaProductores_Click(object sender, EventArgs e)
        {
            // Pasamos servicio de productos y carrito COMPARTIDO
            var listaView = new ListaProductoresView(this.productoService, this.carritoService)
            {
                MdiParent = this
            };

            listaView.Show();
        }

        /// <summary>
        /// Muestra la ventana de carrito de compras.
        /// </summary>
        private void btnCarrito_Click(object sender, EventArgs e)
        {
            // Crear vista del carrito
            var view = new CarritoComprasView(carritoService)
            {
                // Enviamos el usuario actual al carrito
                UsuarioActual = this.usuario,
                MdiParent = this
            };

            // Mostrar como ventana hija del menú
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
