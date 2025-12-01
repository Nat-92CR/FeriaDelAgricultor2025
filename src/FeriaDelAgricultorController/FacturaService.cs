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
        /// NO la guarda todavía en el archivo.
        /// </summary>
        public Factura GenerarFactura(
            Usuario cliente,
            Direccion direccion,
            MetodoPago metodoPago,
            List<Producto> productos)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente));
            }

            if (productos == null || productos.Count == 0)
            {
                throw new ArgumentException("La lista de productos no puede estar vacía.", nameof(productos));
            }

            var factura = new Factura
            {
                Cliente = cliente,
                Direccion = direccion ?? new Direccion(),
                MetodoPago = metodoPago,
                Productos = new List<Producto>(productos)
                // Fecha, PorcentajeImpuesto y Descuento ya vienen con valores por defecto.
            };

            return factura;
        }

        /// <summary>
        /// Guarda la factura en el archivo CSV, agregando una línea por cada producto.
        /// El formato es compatible con EstadisticasService.
        /// </summary>
        public void GuardarFacturaEnCsv(Factura factura)
        {
            if (factura == null)
            {
                throw new ArgumentNullException(nameof(factura));
            }

            // Asegurarnos de que exista el encabezado si el archivo no existe.
            if (!File.Exists(NombreArchivoFacturas))
            {
                var encabezado =
                    "Fecha;Usuario;Productor;Producto;Cantidad;PrecioUnitario;" +
                    "TotalLinea;SubtotalFactura;ImpuestoFactura;TotalFactura";
                File.WriteAllText(NombreArchivoFacturas, encabezado + Environment.NewLine);
            }

            var formatoFecha = "yyyy-MM-dd";
            var fechaTexto = factura.Fecha.ToString(formatoFecha, CultureInfo.InvariantCulture);

            // Calculamos montos globales de la factura.
            var subtotal = factura.ObtenerSubtotalConDescuento();
            var impuesto = factura.ObtenerImpuesto();
            var total = factura.ObtenerTotal();

            var lineas = new List<string>();

            foreach (var producto in factura.Productos)
            {
                var totalLinea = producto.Precio * producto.Cantidad;

                string linea =
                    string.Join(";", new[]
                    {
                        fechaTexto,
                        factura.Cliente.Username ?? string.Empty,
                        producto.Productor,
                        producto.NombreProducto,
                        producto.Cantidad.ToString(CultureInfo.InvariantCulture),
                        producto.Precio.ToString(CultureInfo.InvariantCulture),
                        totalLinea.ToString(CultureInfo.InvariantCulture),
                        subtotal.ToString(CultureInfo.InvariantCulture),
                        impuesto.ToString(CultureInfo.InvariantCulture),
                        total.ToString(CultureInfo.InvariantCulture)
                    });

                lineas.Add(linea);
            }

            File.AppendAllLines(NombreArchivoFacturas, lineas);
        }
    }
}
