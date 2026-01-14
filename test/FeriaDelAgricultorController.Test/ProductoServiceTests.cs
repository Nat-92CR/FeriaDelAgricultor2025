using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FeriaDelAgricultorControllerTest
{
    [TestClass]
    [DoNotParallelize] // Evita colisiones al escribir/leer el mismo CSV si VS corre pruebas en paralelo.
    public class ProductoServiceTests
    {
        private ProductoService? _productoService;

        private readonly string _rutaProductosCsv =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Productos.csv");

        [TestInitialize]
        public void TestInitialize()
        {
            PrepararArchivoProductosBase();
            _productoService = new ProductoService();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _productoService = null;

            try
            {
                if (File.Exists(_rutaProductosCsv))
                    File.Delete(_rutaProductosCsv);

                var dir = Path.GetDirectoryName(_rutaProductosCsv);
                if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                {
                    // Si quieres, puedes borrar la carpeta Data solo si queda vacía.
                    if (!Directory.EnumerateFileSystemEntries(dir).Any())
                        Directory.Delete(dir);
                }
            }
            catch
            {
                // No romper el pipeline de tests por limpieza.
            }
        }

        // ==========================================================
        // Helpers
        // ==========================================================
        private void PrepararArchivoProductosBase()
        {
            var carpeta = Path.GetDirectoryName(_rutaProductosCsv);
            if (!string.IsNullOrWhiteSpace(carpeta))
                Directory.CreateDirectory(carpeta);

            var lineas = new List<string>
            {
                "Productor;NombreProducto;Precio;Cantidad;UnidadMedida",
                "Puesto Central;Tomate;1200.50;10;Kilogramos",
                "Puesto Central;Papa;800;25;Kilogramos",
                "Puesto Verde;Lechuga;500;8;Unidades",
                "Puesto Verde;Jugo Naranja;1500;5;Litros"
            };

            File.WriteAllLines(_rutaProductosCsv, lineas);
        }

        private static Producto CrearProducto(string productor, string nombre, decimal precio, int cantidad, UnidadMedida unidad)
        {
            return new Producto
            {
                Productor = productor,
                NombreProducto = nombre,
                Precio = precio,
                Cantidad = cantidad,
                UnidadMedida = unidad
            };
        }

        // ==========================================================
        // Tests
        // ==========================================================

        [TestMethod]
        public void ObtenerTodos_CuandoHayDatos_RetornaListaConElementos()
        {
            // Arrange
            Assert.IsNotNull(_productoService);

            // Act
            var todos = _productoService.ObtenerTodos();

            // Assert
            Assert.IsNotNull(todos);
            Assert.IsTrue(todos.Count >= 4, "Debe cargar al menos 4 productos del CSV base.");
        }

        [TestMethod]
        public void ObtenerPorProductor_Existente_RetornaSoloEseProductor()
        {
            Assert.IsNotNull(_productoService);

            var lista = _productoService.ObtenerPorProductor("Puesto Central");

            Assert.IsNotNull(lista);
            Assert.IsTrue(lista.Count >= 2);
            Assert.IsTrue(lista.All(p => (p.Productor ?? "").Equals("Puesto Central", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void ObtenerPorProductor_Invalido_RetornaListaVacia()
        {
            Assert.IsNotNull(_productoService);

            var lista = _productoService.ObtenerPorProductor("");

            Assert.IsNotNull(lista);
            Assert.AreEqual(0, lista.Count);
        }

        [TestMethod]
        public void ObtenerProducto_Existente_RetornaProducto()
        {
            Assert.IsNotNull(_productoService);

            var p = _productoService.ObtenerProducto("Puesto Central", "Tomate");

            Assert.IsNotNull(p);
            Assert.AreEqual("Puesto Central", p.Productor);
            Assert.AreEqual("Tomate", p.NombreProducto);
        }

        [TestMethod]
        public void ObtenerProducto_NoExistente_RetornaNull()
        {
            Assert.IsNotNull(_productoService);

            var p = _productoService.ObtenerProducto("Puesto Central", "NoExiste");

            Assert.IsNull(p);
        }

        [TestMethod]
        public void AgregarProducto_NuevoProducto_RetornaTrueYSeGuarda()
        {
            Assert.IsNotNull(_productoService);

            var nuevo = CrearProducto("Puesto Nuevo", "Fresa", 2000m, 12, UnidadMedida.Kilogramos);

            var ok = _productoService.AgregarProducto(nuevo);

            Assert.IsTrue(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Nuevo", "Fresa");

            Assert.IsNotNull(p);
            Assert.AreEqual(12, p.Cantidad);
            Assert.AreEqual(2000m, p.Precio);
        }

        [TestMethod]
        public void AgregarProducto_Duplicado_RetornaFalse()
        {
            Assert.IsNotNull(_productoService);

            var duplicado = CrearProducto("Puesto Central", "Tomate", 999m, 99, UnidadMedida.Kilogramos);

            var ok = _productoService.AgregarProducto(duplicado);

            Assert.IsFalse(ok, "No debe permitir duplicados por (Productor + NombreProducto).");
        }

        [TestMethod]
        public void ActualizarProducto_Existente_RetornaTrueYActualiza()
        {
            Assert.IsNotNull(_productoService);

            var actualizado = CrearProducto("Puesto Central", "Tomate", 1300m, 7, UnidadMedida.Kilogramos);

            var ok = _productoService.ActualizarProducto(actualizado);

            Assert.IsTrue(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Central", "Tomate");

            Assert.IsNotNull(p);
            Assert.AreEqual(7, p.Cantidad);
            Assert.AreEqual(1300m, p.Precio);
        }

        [TestMethod]
        public void ActualizarProducto_NoExistente_RetornaFalse()
        {
            Assert.IsNotNull(_productoService);

            var actualizado = CrearProducto("Puesto X", "Producto X", 100m, 1, UnidadMedida.Unidades);

            var ok = _productoService.ActualizarProducto(actualizado);

            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void EliminarProducto_Existente_RetornaTrueYElimina()
        {
            Assert.IsNotNull(_productoService);

            var ok = _productoService.EliminarProducto("Puesto Verde", "Lechuga");
            Assert.IsTrue(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Verde", "Lechuga");
            Assert.IsNull(p);
        }

        [TestMethod]
        public void EliminarProducto_NoExistente_RetornaFalse()
        {
            Assert.IsNotNull(_productoService);

            var ok = _productoService.EliminarProducto("Puesto Verde", "NoExiste");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void EliminarPorProductor_Existente_EliminaVarios()
        {
            Assert.IsNotNull(_productoService);

            // Puesto Central tiene 2 productos en el CSV base
            int eliminados = _productoService.EliminarPorProductor("Puesto Central");

            Assert.IsTrue(eliminados >= 2);

            var recargado = new ProductoService();
            var listaCentral = recargado.ObtenerPorProductor("Puesto Central");

            Assert.AreEqual(0, listaCentral.Count);
        }

        [TestMethod]
        public void TryDescontarStock_Suficiente_RetornaTrueYDescuenta()
        {
            Assert.IsNotNull(_productoService);

            // Tomate inicia en 10
            var ok = _productoService.TryDescontarStock("Puesto Central", "Tomate", 3);
            Assert.IsTrue(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Central", "Tomate");

            Assert.IsNotNull(p);
            Assert.AreEqual(7, p.Cantidad);
        }

        [TestMethod]
        public void TryDescontarStock_Insuficiente_RetornaFalseNoCambia()
        {
            Assert.IsNotNull(_productoService);

            // Lechuga inicia en 8, intentamos 999
            var ok = _productoService.TryDescontarStock("Puesto Verde", "Lechuga", 999);
            Assert.IsFalse(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Verde", "Lechuga");

            Assert.IsNotNull(p);
            Assert.AreEqual(8, p.Cantidad);
        }

        [TestMethod]
        public void ReponerStock_AumentaStock()
        {
            Assert.IsNotNull(_productoService);

            // Papa inicia 25
            _productoService.ReponerStock("Puesto Central", "Papa", 5);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Central", "Papa");

            Assert.IsNotNull(p);
            Assert.AreEqual(30, p.Cantidad);
        }

        [TestMethod]
        public void CambiarStock_Existente_RetornaTrueYActualiza()
        {
            Assert.IsNotNull(_productoService);

            var ok = _productoService.CambiarStock("Puesto Verde", "Jugo Naranja", 99);
            Assert.IsTrue(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Verde", "Jugo Naranja");

            Assert.IsNotNull(p);
            Assert.AreEqual(99, p.Cantidad);
        }

        [TestMethod]
        public void CambiarPrecio_Existente_RetornaTrueYActualiza()
        {
            Assert.IsNotNull(_productoService);

            var ok = _productoService.CambiarPrecio("Puesto Verde", "Jugo Naranja", 2222.25m);
            Assert.IsTrue(ok);

            var recargado = new ProductoService();
            var p = recargado.ObtenerProducto("Puesto Verde", "Jugo Naranja");

            Assert.IsNotNull(p);
            Assert.AreEqual(2222.25m, p.Precio);
        }
    }
}
