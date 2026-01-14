using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// SUMMARY:
    /// Servicio de lógica de negocio encargado de administrar el carrito de compras del cliente.
    /// Permite agregar, actualizar, eliminar y consultar productos dentro del carrito,
    /// así como calcular el total a pagar.
    ///
    /// El carrito maneja productos únicos identificados por la combinación
    /// Productor + NombreProducto, acumulando la cantidad correspondiente.
    /// </summary>
    public class CarritoService
    {
        /// <summary>
        /// SUMMARY:
        /// Lista interna que almacena los productos agregados al carrito.
        /// Cada elemento representa un producto único (por Productor + NombreProducto)
        /// con su cantidad acumulada.
        ///
        /// Esta lista no se expone directamente para mantener el encapsulamiento.
        /// </summary>
        private readonly List<Producto> items;

        /// <summary>
        /// SUMMARY:
        /// Constructor del servicio.
        /// Inicializa el carrito como una lista vacía de productos.
        /// </summary>
        public CarritoService()
        {
            items = new List<Producto>();
        }

        /// <summary>
        /// SUMMARY:
        /// Devuelve una copia de la lista de productos actualmente almacenados en el carrito.
        /// Se retorna una copia para evitar que código externo modifique la lista interna.
        /// </summary>
        /// <returns>
        /// Lista de productos (copias) con su nombre, precio, cantidad, unidad de medida y productor.
        /// </returns>
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
        /// SUMMARY:
        /// Agrega un producto al carrito utilizando la cantidad incluida en el objeto Producto.
        /// Si la cantidad es menor o igual a 0, se asume automáticamente una cantidad de 1.
        /// </summary>
        /// <param name="producto">
        /// Producto a agregar al carrito. No puede ser null.
        /// </param>
        public void AgregarProducto(Producto producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));

            int cantidad = (producto.Cantidad > 0) ? producto.Cantidad : 1;
            AgregarProducto(producto, cantidad);
        }

        /// <summary>
        /// SUMMARY:
        /// Agrega un producto al carrito con una cantidad específica.
        ///
        /// Reglas:
        /// - Si el producto ya existe (mismo Productor y NombreProducto), se suma la cantidad.
        /// - Si no existe, se agrega como un nuevo producto.
        /// - Si la cantidad es menor o igual a 0, se corrige automáticamente a 1.
        /// </summary>
        /// <param name="producto">
        /// Producto a agregar. Debe tener Productor y NombreProducto válidos.
        /// </param>
        /// <param name="cantidad">
        /// Cantidad a agregar al carrito (mínimo 1).
        /// </param>
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
        /// SUMMARY:
        /// Obtiene la cantidad actual de un producto específico dentro del carrito.
        /// Si el producto no existe o los parámetros son inválidos, retorna 0.
        /// </summary>
        /// <param name="productor">Nombre del productor o puesto.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
        /// <returns>Cantidad actual del producto en el carrito.</returns>
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
        /// SUMMARY:
        /// Actualiza la cantidad de un producto existente en el carrito.
        /// Si la nueva cantidad es menor o igual a 0, el producto se elimina del carrito.
        /// </summary>
        /// <param name="productor">Nombre del productor o puesto.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
        /// <param name="cantidadNueva">Nueva cantidad a asignar.</param>
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
        /// SUMMARY:
        /// Actualiza la cantidad de un producto y retorna el delta de la operación,
        /// es decir, la diferencia entre la cantidad nueva y la anterior.
        ///
        /// Este valor es utilizado para sincronizar cambios con el inventario.
        /// </summary>
        /// <param name="productor">Nombre del productor o puesto.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
        /// <param name="cantidadNueva">Nueva cantidad deseada.</param>
        /// <returns>Diferencia entre la cantidad nueva y la anterior.</returns>
        public int ActualizarCantidadConDelta(string productor, string nombreProducto, int cantidadNueva)
        {
            int anterior = ObtenerCantidad(productor, nombreProducto);

            ActualizarCantidad(productor, nombreProducto, cantidadNueva);

            int actual = ObtenerCantidad(productor, nombreProducto);
            return actual - anterior;
        }

        /// <summary>
        /// SUMMARY:
        /// Elimina un producto específico del carrito utilizando productor y nombre del producto.
        /// Si el producto no existe, no se realiza ninguna acción.
        /// </summary>
        /// <param name="productor">Nombre del productor o puesto.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
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
        /// SUMMARY:
        /// Elimina un producto del carrito y retorna la cantidad que tenía.
        /// Este método es útil para reponer inventario cuando un producto se elimina del carrito.
        /// </summary>
        /// <param name="productor">Nombre del productor o puesto.</param>
        /// <param name="nombreProducto">Nombre del producto.</param>
        /// <returns>Cantidad eliminada del carrito.</returns>
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
        /// SUMMARY:
        /// Vacía completamente el carrito eliminando todos los productos.
        /// </summary>
        public void VaciarCarrito()
        {
            items.Clear();
        }

        /// <summary>
        /// SUMMARY:
        /// Vacía el carrito y retorna una lista con los productos que estaban dentro,
        /// incluyendo sus cantidades, para permitir la reposición de inventario.
        /// </summary>
        /// <returns>Lista de productos que estaban en el carrito antes de vaciarlo.</returns>
        public List<Producto> VaciarYRetornarItems()
        {
            var copia = ObtenerProductos();
            items.Clear();
            return copia;
        }

        /// <summary>
        /// SUMMARY:
        /// Calcula el total a pagar del carrito multiplicando el precio por la cantidad
        /// de cada producto y sumando los resultados.
        /// </summary>
        /// <returns>Total a pagar por el contenido del carrito.</returns>
        public decimal ObtenerTotal()
        {
            return items.Sum(p => (decimal)p.Precio * p.Cantidad);
        }
    }
}
