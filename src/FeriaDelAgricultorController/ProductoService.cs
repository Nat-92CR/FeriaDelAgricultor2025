using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio encargado de administrar los productos disponibles en la feria.
    /// </summary>
    public class ProductoService
    {
        private readonly List<Producto> productos;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ProductoService"/>.
        /// Carga una lista de productos de ejemplo.
        /// </summary>
        public ProductoService()
        {
            this.productos = CrearProductosEjemplo();
        }

        /// <summary>
        /// Obtiene todos los productos disponibles.
        /// </summary>
        /// <returns>Lista con todos los productos.</returns>
        public List<Producto> ObtenerTodos()
        {
            return this.productos;
        }

        /// <summary>
        /// Obtiene la lista de nombres de productores sin repetir.
        /// </summary>
        /// <returns>Lista de nombres de productores ordenada alfabéticamente.</returns>
        public List<string> ObtenerProductores()
        {
            return this.productos
                .Select(p => p.Productor)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(nombre => nombre)
                .ToList();
        }

        // ======================================================
        // (equivalente a ObtenerPorProductor)
        // ======================================================

        /// <summary>
        /// Obtiene todos los productos que pertenecen a un productor específico.
        /// Este método es usado por la vista.
        /// </summary>
        public List<Producto> ObtenerPorProductor(string nombreProductor)
        {
            if (string.IsNullOrWhiteSpace(nombreProductor))
            {
                return new List<Producto>();
            }

            return this.productos
                .Where(p => p.Productor.Equals(nombreProductor, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // ======================================================
        // Obtener un solo producto específico
        // ======================================================

        /// <summary>
        /// Obtiene un producto por PRODUCTOR y NOMBRE de producto.
        /// Este método lo usa la vista cuando el usuario da clic en "Agregar al carrito".
        /// </summary>
        public Producto ObtenerProducto(string nombreProductor, string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(nombreProductor) ||
                string.IsNullOrWhiteSpace(nombreProducto))
            {
                return null;
            }

            return this.productos
                .FirstOrDefault(p =>
                    p.Productor.Equals(nombreProductor, StringComparison.OrdinalIgnoreCase) &&
                    p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));
        }

        // ======================================================
        // lista de ejemplo
        // ======================================================

        /// <summary>
        /// Crea una lista de productos de ejemplo.
        /// Más adelante se podría reemplazar para cargar desde archivo o base de datos.
        /// </summary>
        /// <returns>Lista de productos de ejemplo.</returns>
        private static List<Producto> CrearProductosEjemplo()
        {
            return new List<Producto>
            {
                new() { NombreProducto = "Tomate",      Precio = 800,  Cantidad = 1, UnidadMedida = UnidadMedida.Kilogramos, Productor = "Edwin Solís" },
                new() { NombreProducto = "Cebolla",     Precio = 600,  Cantidad = 1, UnidadMedida = UnidadMedida.Kilogramos, Productor = "Edwin Solís" },
                new() { NombreProducto = "Lechuga",     Precio = 350,  Cantidad = 1, UnidadMedida = UnidadMedida.Unidades,   Productor = "Edwin Solís" },

                new() { NombreProducto = "Sandía",      Precio = 1200, Cantidad = 1, UnidadMedida = UnidadMedida.Unidades,   Productor = "Josué Ramírez" },
                new() { NombreProducto = "Papaya",      Precio = 900,  Cantidad = 1, UnidadMedida = UnidadMedida.Unidades,   Productor = "Josué Ramírez" },
                new() { NombreProducto = "Melón",       Precio = 1000, Cantidad = 1, UnidadMedida = UnidadMedida.Unidades,   Productor = "Josué Ramírez" },

                new() { NombreProducto = "Queso Turrialba", Precio = 2500, Cantidad = 1, UnidadMedida = UnidadMedida.Kilogramos, Productor = "Allison Vargas" },
                new() { NombreProducto = "Natilla",         Precio = 950,  Cantidad = 1, UnidadMedida = UnidadMedida.Litros,     Productor = "Allison Vargas" },

                new() { NombreProducto = "Café molido",  Precio = 2500, Cantidad = 1, UnidadMedida = UnidadMedida.Kilogramos, Productor = "Mario Jiménez" },
                new() { NombreProducto = "Pan casero",   Precio = 700,  Cantidad = 1, UnidadMedida = UnidadMedida.Unidades,   Productor = "Mario Jiménez" },

                new() { NombreProducto = "Fresas",       Precio = 1500, Cantidad = 1, UnidadMedida = UnidadMedida.Kilogramos, Productor = "Gloria Rojas" },
                new() { NombreProducto = "Moras",        Precio = 1800, Cantidad = 1, UnidadMedida = UnidadMedida.Kilogramos, Productor = "Gloria Rojas" },
            };
        }
    }
}
