using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorControllerTest
{
    /// <summary>
    /// SUMMARY:
    /// Pruebas unitarias para CarritoService.
    /// Se valida la lógica del carrito: agregar, acumular por Productor+NombreProducto,
    /// actualizar, eliminar, vaciar y calcular total.
    /// </summary>
    [TestClass]
    public class CarritoServiceTests
    {
        /// <summary>
        /// SUMMARY:
        /// Instancia del servicio bajo prueba (SUT: System Under Test).
        /// Se inicializa antes de cada test para evitar dependencia entre pruebas.
        /// </summary>
        private CarritoService? Carrito { get; set; }

        /// <summary>
        /// SUMMARY:
        /// Se ejecuta ANTES de cada TestMethod.
        /// Crea un carrito “limpio” para que cada prueba sea independiente.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Carrito = new CarritoService();
        }

        /// <summary>
        /// SUMMARY:
        /// Se ejecuta DESPUÉS de cada TestMethod.
        /// Limpia referencias para mantener aislamiento y liberar recursos.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            Carrito = null;
        }

        /// <summary>
        /// SUMMARY:
        /// Método auxiliar para construir un Producto de forma consistente.
        /// Facilita el Arrange y reduce repetición de código en las pruebas.
        /// </summary>
        private Producto CrearProducto(string productor, string nombre, decimal precio, int cantidad)
        {
            return new Producto
            {
                Productor = productor,
                NombreProducto = nombre,
                Precio = precio,
                Cantidad = cantidad
            };
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que AgregarProducto(Producto) lance ArgumentNullException
        /// si el producto recibido es null.
        /// </summary>
        [TestMethod]
        public void AgregarProducto_SiProductoEsNull_DebeLanzarArgumentNullException()
        {
            // Arrange
            Producto producto = null;

            // Act
            Action accion = () => Carrito!.AgregarProducto(producto);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que si producto.Cantidad <= 0, el método AgregarProducto(Producto)
        /// asuma cantidad 1.
        /// </summary>
        [TestMethod]
        public void AgregarProducto_CantidadInvalidaEnObjeto_AsumeUno()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 0);

            // Act
            Carrito!.AgregarProducto(producto);

            // Assert
            Assert.AreEqual(1, Carrito.ObtenerCantidad("Puesto A", "Tomate"));
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que AgregarProducto(Producto, int) lance ArgumentException
        /// si Productor está vacío o solo espacios.
        /// </summary>
        [TestMethod]
        public void AgregarProducto_SinProductor_DebeLanzarArgumentException()
        {
            // Arrange
            var producto = CrearProducto("   ", "Tomate", 100m, 1);

            // Act
            Action accion = () => Carrito!.AgregarProducto(producto, 1);

            // Assert
            Assert.ThrowsException<ArgumentException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que AgregarProducto(Producto, int) lance ArgumentException
        /// si NombreProducto está vacío o solo espacios.
        /// </summary>
        [TestMethod]
        public void AgregarProducto_SinNombreProducto_DebeLanzarArgumentException()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "   ", 100m, 1);

            // Act
            Action accion = () => Carrito!.AgregarProducto(producto, 1);

            // Assert
            Assert.ThrowsException<ArgumentException>(accion);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que al agregar un producto nuevo, se registre en el carrito
        /// con la cantidad indicada.
        /// </summary>
        [TestMethod]
        public void AgregarProducto_ProductoNuevo_SeAgregaConCantidadCorrecta()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 3);

            // Act
            Carrito!.AgregarProducto(producto);

            // Assert
            Assert.AreEqual(3, Carrito.ObtenerCantidad("Puesto A", "Tomate"));
            Assert.AreEqual(1, Carrito.ObtenerProductos().Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida la regla principal:
        /// si se agrega el mismo Productor + NombreProducto, se acumula la cantidad
        /// y no se crea un item adicional.
        /// </summary>
        [TestMethod]
        public void AgregarProducto_MismoProducto_AcumulaCantidad()
        {
            // Arrange
            var p1 = CrearProducto("Puesto A", "Tomate", 100m, 2);
            var p2 = CrearProducto("Puesto A", "Tomate", 100m, 3);

            Carrito!.AgregarProducto(p1);

            // Act
            Carrito.AgregarProducto(p2);

            // Assert
            Assert.AreEqual(5, Carrito.ObtenerCantidad("Puesto A", "Tomate"));
            Assert.AreEqual(1, Carrito.ObtenerProductos().Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que ObtenerCantidad retorne 0 si productor o nombreProducto son inválidos.
        /// </summary>
        [TestMethod]
        public void ObtenerCantidad_ParametrosInvalidos_RetornaCero()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 2);
            Carrito!.AgregarProducto(producto);

            // Act
            int c1 = Carrito.ObtenerCantidad("", "Tomate");
            int c2 = Carrito.ObtenerCantidad("Puesto A", "   ");

            // Assert
            Assert.AreEqual(0, c1);
            Assert.AreEqual(0, c2);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que ActualizarCantidad cambie la cantidad del producto existente.
        /// </summary>
        [TestMethod]
        public void ActualizarCantidad_ProductoExiste_ActualizaCantidad()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 2);
            Carrito!.AgregarProducto(producto);

            // Act
            Carrito.ActualizarCantidad("Puesto A", "Tomate", 7);

            // Assert
            Assert.AreEqual(7, Carrito.ObtenerCantidad("Puesto A", "Tomate"));
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que si ActualizarCantidad recibe cantidadNueva <= 0,
        /// el producto se elimine del carrito.
        /// </summary>
        [TestMethod]
        public void ActualizarCantidad_CantidadCero_EliminaProducto()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 2);
            Carrito!.AgregarProducto(producto);

            // Act
            Carrito.ActualizarCantidad("Puesto A", "Tomate", 0);

            // Assert
            Assert.AreEqual(0, Carrito.ObtenerCantidad("Puesto A", "Tomate"));
            Assert.AreEqual(0, Carrito.ObtenerProductos().Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que ActualizarCantidadConDelta retorne la diferencia
        /// (cantidadNueva - cantidadAnterior).
        /// </summary>
        [TestMethod]
        public void ActualizarCantidadConDelta_RetornaDeltaCorrecto()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 2);
            Carrito!.AgregarProducto(producto);

            // Act
            int delta = Carrito.ActualizarCantidadConDelta("Puesto A", "Tomate", 5);

            // Assert
            Assert.AreEqual(3, delta);
            Assert.AreEqual(5, Carrito.ObtenerCantidad("Puesto A", "Tomate"));
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que EliminarProducto remueva el producto si existe.
        /// </summary>
        [TestMethod]
        public void EliminarProducto_ProductoExiste_Elimina()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 2);
            Carrito!.AgregarProducto(producto);

            // Act
            Carrito.EliminarProducto("Puesto A", "Tomate");

            // Assert
            Assert.AreEqual(0, Carrito.ObtenerProductos().Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que EliminarProductoYRetornarCantidad devuelva la cantidad removida
        /// y elimine el producto del carrito.
        /// </summary>
        [TestMethod]
        public void EliminarProductoYRetornarCantidad_RetornaCantidadYElimina()
        {
            // Arrange
            var producto = CrearProducto("Puesto A", "Tomate", 100m, 6);
            Carrito!.AgregarProducto(producto);

            // Act
            int cantidad = Carrito.EliminarProductoYRetornarCantidad("Puesto A", "Tomate");

            // Assert
            Assert.AreEqual(6, cantidad);
            Assert.AreEqual(0, Carrito.ObtenerProductos().Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que VaciarYRetornarItems devuelva una copia de los items
        /// y deje el carrito vacío.
        /// </summary>
        [TestMethod]
        public void VaciarYRetornarItems_RetornaItemsYVacia()
        {
            // Arrange
            Carrito!.AgregarProducto(CrearProducto("Puesto A", "Tomate", 100m, 2));
            Carrito.AgregarProducto(CrearProducto("Puesto B", "Papa", 50m, 3));

            // Act
            var items = Carrito.VaciarYRetornarItems();

            // Assert
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(0, Carrito.ObtenerProductos().Count);
        }

        /// <summary>
        /// SUMMARY:
        /// Valida que ObtenerTotal calcule correctamente el total del carrito
        /// sumando (Precio * Cantidad) por cada producto.
        /// </summary>
        [TestMethod]
        public void ObtenerTotal_CalculaTotalCorrectamente()
        {
            // Arrange
            Carrito!.AgregarProducto(CrearProducto("Puesto A", "Tomate", 100m, 2)); // 200
            Carrito.AgregarProducto(CrearProducto("Puesto B", "Papa", 50m, 3));    // 150

            // Act
            decimal total = Carrito.ObtenerTotal();

            // Assert
            Assert.AreEqual(350m, total);
        }
    }
}
