using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio para manipular el carrito de compras del cliente.
    /// </summary>
    public class CarritoService
    {
        /// <summary>
        /// Carrito actual del usuario conectado.
        /// </summary>
        public Carrito CarritoActual { get; } = new Carrito();

        /// <summary>
        /// Agrega un producto al carrito actual.
        /// </summary>
        public void AgregarProducto(Producto producto, int cantidad)
        {
            this.CarritoActual.AgregarProducto(producto, cantidad);
        }

        /// <summary>
        /// Elimina un producto del carrito actual.
        /// </summary>
        public void EliminarProducto(string nombreProducto, string productor)
        {
            this.CarritoActual.EliminarProducto(nombreProducto, productor);
        }

        /// <summary>
        /// Limpia el carrito luego de confirmar la compra.
        /// </summary>
        public void LimpiarCarrito()
        {
            this.CarritoActual.Limpiar();
        }
    }
}
