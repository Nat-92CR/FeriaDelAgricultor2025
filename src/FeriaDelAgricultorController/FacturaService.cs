using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio encargado de generar facturas a partir del carrito
    /// y de guardarlas en el archivo Facturas.csv.
    /// </summary>
    public class FacturaService
    {
        private const string NombreArchivoFacturas = "Facturas.csv";

        /// <summary>
        /// Genera una factura en memoria a partir de los datos recibidos.
        /// Este método no guarda todavía la factura en el archivo.
        /// </summary>
        public Factura GenerarFactura(
            Usuario cliente,
            Direccion direccion,
            MetodoPago metodoPago,
            List<Producto> productos)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (productos == null || productos.Count == 0)
                throw new ArgumentException("La lista de productos no puede estar vacía.", nameof(productos));

            var factura = new Factura
            {
                Cliente = cliente,
                Direccion = direccion ?? new Direccion(),
                MetodoPago = metodoPago,
                Productos = new List<Producto>(productos)
            };

            return factura;
        }

        /// <summary>
        /// Guarda la factura en el archivo CSV, agregando una línea por cada producto.
        /// El archivo se crea si no existe e incluye encabezado.
        /// </summary>
        public void GuardarFacturaEnCsv(Factura factura)
        {
            if (factura == null)
                throw new ArgumentNullException(nameof(factura));

            var rutaArchivo = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                NombreArchivoFacturas
            );

            var carpetaData = Path.GetDirectoryName(rutaArchivo);
            if (!string.IsNullOrWhiteSpace(carpetaData) && !Directory.Exists(carpetaData))
                Directory.CreateDirectory(carpetaData);

            // Encabezado si el archivo no existe
            if (!File.Exists(rutaArchivo))
            {
                var encabezado =
                    "Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;Productor;Producto;Cantidad;PrecioUnitario;TotalLinea;SubtotalFactura;ImpuestoFactura;TotalFactura";
                File.WriteAllText(rutaArchivo, encabezado + Environment.NewLine);
            }

            string fechaTexto = factura.Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Dirección (según tu modelo actual en inglés)
            string provincia = factura.Direccion?.Province ?? string.Empty;
            string canton = factura.Direccion?.Canton ?? string.Empty;
            string distrito = factura.Direccion?.District ?? string.Empty;
            string detalles = factura.Direccion?.OtherDetails ?? string.Empty;

            string metodoPago = factura.MetodoPago.ToString();

            decimal subtotal = factura.ObtenerSubtotalConDescuento();
            decimal impuesto = factura.ObtenerImpuesto();
            decimal total = factura.ObtenerTotal();

            var lineas = new List<string>();

            foreach (var producto in factura.Productos)
            {
                decimal totalLinea = producto.Precio * producto.Cantidad;

                string linea = string.Join(";", new[]
                {
                    fechaTexto,
                    factura.Cliente?.Username ?? string.Empty,
                    provincia,
                    canton,
                    distrito,
                    detalles,
                    metodoPago,
                    producto.Productor ?? string.Empty,
                    producto.NombreProducto ?? string.Empty,
                    producto.Cantidad.ToString(CultureInfo.InvariantCulture),
                    producto.Precio.ToString(CultureInfo.InvariantCulture),
                    totalLinea.ToString(CultureInfo.InvariantCulture),
                    subtotal.ToString(CultureInfo.InvariantCulture),
                    impuesto.ToString(CultureInfo.InvariantCulture),
                    total.ToString(CultureInfo.InvariantCulture)
                });

                lineas.Add(linea);
            }

            File.AppendAllLines(rutaArchivo, lineas);
        }
    }
}
