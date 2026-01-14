using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorControllerTest
{
    /// <summary>
    /// SUMMARY:
    /// Pruebas unitarias para FacturaService.
    /// Se valida:
    /// - Generación de factura en memoria (validaciones y contenido).
    /// - Persistencia en CSV (creación, encabezado, líneas por producto y no duplicar encabezado).
    /// </summary>
    [TestClass]
    public class FacturaServiceTests
    {
        /// <summary>
        /// SUMMARY:
        /// Instancia del servicio bajo prueba (SUT).
        /// </summary>
        private FacturaService? FacturaService { get; set; }

        /// <summary>
        /// SUMMARY:
        /// Ruta real donde FacturaService escribe el archivo, según su implementación:
        /// AppDomain.CurrentDomain.BaseDirectory/Data/Facturas.csv
        /// </summary>
        private string RutaFacturasCsv =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Facturas.csv");

        /// <summary>
        /// SUMMARY:
        /// Se ejecuta ANTES de cada test para:
        /// - Crear la instancia del servicio
        /// - Asegurar un entorno limpio eliminando Facturas.csv si existe
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FacturaService = new FacturaService();

            if (File.Exists(RutaFacturasCsv))
            {
                File.Delete(RutaFacturasCsv);
            }
        }

        /// <summary>
        /// SUMMARY:
        /// Se ejecuta DESPUÉS de cada test para:
        /// - Limpiar el archivo generado
        /// - Evitar que una prueba afecte a otra
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(RutaFacturasCsv))
            {
                File.Delete(RutaFacturasCsv);
            }

            FacturaService = null;
        }

        /// <summary>
        /// SUMMARY:
        /// Crea un cliente mínimo válido para generar factura.
        /// Se usa el constructor público de Usuario (porque el constructor vacío es internal).
        /// </summary>
        private Usuario ClienteValido()
        {
            return new Usuario(
                name: "Natalia",
                lastName: "Tobal",
                username: "natalia",
                password: "1234",
                directions: "[]",
                tipoUsuario: TipoUsuario.Cliente
            );
        }

        /// <summary>
        /// SUMMARY:
        /// Crea una dirección válida basada en las propiedades usadas por FacturaService:
        /// Province, Canton, District, OtherDetails.
        /// </summary>
        private Direccion DireccionValida()
        {
            return new Direccion
            {
                Province = "San Jose",
                Canton = "Central",
                District = "Carmen",
                OtherDetails = "Casa 123"
            };
        }

        /// <summary>
        /// SUMMARY:
        /// Crea una lista de productos válida con Productor, NombreProducto, Precio y Cantidad.
        /// </summary>
        private List<Producto> ProductosValidos()
        {
            return new List<Producto>
            {
                new Producto { Productor = "Don Juan", NombreProducto = "Tomate", Precio = 100m, Cantidad = 2 },
                new Producto { Productor = "Doña Ana", NombreProducto = "Papa", Precio = 50m, Cantidad = 3 }
            };
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que GenerarFactura lance ArgumentNullException si cliente es null.
        /// </summary>
        [TestMethod]
        public void GenerarFactura_ClienteNull_DebeLanzarArgumentNullException()
        {
            // Arrange
            Usuario cliente = null;

            // Act
            Action accion = () => FacturaService!.GenerarFactura(cliente, DireccionValida(), MetodoPago.Efectivo, ProductosValidos());

            // Assert
            Assert.ThrowsException<ArgumentNullException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que GenerarFactura lance ArgumentException si productos es null.
        /// </summary>
        [TestMethod]
        public void GenerarFactura_ProductosNull_DebeLanzarArgumentException()
        {
            // Arrange
            var cliente = ClienteValido();
            List<Producto> productos = null;

            // Act
            Action accion = () => FacturaService!.GenerarFactura(cliente, DireccionValida(), MetodoPago.Efectivo, productos);

            // Assert
            Assert.ThrowsException<ArgumentException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que GenerarFactura lance ArgumentException si productos está vacío.
        /// </summary>
        [TestMethod]
        public void GenerarFactura_ProductosVacios_DebeLanzarArgumentException()
        {
            // Arrange
            var cliente = ClienteValido();
            var productos = new List<Producto>();

            // Act
            Action accion = () => FacturaService!.GenerarFactura(cliente, DireccionValida(), MetodoPago.Efectivo, productos);

            // Assert
            Assert.ThrowsException<ArgumentException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que GenerarFactura construya la Factura en memoria con sus datos principales:
        /// Cliente, Dirección, Método de pago y lista de productos.
        /// </summary>
        [TestMethod]
        public void GenerarFactura_DatosValidos_CreaFacturaEnMemoria()
        {
            // Arrange
            var cliente = ClienteValido();
            var direccion = DireccionValida();
            var productos = ProductosValidos();

            // Act
            var factura = FacturaService!.GenerarFactura(cliente, direccion, MetodoPago.Efectivo, productos);

            // Assert
            Assert.IsNotNull(factura);
            Assert.IsNotNull(factura.Cliente);
            Assert.AreEqual("natalia", factura.Cliente.Username);

            Assert.IsNotNull(factura.Direccion);
            Assert.AreEqual("San Jose", factura.Direccion.Province);

            Assert.AreEqual(MetodoPago.Efectivo, factura.MetodoPago);

            Assert.IsNotNull(factura.Productos);
            Assert.AreEqual(2, factura.Productos.Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que GuardarFacturaEnCsv lance ArgumentNullException si factura es null.
        /// </summary>
        [TestMethod]
        public void GuardarFacturaEnCsv_FacturaNull_DebeLanzarArgumentNullException()
        {
            // Arrange
            Factura factura = null;

            // Act
            Action accion = () => FacturaService!.GuardarFacturaEnCsv(factura);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que GuardarFacturaEnCsv:
        /// - Cree el archivo Facturas.csv si no existe
        /// - Escriba encabezado
        /// - Agregue una línea por cada producto
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        public void GuardarFacturaEnCsv_PrimeraVez_CreaArchivoEncabezadoYLineas()
        {
            // Arrange
            var factura = FacturaService!.GenerarFactura(ClienteValido(), DireccionValida(), MetodoPago.Efectivo, ProductosValidos());

            // Act
            FacturaService.GuardarFacturaEnCsv(factura);

            // Assert
            Assert.IsTrue(File.Exists(RutaFacturasCsv), "No se creó Facturas.csv.");

            var lineas = File.ReadAllLines(RutaFacturasCsv).ToList();

            // 1 encabezado + 2 productos
            Assert.AreEqual(3, lineas.Count, "El CSV debería tener 1 encabezado + 2 líneas (una por producto).");

            Assert.IsTrue(lineas[0].StartsWith("Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;"),
                "El encabezado no coincide con lo esperado.");

            // Validación básica de contenido (sin depender de la fecha exacta)
            Assert.IsTrue(lineas[1].Contains(";natalia;"), "La primera línea de datos no contiene el usuario esperado.");
            Assert.IsTrue(lineas[2].Contains(";natalia;"), "La segunda línea de datos no contiene el usuario esperado.");

            // Cada línea debe tener 15 columnas separadas por ';'
            Assert.AreEqual(15, lineas[1].Split(';').Length, "La línea 1 no tiene 15 columnas.");
            Assert.AreEqual(15, lineas[2].Split(';').Length, "La línea 2 no tiene 15 columnas.");
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que si GuardarFacturaEnCsv se llama dos veces,
        /// el encabezado NO se duplique (porque solo se crea cuando el archivo no existe).
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        public void GuardarFacturaEnCsv_SegundaVez_NoDuplicaEncabezado()
        {
            // Arrange
            var factura1 = FacturaService!.GenerarFactura(ClienteValido(), DireccionValida(), MetodoPago.Efectivo, ProductosValidos());
            var factura2 = FacturaService.GenerarFactura(ClienteValido(), DireccionValida(), MetodoPago.Efectivo, ProductosValidos());

            // Act
            FacturaService.GuardarFacturaEnCsv(factura1);
            FacturaService.GuardarFacturaEnCsv(factura2);

            // Assert
            var lineas = File.ReadAllLines(RutaFacturasCsv).ToList();

            // 1 encabezado + 2 productos + 2 productos = 5 líneas
            Assert.AreEqual(5, lineas.Count, "Deberían ser 1 encabezado + 4 líneas de productos.");

            int encabezados = lineas.Count(l => l.StartsWith("Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;"));
            Assert.AreEqual(1, encabezados, "El encabezado se duplicó y no debería.");
        }
    }
}
