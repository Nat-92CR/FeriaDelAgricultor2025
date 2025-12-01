using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio que administra el carrito de compras del cliente.
    /// Aquí solo manejamos la lógica (no controles de UI).
    /// </summary>
    public class CarritoService
    {
        /// <summary>
        /// Lista interna de productos en el carrito.
        /// Cada elemento representa un producto con su cantidad.
        /// </summary>
        private readonly List<Producto> items;

        /// <summary>
        /// Crea un carrito vacío.
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
            // Devolvemos una copia para no exponer la lista interna.
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
        /// Agrega un producto al carrito.
        /// Si ya existe el mismo producto (mismo productor y nombre),
        /// se suma la cantidad. Si no existe, se agrega uno nuevo.
        /// </summary>
        public void AgregarProducto(Producto producto, int cantidad = 1)
        {
            if (producto == null)
            {
                throw new ArgumentNullException(nameof(producto));
            }

            if (cantidad <= 0)
            {
                cantidad = 1;
            }

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(producto.Productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(producto.NombreProducto, StringComparison.OrdinalIgnoreCase));

            if (existente == null)
            {
                // Creamos una copia para no modificar la instancia original
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
        /// Elimina un producto específico del carrito.
        /// Se identifica por productor + nombre de producto.
        /// </summary>
        public void EliminarProducto(string productor, string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(productor) ||
                string.IsNullOrWhiteSpace(nombreProducto))
            {
                return;
            }

            var existente = items.FirstOrDefault(p =>
                p.Productor.Equals(productor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

            if (existente != null)
            {
                items.Remove(existente);
            }
        }

        /// <summary>
        /// Vacía completamente el carrito.
        /// </summary>
        public void VaciarCarrito()
        {
            items.Clear();
        }

        /// <summary>
        /// Calcula el total a pagar (precio * cantidad) de todos los productos del carrito.
        /// </summary>
        public decimal ObtenerTotal()
        {
            return items.Sum(p => (decimal)p.Precio * p.Cantidad);
        }
    }
}
