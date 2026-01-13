using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio que administra el carrito de compras del cliente.
    /// Maneja la lista de productos agregados, sus cantidades y cálculos de total.
    /// </summary>
    public class CarritoService
    {
        /// <summary>
        /// Lista interna de productos en el carrito.
        /// Cada elemento representa un producto único (por Productor + NombreProducto) con su cantidad acumulada.
        /// </summary>
        private readonly List<Producto> items;

        /// <summary>
        /// Inicializa un carrito vacío.
        /// </summary>
        public CarritoService()
        {
            items = new List<Producto>();
        }

        /// <summary>
        /// Devuelve una copia de la lista de productos en el carrito.
        /// </summary>
        public List<Producto> ObtenerProductos()
        {
            return items
                .Select(p => new Producto
                {
                    NombreProducto = p.NombreProducto,
                    Precio = p.Precio,
                    Cantidad = p.Cantidad,
                    UnidadMedida = p.UnidadMedida,
                    Productor = p.Productor
                })
                .ToList();
        }

        /// <summary>
        /// Agrega un producto al carrito usando la cantidad que venga en el objeto (producto.Cantidad).
        /// Si producto.Cantidad es menor o igual a 0, se asume 1.
        /// </summary>
        /// <param name="producto">Producto a agregar.</param>
        public void AgregarProducto(Producto producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));

            int cantidad = (producto.Cantidad > 0) ? producto.Cantidad : 1;
            AgregarProducto(producto, cantidad);
        }

        /// <summary>
        /// Agrega un producto al carrito.
        /// Si ya existe el mismo producto (mismo productor y nombre), se suma la cantidad.
        /// Si no existe, se agrega como nuevo.
        /// </summary>
        /// <param name="producto">Producto a agregar.</param>
        /// <param name="cantidad">Cantidad a sumar (mínimo 1).</param>
        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));

            if (string.IsNullOrWhiteSpace(producto.Productor))
                throw new ArgumentException("El producto debe tener Productor.", nameof(producto));

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
                throw new ArgumentException("El producto debe tener NombreProducto.", nameof(producto));

            if (cantidad <= 0) cantidad = 1;

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(producto.Productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(producto.NombreProducto, StringComparison.OrdinalIgnoreCase));

            if (existente == null)
            {
                items.Add(new Producto
                {
                    NombreProducto = producto.NombreProducto,
                    Precio = producto.Precio,
                    Cantidad = cantidad,
                    UnidadMedida = producto.UnidadMedida,
                    Productor = producto.Productor
                });
            }
            else
            {
                existente.Cantidad += cantidad;
            }
        }

        /// <summary>
        /// Obtiene la cantidad actual de un producto en el carrito.
        /// Retorna 0 si no existe.
        /// </summary>
        public int ObtenerCantidad(string productor, string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(productor) || string.IsNullOrWhiteSpace(nombreProducto))
                return 0;

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

            return existente?.Cantidad ?? 0;
        }

        /// <summary>
        /// Actualiza la cantidad de un producto ya existente en el carrito.
        /// Si la cantidad nueva es 0 o menor, el producto se elimina del carrito.
        /// </summary>
        /// <param name="productor">Nombre del puesto/productor.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
        /// <param name="cantidadNueva">Cantidad nueva.</param>
        public void ActualizarCantidad(string productor, string nombreProducto, int cantidadNueva)
        {
            if (string.IsNullOrWhiteSpace(productor) || string.IsNullOrWhiteSpace(nombreProducto))
                return;

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

            if (existente == null) return;

            if (cantidadNueva <= 0)
            {
                items.Remove(existente);
                return;
            }

            existente.Cantidad = cantidadNueva;
        }

        /// <summary>
        /// Actualiza la cantidad y devuelve el delta (diferencia) respecto a lo anterior:
        /// delta = cantidadNueva - cantidadAnterior
        ///
        /// - Si delta > 0: el usuario está agregando más unidades.
        /// - Si delta < 0: el usuario está quitando unidades.
        ///
        /// Esto es clave para sincronizar inventario con ProductoService.
        /// </summary>
        /// <returns>Delta (cantidadNueva - cantidadAnterior).</returns>
        public int ActualizarCantidadConDelta(string productor, string nombreProducto, int cantidadNueva)
        {
            int anterior = ObtenerCantidad(productor, nombreProducto);

            // Reutilizamos la lógica existente
            ActualizarCantidad(productor, nombreProducto, cantidadNueva);

            int actual = ObtenerCantidad(productor, nombreProducto);
            return actual - anterior;
        }

        /// <summary>
        /// Elimina un producto específico del carrito (por productor + nombre de producto).
        /// </summary>
        public void EliminarProducto(string productor, string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(productor) || string.IsNullOrWhiteSpace(nombreProducto))
                return;

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

            if (existente != null)
            {
                items.Remove(existente);
            }
        }

        /// <summary>
        /// Elimina un producto y retorna cuántas unidades se eliminaron.
        /// (Útil para reponer inventario en ProductoService.)
        /// </summary>
        public int EliminarProductoYRetornarCantidad(string productor, string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(productor) || string.IsNullOrWhiteSpace(nombreProducto))
                return 0;

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

            if (existente == null) return 0;

            int cantidad = existente.Cantidad;
            items.Remove(existente);
            return cantidad;
        }

        /// <summary>
        /// Vacía completamente el carrito.
        /// </summary>
        public void VaciarCarrito()
        {
            items.Clear();
        }

        /// <summary>
        /// Vacía el carrito y retorna la lista de items que estaban dentro (copias),
        /// incluyendo sus cantidades, para poder reponer inventario.
        /// </summary>
        public List<Producto> VaciarYRetornarItems()
        {
            var copia = ObtenerProductos();
            items.Clear();
            return copia;
        }

        /// <summary>
        /// Calcula el total a pagar del carrito (precio * cantidad).
        /// </summary>
        public decimal ObtenerTotal()
        {
            return items.Sum(p => (decimal)p.Precio * p.Cantidad);
        }
    }
}
