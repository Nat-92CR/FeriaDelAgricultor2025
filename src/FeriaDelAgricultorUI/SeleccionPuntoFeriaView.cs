using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Vista (WinForms) que permite al usuario seleccionar:
    /// Provincia → Cantón → Punto de feria,
    /// antes de continuar hacia la lista de productores/productos.
    /// </summary>
    public partial class SeleccionPuntoFeriaView : Form
    {
        // ============================
        // Campos privados (dependencias)
        // ============================

        /// <summary>
        /// Usuario logueado (para contexto del flujo).
        /// </summary>
        private readonly Usuario usuario;

        /// <summary>
        /// Servicio para obtener provincias, cantones y puntos de feria.
        /// </summary>
        private readonly PuntoFeriaService puntoFeriaService;

        /// <summary>
        /// Servicio asociado a productos (se usa en pantallas posteriores).
        /// </summary>
        private readonly ProductoService productoService;

        /// <summary>
        /// Servicio del carrito (se mantiene a través del flujo).
        /// </summary>
        private readonly CarritoService carritoService;

        // ============================
        // Constructor
        // ============================

        /// <summary>
        /// Inicializa la vista y recibe las dependencias necesarias.
        /// Al cargar el formulario, se cargan las provincias disponibles.
        /// </summary>
        /// <param name="usuario">Usuario logueado.</param>
        /// <param name="puntoFeriaService">Servicio para puntos de feria.</param>
        /// <param name="productoService">Servicio para productos.</param>
        /// <param name="carritoService">Servicio para carrito.</param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si alguna dependencia es null.
        /// </exception>
        public SeleccionPuntoFeriaView(
            Usuario usuario,
            PuntoFeriaService puntoFeriaService,
            ProductoService productoService,
            CarritoService carritoService)
        {
            InitializeComponent();

            // Validación defensiva (evita nulls en tiempo de ejecución)
            this.usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            this.puntoFeriaService = puntoFeriaService ?? throw new ArgumentNullException(nameof(puntoFeriaService));
            this.productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            this.carritoService = carritoService ?? throw new ArgumentNullException(nameof(carritoService));

            // Cargar provincias al iniciar la vista
            CargarProvincias();
        }

        // ============================
        // Carga inicial (Provincias)
        // ============================

        /// <summary>
        /// Llena el ComboBox de Provincias usando el servicio de puntos de feria.
        /// También limpia cantones y puntos de feria para asegurar consistencia.
        /// </summary>
        private void CargarProvincias()
        {
            // Limpieza para evitar residuos de selecciones previas
            cbxProvincia.Items.Clear();
            cbxCanton.Items.Clear();
            cbxPuntoFeria.DataSource = null;
            cbxPuntoFeria.Items.Clear();

            // Obtener provincias desde el servicio
            List<string> provincias = puntoFeriaService.ObtenerProvincias();

            // Cargar provincias en el ComboBox
            foreach (var provincia in provincias)
            {
                cbxProvincia.Items.Add(provincia);
            }

            // Sin selección inicial
            cbxProvincia.SelectedIndex = -1;
            cbxCanton.SelectedIndex = -1;
            cbxPuntoFeria.SelectedIndex = -1;
        }

        // ============================
        // Eventos de selección (Provincia/Cantón)
        // ============================

        /// <summary>
        /// Evento: se dispara cuando cambia la provincia seleccionada.
        /// Carga los cantones disponibles para la provincia seleccionada.
        /// </summary>
        /// <param name="sender">Control que dispara el evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void cbxProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpiar dependientes: Cantón y Punto de feria
            cbxCanton.Items.Clear();
            cbxPuntoFeria.DataSource = null;
            cbxPuntoFeria.Items.Clear();

            // Si no hay provincia seleccionada, no hacemos nada
            if (cbxProvincia.SelectedItem == null)
                return;

            // Obtener provincia seleccionada
            string provinciaSeleccionada = cbxProvincia.SelectedItem.ToString();

            // Consultar cantones para la provincia
            List<string> cantones = puntoFeriaService.ObtenerCantonesPorProvincia(provinciaSeleccionada);

            // Cargar cantones en el ComboBox
            foreach (var canton in cantones)
            {
                cbxCanton.Items.Add(canton);
            }

            // Sin selección en cascada
            cbxCanton.SelectedIndex = -1;
            cbxPuntoFeria.SelectedIndex = -1;
        }

        /// <summary>
        /// Evento: se dispara cuando cambia el cantón seleccionado.
        /// Carga los puntos de feria disponibles para la provincia y cantón seleccionados.
        /// </summary>
        /// <param name="sender">Control que dispara el evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void cbxCanton_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpiar puntos de feria anteriores
            cbxPuntoFeria.DataSource = null;
            cbxPuntoFeria.Items.Clear();

            // Debe existir provincia y cantón seleccionados
            if (cbxProvincia.SelectedItem == null || cbxCanton.SelectedItem == null)
                return;

            // Obtener selecciones
            string provinciaSeleccionada = cbxProvincia.SelectedItem.ToString();
            string cantonSeleccionado = cbxCanton.SelectedItem.ToString();

            // Consultar puntos de feria para provincia/cantón
            List<PuntoFeria> puntos = puntoFeriaService.ObtenerPuntosPorProvinciaYCanton(
                provinciaSeleccionada,
                cantonSeleccionado);

            // Cargar puntos en ComboBox usando DataSource (para mostrar Nombre)
            cbxPuntoFeria.DataSource = puntos;
            cbxPuntoFeria.DisplayMember = nameof(PuntoFeria.Nombre);

            // Sin selección inicial
            cbxPuntoFeria.SelectedIndex = -1;
        }

        // ============================
        // Evento del botón Continuar
        // ============================

        /// <summary>
        /// Evento Click del botón "Continuar".
        /// Valida que se haya seleccionado Provincia, Cantón y Punto de feria.
        /// Si todo es válido, abre la vista de lista de productores (filtrada por ubicación).
        /// </summary>
        /// <param name="sender">Control que dispara el evento (botón).</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnContinuar_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas de selección
            if (cbxProvincia.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una provincia.");
                return;
            }

            if (cbxCanton.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un cantón.");
                return;
            }

            if (cbxPuntoFeria.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un punto de feria.");
                return;
            }

            // Lectura de selección
            string provinciaSeleccionada = cbxProvincia.SelectedItem.ToString();
            PuntoFeria puntoSeleccionado = cbxPuntoFeria.SelectedItem as PuntoFeria;

            if (puntoSeleccionado == null)
            {
                MessageBox.Show("Error al leer el punto de feria seleccionado.");
                return;
            }

            // En esta pantalla NO se selecciona un Productor,
            // por lo tanto se pasa null y se selecciona productor en la siguiente vista.
            Productor productor = null;

            // Abrir la vista siguiente
            var listaView = new ListaProductoresView(
                this.productoService,
                this.carritoService,
                provinciaSeleccionada,
                puntoSeleccionado,
                productor)
            {
                // Mantener MDI si tu app trabaja como MDI
                MdiParent = this.MdiParent
            };

            listaView.Show();
            this.Close();
        }
    }
}
