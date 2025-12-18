using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio encargado de cargar y proveer productos disponibles.
    ///
    /// Lee desde: [BaseDirectory]\Data\Productos.csv
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
        /// Lista interna con todos los productos cargados desde CSV.
        /// </summary>
        private readonly List<Producto> productos;

        /// <summary>
        /// Constructor del servicio. Al instanciar, carga Productos.csv.
        /// </summary>
        public ProductoService()
        {
            productos = CargarProductos();
        }

        /// <summary>
        /// Devuelve todos los productos cargados.
        /// </summary>
        public List<Producto> ObtenerTodos()
        {
            // Devolvemos copia para no exponer la lista interna
            return productos.ToList();
        }

        /// <summary>
        /// Devuelve la lista de productores (puestos) únicos, tomados del campo Productor del CSV.
        /// </summary>
        public List<string> ObtenerProductores()
        {
            return productos
                .Where(p => !string.IsNullOrWhiteSpace(p.Productor))
                .Select(p => p.Productor.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        /// Devuelve todos los productos que pertenecen a un puesto específico.
        /// </summary>
        /// <param name="nombreProductor">Nombre del puesto (NombrePuesto).</param>
        public List<Producto> ObtenerPorProductor(string nombreProductor)
        {
            if (string.IsNullOrWhiteSpace(nombreProductor))
                return new List<Producto>();

            return productos
                .Where(p => p.Productor != null &&
                            p.Productor.Equals(nombreProductor, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Busca un producto específico por puesto y nombre de producto.
        /// </summary>
        public Producto ObtenerProducto(string nombreProductor, string nombreProducto)
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
        /// Lee Productos.csv desde la carpeta Data del ejecutable y construye la lista.
        /// Si el archivo no existe, devuelve lista vacía.
        /// </summary>
        private static List<Producto> CargarProductos()
        {
            var lista = new List<Producto>();

            // Ruta: [carpeta del exe]\Data\Productos.csv
            string ruta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                NombreArchivo);

            if (!File.Exists(ruta))
            {
                return lista;
            }

            var lineas = File.ReadAllLines(ruta)
                             .Skip(1) // omitir encabezado
                             .Where(l => !string.IsNullOrWhiteSpace(l));

            foreach (var linea in lineas)
            {
                var partes = linea.Split(';');

                // Productor;NombreProducto;Precio;Cantidad;UnidadMedida
                if (partes.Length < 5)
                    continue;

                string puestoNombre = partes[0].Trim();
                string nombreProducto = partes[1].Trim();

                // Precio: intentamos InvariantCulture y luego cultura local.
                if (!decimal.TryParse(partes[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
                {
                    if (!decimal.TryParse(partes[2], NumberStyles.Any, CultureInfo.CurrentCulture, out precio))
                        continue;
                }

                if (!int.TryParse(partes[3], NumberStyles.Any, CultureInfo.InvariantCulture, out int cantidad))
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

        /// <summary>
        /// Convierte el texto de UnidadMedida del CSV a enum.
        /// </summary>
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
    }
}
