using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio que se encarga de leer el archivo de facturas y
    /// calcular diferentes estadísticas de compras.
    /// </summary>
    public class EstadisticasService
    {
        private const string NombreArchivoFacturas = "Facturas.csv";

        /// <summary>
        /// Clase interna que representa el detalle de una línea de factura.
        /// </summary>
        private class RegistroFacturaDetalle
        {
            public DateTime Fecha { get; set; }
            public string Usuario { get; set; } = string.Empty;
            public string Productor { get; set; } = string.Empty;
            public string Producto { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal TotalLinea { get; set; }
            public decimal SubtotalFactura { get; set; }
            public decimal ImpuestoFactura { get; set; }
            public decimal TotalFactura { get; set; }
        }

        /// <summary>
        /// Lee el archivo de facturas y obtiene una lista de registros
        /// filtrados por rango de fechas (si se especifica).
        /// </summary>
        private List<RegistroFacturaDetalle> CargarRegistros(
            DateTime? desde,
            DateTime? hasta)
        {
            var resultado = new List<RegistroFacturaDetalle>();

            if (!File.Exists(NombreArchivoFacturas))
            {
                return resultado;
            }

            var lineas = File.ReadAllLines(NombreArchivoFacturas).Skip(1);

            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                var partes = linea.Split(';');
                if (partes.Length < 10)
                {
                    continue;
                }

                try
                {
                    var registro = new RegistroFacturaDetalle
                    {
                        Fecha = DateTime.ParseExact(
                            partes[0],
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture),
                        Usuario = partes[1],
                        Productor = partes[2],
                        Producto = partes[3],
                        Cantidad = int.Parse(partes[4], CultureInfo.InvariantCulture),
                        PrecioUnitario = decimal.Parse(partes[5], CultureInfo.InvariantCulture),
                        TotalLinea = decimal.Parse(partes[6], CultureInfo.InvariantCulture),
                        SubtotalFactura = decimal.Parse(partes[7], CultureInfo.InvariantCulture),
                        ImpuestoFactura = decimal.Parse(partes[8], CultureInfo.InvariantCulture),
                        TotalFactura = decimal.Parse(partes[9], CultureInfo.InvariantCulture)
                    };

                    resultado.Add(registro);
                }
                catch
                {
                    // Si una línea está corrupta, la ignoramos.
                    continue;
                }
            }

            if (desde.HasValue)
            {
                resultado = resultado
                    .Where(r => r.Fecha.Date >= desde.Value.Date)
                    .ToList();
            }

            if (hasta.HasValue)
            {
                resultado = resultado
                    .Where(r => r.Fecha.Date <= hasta.Value.Date)
                    .ToList();
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene una lista de productores ordenados por el monto total
        /// de compras realizadas (de mayor a menor).
        /// </summary>
        public List<ProductorEstadistica> ObtenerProductoresMasComprados(
            DateTime? desde,
            DateTime? hasta)
        {
            var registros = CargarRegistros(desde, hasta);

            var query = registros
                .GroupBy(r => r.Productor)
                .Select(g => new ProductorEstadistica
                {
                    Productor = g.Key,
                    MontoTotal = g.Sum(x => x.TotalLinea),
                    CantidadProductos = g.Sum(x => x.Cantidad),
                    NumeroCompras = g
                        .Select(x => new { x.Fecha, x.SubtotalFactura })
                        .Distinct()
                        .Count()
                })
                .OrderByDescending(e => e.MontoTotal)
                .ToList();

            return query;
        }

        /// <summary>
        /// Obtiene los productos más comprados, mostrando la cantidad total
        /// y el monto total gastado por cada producto.
        /// </summary>
        public List<ProductoEstadistica> ObtenerProductosMasComprados(
            DateTime? desde,
            DateTime? hasta)
        {
            var registros = CargarRegistros(desde, hasta);

            var query = registros
                .GroupBy(r => r.Producto)
                .Select(g => new ProductoEstadistica
                {
                    Producto = g.Key,
                    CantidadTotal = g.Sum(x => x.Cantidad),
                    MontoTotal = g.Sum(x => x.TotalLinea)
                })
                .OrderByDescending(e => e.CantidadTotal)
                .ToList();

            return query;
        }

        /// <summary>
        /// Obtiene el gasto total por mes (año/mes) dentro del rango de fechas.
        /// </summary>
        public List<GastoMesEstadistica> ObtenerGastoPorMes(
            DateTime? desde,
            DateTime? hasta)
        {
            var registros = CargarRegistros(desde, hasta);

            var query = registros
                .GroupBy(r => new { r.Fecha.Year, r.Fecha.Month })
                .Select(g => new GastoMesEstadistica
                {
                    Anio = g.Key.Year,
                    MesNumero = g.Key.Month,
                    Mes = $"{g.Key.Month:00}/{g.Key.Year}",
                    MontoTotal = g.Sum(x => x.TotalLinea)
                })
                .OrderBy(e => e.Anio)
                .ThenBy(e => e.MesNumero)
                .ToList();

            return query;
        }
    }

    /// <summary>
    /// Representa un resumen de compras por productor.
    /// </summary>
    public class ProductorEstadistica
    {
        public string Productor { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        public int NumeroCompras { get; set; }
        public decimal MontoTotal { get; set; }
    }

    /// <summary>
    /// Representa un resumen de compras por producto.
    /// </summary>
    public class ProductoEstadistica
    {
        public string Producto { get; set; } = string.Empty;
        public int CantidadTotal { get; set; }
        public decimal MontoTotal { get; set; }
    }

    /// <summary>
    /// Representa el gasto total realizado en un mes específico.
    /// </summary>
    public class GastoMesEstadistica
    {
        public int Anio { get; set; }
        public int MesNumero { get; set; }
        public string Mes { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
    }
}
