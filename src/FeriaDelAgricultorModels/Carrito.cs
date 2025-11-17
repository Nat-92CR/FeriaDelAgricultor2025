using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa el carrito de compras de un cliente.
    /// </summary>
    public class Carrito
    {
        /// <summary>
        /// Productos que el cliente ha agregado al carrito.
        /// Cada producto usa la propiedad Cantidad para saber cuántas unidades lleva.
        /// </summary>
        public List<Producto> Productos { get; } = new List<Producto>();

        /// <summary>
        /// Agrega un producto al carrito. Si ya existe, suma la cantidad.
        /// </summary>
        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (producto == null || cantidad <= 0)
            {
                return;
            }

            // Buscamos si ya existe un producto igual (por nombre y productor)
            var existente = this.Productos.FirstOrDefault(p =>
                p.NombreProducto == producto.NombreProducto &&
                p.Productor == producto.Productor);

            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                // Clon sencillo para no modificar el original
                var copia = new Producto
                {
                    NombreProducto = producto.NombreProducto,
                    Precio = producto.Precio,
                    UnidadMedida = producto.UnidadMedida,
                    Productor = producto.Productor,
                    Cantidad = cantidad,
                };

                this.Productos.Add(copia);
            }
        }

        /// <summary>
        /// Elimina un producto del carrito por nombre y productor.
        /// </summary>
        public void EliminarProducto(string nombreProducto, string productor)
        {
            var existente = this.Productos.FirstOrDefault(p =>
                p.NombreProducto == nombreProducto &&
                p.Productor == productor);

            if (existente != null)
            {
                this.Productos.Remove(existente);
            }
        }

        /// <summary>
        /// Calcula el total actual del carrito.
        /// </summary>
        public decimal ObtenerTotal()
        {
            return this.Productos.Sum(p => p.Precio * p.Cantidad);
        }

        /// <summary>
        /// Limpia el carrito después de confirmar la compra.
        /// </summary>
        public void Limpiar()
        {
            this.Productos.Clear();
        }
    }
}
