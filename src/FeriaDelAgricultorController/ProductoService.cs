using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio encargado de cargar y administrar productos disponibles.
    ///
    /// Lee y escribe desde: [BaseDirectory]\Data\Productos.csv
    ///
    /// Formato esperado (separado por ';'):
    /// Productor;NombreProducto;Precio;Cantidad;UnidadMedida
    ///
    /// IMPORTANTE:
    /// - "Productor" en el CSV debe contener el NOMBRE DEL PUESTO (NombrePuesto),
    ///   para hacer match con Productor.NombrePuesto (de Productores.csv).
    /// - UnidadMedida admite: Unidades, Kilogramos, Litros (y también Kg/Kilos/Litro/Litros).
    /// </summary>
    public class ProductoService
    {
        private const string NombreArchivo = "Productos.csv";

        /// <summary>
        /// Ruta real del archivo CSV en ejecución (bin/.../Data/Productos.csv).
        /// </summary>
        private readonly string rutaArchivo;

        /// <summary>
        /// Lista interna con todos los productos cargados desde CSV.
        /// </summary>
        private readonly List<Producto> productos;

        /// <summary>
        /// Constructor del servicio. Al instanciar, carga Productos.csv.
        /// </summary>
        public ProductoService()
        {
            rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", NombreArchivo);
            productos = CargarProductos(rutaArchivo);
        }

        /// <summary>
        /// Devuelve todos los productos (copia).
        /// </summary>
        public List<Producto> ObtenerTodos()
        {
            return productos.Select(Copia).ToList();
        }

        /// <summary>
        /// Devuelve productos por productor/puesto (COPIAS para la UI).
        /// </summary>
        public List<Producto> ObtenerPorProductor(string nombreProductor)
        {
            if (string.IsNullOrWhiteSpace(nombreProductor))
                return new List<Producto>();

            return productos
                .Where(p => p.Productor != null &&
                            p.Productor.Equals(nombreProductor, StringComparison.OrdinalIgnoreCase))
                .Select(Copia)
                .ToList();
        }

        /// <summary>
        /// Obtiene un producto (COPIA) por productor + nombre.
        /// </summary>
        public Producto? ObtenerProducto(string nombreProductor, string nombreProducto)
        {
            var interno = ObtenerProductoInterno(nombreProductor, nombreProducto);
            return interno == null ? null : Copia(interno);
        }

        /// <summary>
        /// Obtiene referencia interna (NO copia) para operaciones de inventario.
        /// </summary>
        public Producto? ObtenerProductoInterno(string nombreProductor, string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(nombreProductor) || string.IsNullOrWhiteSpace(nombreProducto))
                return null;

            return productos.FirstOrDefault(p =>
                p.Productor != null &&
                p.NombreProducto != null &&
                p.Productor.Equals(nombreProductor, StringComparison.OrdinalIgnoreCase) &&
                p.NombreProducto.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Descuenta inventario si hay stock suficiente y persiste el CSV.
        /// </summary>
        public bool TryDescontarStock(string nombreProductor, string nombreProducto, int cantidad)
        {
            if (cantidad <= 0) cantidad = 1;

            var p = ObtenerProductoInterno(nombreProductor, nombreProducto);
            if (p == null) return false;

            if (p.Cantidad < cantidad) return false;

            p.Cantidad -= cantidad;
            GuardarProductos();
            return true;
        }

        /// <summary>
        /// Repone inventario (útil al eliminar/vaciar carrito) y persiste el CSV.
        /// </summary>
        public void ReponerStock(string nombreProductor, string nombreProducto, int cantidad)
        {
            if (cantidad <= 0) cantidad = 1;

            var p = ObtenerProductoInterno(nombreProductor, nombreProducto);
            if (p == null) return;

            p.Cantidad += cantidad;
            GuardarProductos();
        }

        /// <summary>
        /// Guarda Productos.csv con separador ';'.
        /// </summary>
        public void GuardarProductos()
        {
            try
            {
                var carpeta = Path.GetDirectoryName(rutaArchivo);
                if (!string.IsNullOrWhiteSpace(carpeta))
                    Directory.CreateDirectory(carpeta);

                var lineas = new List<string>
                {
                    "Productor;NombreProducto;Precio;Cantidad;UnidadMedida"
                };

                foreach (var p in productos)
                {
                    string precio = p.Precio.ToString(CultureInfo.InvariantCulture);
                    string unidad = p.UnidadMedida.ToString();

                    lineas.Add($"{p.Productor};{p.NombreProducto};{precio};{p.Cantidad};{unidad}");
                }

                File.WriteAllLines(rutaArchivo, lineas);
            }
            catch
            {
                // Ideal: logging, pero no rompemos la app.
            }
        }

        private static List<Producto> CargarProductos(string ruta)
        {
            var lista = new List<Producto>();

            if (!File.Exists(ruta))
                return lista;

            var lineas = File.ReadAllLines(ruta)
                             .Skip(1)
                             .Where(l => !string.IsNullOrWhiteSpace(l));

            foreach (var linea in lineas)
            {
                var partes = linea.Split(';');
                if (partes.Length < 5) continue;

                string puestoNombre = partes[0].Trim();
                string nombreProducto = partes[1].Trim();

                if (!decimal.TryParse(partes[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
                {
                    if (!decimal.TryParse(partes[2].Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out precio))
                        continue;
                }

                if (!int.TryParse(partes[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int cantidad))
                    continue;

                UnidadMedida unidad = ParseUnidad(partes[4]);

                lista.Add(new Producto
                {
                    Productor = puestoNombre,
                    NombreProducto = nombreProducto,
                    Precio = precio,
                    Cantidad = cantidad,
                    UnidadMedida = unidad
                });
            }

            return lista;
        }

        private static Producto Copia(Producto p)
        {
            return new Producto
            {
                Productor = p.Productor,
                NombreProducto = p.NombreProducto,
                Precio = p.Precio,
                Cantidad = p.Cantidad,
                UnidadMedida = p.UnidadMedida
            };
        }

        private static UnidadMedida ParseUnidad(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return UnidadMedida.Unidades;

            string t = texto.Trim().ToLowerInvariant();

            if (t is "unidad" or "unidades" or "u" or "und" or "unds")
                return UnidadMedida.Unidades;

            if (t is "kg" or "kilo" or "kilos" or "kilogramo" or "kilogramos")
                return UnidadMedida.Kilogramos;

            if (t is "l" or "lt" or "litro" or "litros")
                return UnidadMedida.Litros;

            if (Enum.TryParse(texto.Trim(), ignoreCase: true, out UnidadMedida unidad))
                return unidad;

            return UnidadMedida.Unidades;
        }

        // ==========================================================
        // ✅ CRUD PARA ADMIN (sin romper el flujo actual)
        // ==========================================================

        /// <summary>
        /// Agrega un producto nuevo. Evita duplicado por (Productor + NombreProducto).
        /// Persiste en CSV.
        /// </summary>
        public bool AgregarProducto(Producto nuevo)
        {
            if (nuevo == null) return false;

            if (string.IsNullOrWhiteSpace(nuevo.Productor) ||
                string.IsNullOrWhiteSpace(nuevo.NombreProducto))
                return false;

            string prod = nuevo.Productor.Trim();
            string nom = nuevo.NombreProducto.Trim();

            var existe = ObtenerProductoInterno(prod, nom);
            if (existe != null) return false;

            productos.Add(new Producto
            {
                Productor = prod,
                NombreProducto = nom,
                Precio = nuevo.Precio < 0 ? 0 : nuevo.Precio,
                Cantidad = nuevo.Cantidad < 0 ? 0 : nuevo.Cantidad,
                UnidadMedida = nuevo.UnidadMedida
            });

            GuardarProductos();
            return true;
        }

        /// <summary>
        /// Actualiza un producto existente por (Productor + NombreProducto).
        /// Persiste en CSV.
        /// </summary>
        public bool ActualizarProducto(Producto actualizado)
        {
            if (actualizado == null) return false;

            if (string.IsNullOrWhiteSpace(actualizado.Productor) ||
                string.IsNullOrWhiteSpace(actualizado.NombreProducto))
                return false;

            var p = ObtenerProductoInterno(actualizado.Productor.Trim(), actualizado.NombreProducto.Trim());
            if (p == null) return false;

            p.Precio = actualizado.Precio < 0 ? 0 : actualizado.Precio;
            p.Cantidad = actualizado.Cantidad < 0 ? 0 : actualizado.Cantidad;
            p.UnidadMedida = actualizado.UnidadMedida;

            GuardarProductos();
            return true;
        }

        /// <summary>
        /// Elimina un producto por (Productor + NombreProducto).
        /// Persiste en CSV.
        /// </summary>
        public bool EliminarProducto(string productor, string nombreProducto)
        {
            var p = ObtenerProductoInterno(productor?.Trim() ?? "", nombreProducto?.Trim() ?? "");
            if (p == null) return false;

            productos.Remove(p);
            GuardarProductos();
            return true;
        }

        /// <summary>
        /// ✅ Elimina TODOS los productos asociados a un productor/puesto.
        /// Devuelve cuántos eliminó (para mostrarlo en la UI).
        /// </summary>
        public int EliminarPorProductor(string nombreProductor)
        {
            if (string.IsNullOrWhiteSpace(nombreProductor))
                return 0;

            string prod = nombreProductor.Trim();

            int eliminados = productos.RemoveAll(p =>
                !string.IsNullOrWhiteSpace(p.Productor) &&
                p.Productor.Trim().Equals(prod, StringComparison.OrdinalIgnoreCase));

            if (eliminados > 0)
                GuardarProductos();

            return eliminados;
        }

        // Opcionales
        public bool CambiarStock(string productor, string nombreProducto, int nuevoStock)
        {
            var p = ObtenerProductoInterno(productor?.Trim() ?? "", nombreProducto?.Trim() ?? "");
            if (p == null) return false;

            p.Cantidad = nuevoStock < 0 ? 0 : nuevoStock;
            GuardarProductos();
            return true;
        }

        public bool CambiarPrecio(string productor, string nombreProducto, decimal nuevoPrecio)
        {
            var p = ObtenerProductoInterno(productor?.Trim() ?? "", nombreProducto?.Trim() ?? "");
            if (p == null) return false;

            p.Precio = nuevoPrecio < 0 ? 0 : nuevoPrecio;
            GuardarProductos();
            return true;
        }
    }
}
