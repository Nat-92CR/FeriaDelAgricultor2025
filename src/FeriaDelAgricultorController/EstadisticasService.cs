using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio que se encarga de leer el archivo de facturas (Facturas.csv)
    /// y calcular estadísticas / reportes de compras.
    ///
    /// Formato esperado (15 columnas):
    /// "Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;Productor;Producto;Cantidad;PrecioUnitario;TotalLinea;SubtotalFactura;ImpuestoFactura;TotalFactura"
    /// </summary>
    public class EstadisticasService
    {
        private const string NombreArchivoFacturas = "Facturas.csv";

        // =========================
        // Dependencias
        // =========================
        private readonly ProductoService _productoService;

        public EstadisticasService(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // =========================
        // DTO interno de lectura de facturas
        // =========================
        private class RegistroFacturaDetalle
        {
            public DateTime Fecha { get; set; }
            public string Usuario { get; set; } = string.Empty;

            public string Provincia { get; set; } = string.Empty;
            public string Canton { get; set; } = string.Empty;
            public string Distrito { get; set; } = string.Empty;

            public string MetodoPago { get; set; } = string.Empty;

            public string Productor { get; set; } = string.Empty;
            public string Producto { get; set; } = string.Empty;

            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal TotalLinea { get; set; }

            public decimal SubtotalFactura { get; set; }
            public decimal ImpuestoFactura { get; set; }
            public decimal TotalFactura { get; set; }

            public string ClaveFactura => $"{Fecha:yyyy-MM-dd}|{Usuario}|{TotalFactura.ToString(CultureInfo.InvariantCulture)}";
        }

        // ==================================
        // Rutas / carga de registros Facturas.csv
        // ==================================
        private string ObtenerRutaFacturas()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", NombreArchivoFacturas);
        }

        private List<RegistroFacturaDetalle> CargarRegistros(DateTime? desde, DateTime? hasta, string? usuarioFiltro = null)
        {
            var resultado = new List<RegistroFacturaDetalle>();
            var ruta = ObtenerRutaFacturas();

            if (!File.Exists(ruta))
                return resultado;

            var lineas = File.ReadAllLines(ruta)
                             .Skip(1)
                             .Where(l => !string.IsNullOrWhiteSpace(l));

            foreach (var linea in lineas)
            {
                var partes = linea.Split(';');
                if (partes.Length < 15) continue;

                try
                {
                    var registro = new RegistroFacturaDetalle
                    {
                        Fecha = DateTime.ParseExact(partes[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Usuario = (partes[1] ?? "").Trim(),

                        Provincia = (partes[2] ?? "").Trim(),
                        Canton = (partes[3] ?? "").Trim(),
                        Distrito = (partes[4] ?? "").Trim(),

                        MetodoPago = (partes[6] ?? "").Trim(),

                        Productor = (partes[7] ?? "").Trim(),
                        Producto = (partes[8] ?? "").Trim(),

                        Cantidad = int.Parse(partes[9], CultureInfo.InvariantCulture),
                        PrecioUnitario = decimal.Parse(partes[10], CultureInfo.InvariantCulture),
                        TotalLinea = decimal.Parse(partes[11], CultureInfo.InvariantCulture),

                        SubtotalFactura = decimal.Parse(partes[12], CultureInfo.InvariantCulture),
                        ImpuestoFactura = decimal.Parse(partes[13], CultureInfo.InvariantCulture),
                        TotalFactura = decimal.Parse(partes[14], CultureInfo.InvariantCulture)
                    };

                    resultado.Add(registro);
                }
                catch
                {
                    continue;
                }
            }

            if (!string.IsNullOrWhiteSpace(usuarioFiltro))
            {
                resultado = resultado
                    .Where(r => string.Equals(r.Usuario, usuarioFiltro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (desde.HasValue)
                resultado = resultado.Where(r => r.Fecha.Date >= desde.Value.Date).ToList();

            if (hasta.HasValue)
                resultado = resultado.Where(r => r.Fecha.Date <= hasta.Value.Date).ToList();

            return resultado;
        }

        // ==================================
        // DTO públicos para UI
        // ==================================
        public class ProductorEstadistica
        {
            public string Productor { get; set; } = string.Empty;
            public int CantidadProductos { get; set; }
            public int NumeroCompras { get; set; }
            public decimal MontoTotal { get; set; }
        }

        public class ProductoEstadistica
        {
            public string Producto { get; set; } = string.Empty;
            public int CantidadTotal { get; set; }
            public decimal MontoTotal { get; set; }
        }

        public class GastoMesEstadistica
        {
            public int Anio { get; set; }
            public int MesNumero { get; set; }
            public string Mes { get; set; } = string.Empty;
            public decimal MontoTotal { get; set; }
        }

        public class UsuarioEstadistica
        {
            public string Usuario { get; set; } = string.Empty;
            public int NumeroCompras { get; set; }
            public decimal MontoTotal { get; set; }
        }

        public class ResumenEstadistica
        {
            public int NumeroCompras { get; set; }
            public decimal TotalVentas { get; set; }
            public decimal TotalImpuestos { get; set; }
            public decimal TicketPromedio { get; set; }
        }

        // ==========================
        // NUEVOS: Inventario (Admin)
        // ==========================
        public class InventarioItem
        {
            public string Productor { get; set; } = string.Empty;
            public string Producto { get; set; } = string.Empty;
            public string Unidad { get; set; } = string.Empty;
            public int Stock { get; set; }
            public decimal Precio { get; set; }
            public decimal ValorInventario => Precio * Stock;
        }

        public class InventarioResumenProductor
        {
            public string Productor { get; set; } = string.Empty;
            public int TotalItems { get; set; }          // suma de stock
            public int CantidadProductos { get; set; }   // cantidad de productos distintos
            public decimal ValorTotal { get; set; }      // suma precio*stock
        }

        public class InventarioPorProductorReporte
        {
            public List<InventarioResumenProductor> Resumen { get; set; } = new();
            public List<InventarioItem> Detalle { get; set; } = new();
        }

        /// <summary>
        /// Admin: Inventario por productor (lee Productos.csv vía ProductoService).
        /// </summary>
        public InventarioPorProductorReporte ObtenerInventarioPorProductor()
        {
            var productos = _productoService.ObtenerTodos();

            var detalle = productos
                .Where(p => !string.IsNullOrWhiteSpace(p.Productor) && !string.IsNullOrWhiteSpace(p.NombreProducto))
                .Select(p => new InventarioItem
                {
                    Productor = p.Productor ?? "",
                    Producto = p.NombreProducto ?? "",
                    Unidad = p.UnidadMedida.ToString(),
                    Stock = p.Cantidad,
                    Precio = p.Precio
                })
                .OrderBy(x => x.Productor)
                .ThenBy(x => x.Producto)
                .ToList();

            var resumen = detalle
                .GroupBy(d => d.Productor)
                .Select(g => new InventarioResumenProductor
                {
                    Productor = g.Key,
                    TotalItems = g.Sum(x => x.Stock),
                    CantidadProductos = g.Count(),
                    ValorTotal = g.Sum(x => x.ValorInventario)
                })
                .OrderByDescending(x => x.ValorTotal)
                .ToList();

            return new InventarioPorProductorReporte
            {
                Resumen = resumen,
                Detalle = detalle
            };
        }

        // ==================================
        // Reportes base (Admin o Usuario)
        // ==================================
        public List<ProductorEstadistica> ObtenerProductoresMasComprados(DateTime? desde, DateTime? hasta)
            => ObtenerProductoresMasComprados(desde, hasta, null);

        public List<ProductorEstadistica> ObtenerProductoresMasComprados(DateTime? desde, DateTime? hasta, string? usuarioFiltro)
        {
            var registros = CargarRegistros(desde, hasta, usuarioFiltro);

            return registros
                .Where(r => !string.IsNullOrWhiteSpace(r.Productor))
                .GroupBy(r => r.Productor)
                .Select(g => new ProductorEstadistica
                {
                    Productor = g.Key,
                    MontoTotal = g.Sum(x => x.TotalLinea),
                    CantidadProductos = g.Sum(x => x.Cantidad),
                    NumeroCompras = g.Select(x => x.ClaveFactura).Distinct().Count()
                })
                .OrderByDescending(x => x.MontoTotal)
                .ToList();
        }

        public List<ProductoEstadistica> ObtenerProductosMasComprados(DateTime? desde, DateTime? hasta)
            => ObtenerProductosMasComprados(desde, hasta, null);

        public List<ProductoEstadistica> ObtenerProductosMasComprados(DateTime? desde, DateTime? hasta, string? usuarioFiltro)
        {
            var registros = CargarRegistros(desde, hasta, usuarioFiltro);

            return registros
                .Where(r => !string.IsNullOrWhiteSpace(r.Producto))
                .GroupBy(r => r.Producto)
                .Select(g => new ProductoEstadistica
                {
                    Producto = g.Key,
                    CantidadTotal = g.Sum(x => x.Cantidad),
                    MontoTotal = g.Sum(x => x.TotalLinea)
                })
                .OrderByDescending(x => x.CantidadTotal)
                .ToList();
        }

        public List<GastoMesEstadistica> ObtenerGastoPorMes(DateTime? desde, DateTime? hasta)
            => ObtenerGastoPorMes(desde, hasta, null);

        public List<GastoMesEstadistica> ObtenerGastoPorMes(DateTime? desde, DateTime? hasta, string? usuarioFiltro)
        {
            var registros = CargarRegistros(desde, hasta, usuarioFiltro);

            return registros
                .GroupBy(r => new { r.Fecha.Year, r.Fecha.Month })
                .Select(g => new GastoMesEstadistica
                {
                    Anio = g.Key.Year,
                    MesNumero = g.Key.Month,
                    Mes = $"{g.Key.Month:00}/{g.Key.Year}",
                    MontoTotal = g.Sum(x => x.TotalLinea)
                })
                .OrderBy(x => x.Anio)
                .ThenBy(x => x.MesNumero)
                .ToList();
        }

        public List<UsuarioEstadistica> ObtenerUsuariosTop(DateTime? desde, DateTime? hasta)
        {
            var registros = CargarRegistros(desde, hasta, null);

            return registros
                .Where(r => !string.IsNullOrWhiteSpace(r.Usuario))
                .GroupBy(r => r.Usuario)
                .Select(g => new UsuarioEstadistica
                {
                    Usuario = g.Key,
                    NumeroCompras = g.Select(x => x.ClaveFactura).Distinct().Count(),
                    MontoTotal = g.Sum(x => x.TotalLinea)
                })
                .OrderByDescending(x => x.MontoTotal)
                .ToList();
        }

        public ResumenEstadistica ObtenerResumenAdmin(DateTime? desde, DateTime? hasta)
        {
            var registros = CargarRegistros(desde, hasta, null);

            var facturas = registros
                .GroupBy(r => r.ClaveFactura)
                .Select(g => new
                {
                    TotalFactura = g.First().TotalFactura,
                    Impuesto = g.First().ImpuestoFactura
                })
                .ToList();

            var numeroCompras = facturas.Count;
            var totalVentas = facturas.Sum(x => x.TotalFactura);
            var totalImpuestos = facturas.Sum(x => x.Impuesto);

            var ticketProm = (numeroCompras == 0) ? 0 : (totalVentas / numeroCompras);

            return new ResumenEstadistica
            {
                NumeroCompras = numeroCompras,
                TotalVentas = totalVentas,
                TotalImpuestos = totalImpuestos,
                TicketPromedio = Math.Round(ticketProm, 2)
            };
        }

        public ResumenEstadistica ObtenerResumenUsuario(DateTime? desde, DateTime? hasta, string usuario)
        {
            var registros = CargarRegistros(desde, hasta, usuario);

            var facturas = registros
                .GroupBy(r => r.ClaveFactura)
                .Select(g => new
                {
                    TotalFactura = g.First().TotalFactura,
                    Impuesto = g.First().ImpuestoFactura
                })
                .ToList();

            var numeroCompras = facturas.Count;
            var totalVentas = facturas.Sum(x => x.TotalFactura);
            var totalImpuestos = facturas.Sum(x => x.Impuesto);

            var ticketProm = (numeroCompras == 0) ? 0 : (totalVentas / numeroCompras);

            return new ResumenEstadistica
            {
                NumeroCompras = numeroCompras,
                TotalVentas = totalVentas,
                TotalImpuestos = totalImpuestos,
                TicketPromedio = Math.Round(ticketProm, 2)
            };
        }
    }
}
