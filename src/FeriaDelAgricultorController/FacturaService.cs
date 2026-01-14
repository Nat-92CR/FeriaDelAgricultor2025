using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// SUMMARY:
    /// Servicio de lógica de negocio encargado de la generación y persistencia de facturas.
    ///
    /// Responsabilidades principales:
    /// - Construir una factura en memoria a partir de los datos del cliente, dirección,
    ///   método de pago y productos comprados.
    /// - Persistir la factura en un archivo CSV (Facturas.csv), registrando una línea
    ///   por cada producto incluido en la factura.
    ///
    /// Este servicio no interactúa directamente con la interfaz de usuario y se enfoca
    /// únicamente en la lógica de facturación.
    /// </summary>
    public class FacturaService
    {
        /// <summary>
        /// SUMMARY:
        /// Nombre del archivo CSV donde se almacenan las facturas.
        /// Se utiliza como constante para evitar valores mágicos en el código.
        /// </summary>
        private const string NombreArchivoFacturas = "Facturas.csv";

        /// <summary>
        /// SUMMARY:
        /// Genera una factura en memoria a partir de la información proporcionada.
        /// Este método NO guarda la factura en el archivo CSV; únicamente construye
        /// el objeto Factura con sus relaciones correspondientes.
        ///
        /// Validaciones:
        /// - El cliente no puede ser null.
        /// - La lista de productos no puede ser null ni estar vacía.
        /// </summary>
        /// <param name="cliente">Usuario que realiza la compra.</param>
        /// <param name="direccion">Dirección de entrega asociada a la factura.</param>
        /// <param name="metodoPago">Método de pago seleccionado.</param>
        /// <param name="productos">Lista de productos incluidos en la compra.</param>
        /// <returns>Factura generada en memoria.</returns>
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
        /// SUMMARY:
        /// Guarda una factura en el archivo CSV de facturas.
        ///
        /// Funcionamiento:
        /// - Si el archivo no existe, se crea automáticamente y se agrega el encabezado.
        /// - Por cada producto incluido en la factura, se genera una línea en el CSV.
        /// - Cada línea incluye información de fecha, cliente, dirección, método de pago,
        ///   producto, cantidades y totales.
        ///
        /// Este diseño permite analizar ventas por producto, productor o fecha.
        /// </summary>
        /// <param name="factura">Factura a persistir en el archivo CSV.</param>
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

            /// <summary>
            /// SUMMARY:
            /// Si el archivo de facturas no existe, se crea e incluye el encabezado
            /// con los nombres de las columnas.
            /// </summary>
            if (!File.Exists(rutaArchivo))
            {
                var encabezado =
                    "Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;Productor;Producto;Cantidad;PrecioUnitario;TotalLinea;SubtotalFactura;ImpuestoFactura;TotalFactura";
                File.WriteAllText(rutaArchivo, encabezado + Environment.NewLine);
            }

            /// <summary>
            /// SUMMARY:
            /// Conversión de la fecha a formato estándar (YYYY-MM-DD)
            /// para mantener consistencia en el archivo CSV.
            /// </summary>
            string fechaTexto = factura.Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            /// <summary>
            /// SUMMARY:
            /// Extracción de los datos de dirección.
            /// Se utilizan valores vacíos en caso de que alguna propiedad sea null
            /// para evitar errores en la escritura del archivo.
            /// </summary>
            string provincia = factura.Direccion?.Province ?? string.Empty;
            string canton = factura.Direccion?.Canton ?? string.Empty;
            string distrito = factura.Direccion?.District ?? string.Empty;
            string detalles = factura.Direccion?.OtherDetails ?? string.Empty;

            /// <summary>
            /// SUMMARY:
            /// Conversión del método de pago a texto para su almacenamiento en el CSV.
            /// </summary>
            string metodoPago = factura.MetodoPago.ToString();

            /// <summary>
            /// SUMMARY:
            /// Cálculo de totales de la factura utilizando la lógica del modelo Factura.
            /// </summary>
            decimal subtotal = factura.ObtenerSubtotalConDescuento();
            decimal impuesto = factura.ObtenerImpuesto();
            decimal total = factura.ObtenerTotal();

            var lineas = new List<string>();

            /// <summary>
            /// SUMMARY:
            /// Por cada producto de la factura se genera una línea independiente en el CSV,
            /// permitiendo análisis detallado por producto y productor.
            /// </summary>
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

            /// <summary>
            /// SUMMARY:
            /// Se agregan todas las líneas generadas al archivo CSV de facturas.
            /// </summary>
            File.AppendAllLines(rutaArchivo, lineas);
        }
    }
}
