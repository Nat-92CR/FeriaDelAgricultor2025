using FeriaDelAgricultorModels;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using FeriaDelAgricultorController;



namespace FeriaDelAgricultorUI
{
    public partial class MainMenuView : Form
    {
        // Usuario que inició sesión
        private readonly Usuario usuario;

        private readonly FacturaService facturaService;
        // Más adelante podremos inyectar servicios de productos y reportes.

        private readonly ProductoService productoService;

        /// <summary>
        /// Este constructor recibe el usuario luego del login.
        /// </summary>
        /// <param name="usuario">Usuario autenticado.</param>
        public MainMenuView(Usuario usuario)
        {

         
            InitializeComponent();

         //Se instancia aquí.
            this.usuario = usuario;
            this.facturaService = new FacturaService();
            this.productoService = new ProductoService();

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

            // Nombre 
            string nombre = $"{usuario.Name} {usuario.LastName}".Trim();

            // Texto según tipo de usuario
            if (usuario.TipoUsuario == TipoUsuario.Cliente)
            {
                lblBienvenida.Text =
                    $"¡Bienvenid@ {nombre}! Has iniciado sesión como CLIENTE.";
                // Opcional: color de fondo para clientes
                this.BackColor = Color.LightSkyBlue;
            }
            else if (usuario.TipoUsuario == TipoUsuario.Productor)
            {
                lblBienvenida.Text =
                    $"¡Hola {nombre}! Has iniciado sesión como PRODUCTOR.";
                // Opcional: color de fondo para productores
                this.BackColor = Color.Moccasin;
            }
            else
            {
                lblBienvenida.Text = $"Bienvenid@ {nombre} ";
            }
        }

        /// <summary>
        /// Genera y muestra una factura utilizando el servicio de facturación.
        /// </summary>
        private void btnFactura_Click(object sender, EventArgs e)
        {
            if (this.usuario == null)
            {
                MessageBox.Show(
                    "No hay un usuario autenticado. Vuelva a iniciar sesión.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Pedimos al servicio que cree la factura para este usuario
            var factura = this.facturaService.CrearFacturaEjemplo(this.usuario);

            // Mostramos la vista de factura como ventana hija MDI
            var facturaForm = new FacturaView(factura);
            facturaForm.MdiParent = this;   // porque MainMenuView es MDI container
            facturaForm.Show();
        }

        /// <summary>
        /// Muestra la ventana con la lista de productores y sus productos.
        /// </summary>
        private void btnListaProductores_Click(object sender, EventArgs e)
        {
            //Pasar el servicio al formulario
            var listaView = new ListaProductoresView(this.productoService);
            listaView.MdiParent = this;
            listaView.Show();
        }


        /// <summary>
        /// Muestra la ventana de carrito de compras.
        /// </summary>
        private void btnCarrito_Click(object sender, EventArgs e)
        {
            var carritoView = new CarritoComprasView();
            carritoView.MdiParent = this;
            carritoView.Show();
        }

        /// <summary>
        /// Muestra la ventana de reportes y estadísticas.
        /// </summary>
        private void btnReportes_Click(object sender, EventArgs e)
        {
            var reportesView = new ReportesEstadisticasView();
            reportesView.MdiParent = this;
            reportesView.Show();
        }
    }
}
