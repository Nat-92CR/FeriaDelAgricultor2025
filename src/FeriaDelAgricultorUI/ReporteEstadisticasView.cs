using FeriaDelAgricultorController;
using System;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Vista que permite al usuario consultar diferentes reportes
    /// y estadísticas basadas en las facturas registradas.
    /// </summary>
    public partial class ReporteEstadisticasView : Form
    {
        private readonly EstadisticasService estadisticasService = new EstadisticasService();

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ReporteEstadisticasView"/>.
        /// </summary>
        public ReporteEstadisticasView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento de carga de la vista. Inicializa controles y muestra
        /// un reporte por defecto.
        /// </summary>
        private void ReporteEstadisticasView_Load(object sender, EventArgs e)
        {
            // Rango de fechas por defecto: último mes.
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            cbTipoReporte.Items.Clear();
            cbTipoReporte.Items.Add("Productores con más compras");
            cbTipoReporte.Items.Add("Productos más comprados");
            cbTipoReporte.Items.Add("Gasto por mes");
            cbTipoReporte.SelectedIndex = 0;

            dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResultados.ReadOnly = true;
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.AllowUserToDeleteRows = false;
            dgvResultados.MultiSelect = false;
            dgvResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            CargarReporte();
        }

        /// <summary>
        /// Botón para aplicar filtros de fecha y recargar el reporte.
        /// </summary>
        private void btnAplicar_Click(object sender, EventArgs e)
        {
            CargarReporte();
        }

        /// <summary>
        /// Si cambia el tipo de reporte, recargamos automáticamente.
        /// </summary>
        private void cbTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarReporte();
        }

        /// <summary>
        /// Carga el reporte seleccionado usando el rango de fechas.
        /// </summary>
        private void CargarReporte()
        {
            DateTime? desde = dtpDesde.Value.Date;
            DateTime? hasta = dtpHasta.Value.Date;

            if (desde > hasta)
            {
                MessageBox.Show(
                    "La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.",
                    "Rango de fechas inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int tipo = cbTipoReporte.SelectedIndex;
            dgvResultados.DataSource = null;

            switch (tipo)
            {
                case 0:
                    dgvResultados.DataSource =
                        estadisticasService.ObtenerProductoresMasComprados(desde, hasta);
                    FormatearColumnasProductores();
                    break;

                case 1:
                    dgvResultados.DataSource =
                        estadisticasService.ObtenerProductosMasComprados(desde, hasta);
                    FormatearColumnasProductos();
                    break;

                case 2:
                    dgvResultados.DataSource =
                        estadisticasService.ObtenerGastoPorMes(desde, hasta);
                    FormatearColumnasGastoMes();
                    break;

                default:
                    dgvResultados.DataSource = null;
                    break;
            }
        }

        /// <summary>
        /// Ajusta encabezados y formato de columnas para el reporte
        /// de productores con más compras.
        /// </summary>
        private void FormatearColumnasProductores()
        {
            if (dgvResultados.Columns["Productor"] != null)
            {
                dgvResultados.Columns["Productor"].HeaderText = "Productor";
            }

            if (dgvResultados.Columns["CantidadProductos"] != null)
            {
                dgvResultados.Columns["CantidadProductos"].HeaderText = "Cantidad de productos";
            }

            if (dgvResultados.Columns["NumeroCompras"] != null)
            {
                dgvResultados.Columns["NumeroCompras"].HeaderText = "Número de compras";
            }

            if (dgvResultados.Columns["MontoTotal"] != null)
            {
                dgvResultados.Columns["MontoTotal"].HeaderText = "Monto total";
                dgvResultados.Columns["MontoTotal"].DefaultCellStyle.Format = "₡#,0";
            }
        }

        /// <summary>
        /// Ajusta encabezados y formato de columnas para el reporte
        /// de productos más comprados.
        /// </summary>
        private void FormatearColumnasProductos()
        {
            if (dgvResultados.Columns["Producto"] != null)
            {
                dgvResultados.Columns["Producto"].HeaderText = "Producto";
            }

            if (dgvResultados.Columns["CantidadTotal"] != null)
            {
                dgvResultados.Columns["CantidadTotal"].HeaderText = "Cantidad total";
            }

            if (dgvResultados.Columns["MontoTotal"] != null)
            {
                dgvResultados.Columns["MontoTotal"].HeaderText = "Monto total";
                dgvResultados.Columns["MontoTotal"].DefaultCellStyle.Format = "₡#,0";
            }
        }

        /// <summary>
        /// Ajusta encabezados y formato de columnas para el reporte
        /// de gasto por mes.
        /// </summary>
        private void FormatearColumnasGastoMes()
        {
            if (dgvResultados.Columns["Mes"] != null)
            {
                dgvResultados.Columns["Mes"].HeaderText = "Mes";
            }

            if (dgvResultados.Columns["MontoTotal"] != null)
            {
                dgvResultados.Columns["MontoTotal"].HeaderText = "Monto total";
                dgvResultados.Columns["MontoTotal"].DefaultCellStyle.Format = "₡#,0";
            }

            // Columnas técnicas que no hace falta mostrar
            if (dgvResultados.Columns["Año"] != null)
            {
                dgvResultados.Columns["Año"].Visible = false;
            }

            if (dgvResultados.Columns["MesNumero"] != null)
            {
                dgvResultados.Columns["MesNumero"].Visible = false;
            }
        }
    }
}
