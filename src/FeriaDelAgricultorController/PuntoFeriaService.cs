using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Proporciona operaciones para cargar, consultar y filtrar los puntos de feria,
    /// a partir de un archivo CSV o, en su defecto, mediante datos de respaldo.
    /// </summary>
    public class PuntoFeriaService
    {
        /// <summary>
        /// Almacena en memoria la lista de puntos de feria cargados desde el CSV o desde el respaldo.
        /// </summary>
        private readonly List<PuntoFeria> puntosFeria;

        /// <summary>
        /// Inicializa el servicio y carga los puntos de feria desde un archivo CSV.
        /// Si no se proporciona ruta, se utiliza la ruta por defecto [BaseDirectory]\Data\PuntosFeria.csv.
        /// Si no se logra cargar información, se emplean datos de respaldo.
        /// </summary>
        /// <param name="rutaCsv">Representa la ruta del archivo CSV a utilizar. Si es nula o vacía, se usa la ruta por defecto.</param>
        public PuntoFeriaService(string rutaCsv = null)
        {
            // Ruta por defecto: [BaseDirectory]\Data\PuntosFeria.csv
            if (string.IsNullOrWhiteSpace(rutaCsv))
            {
                rutaCsv = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Data",
                    "PuntosFeria.csv"
                );
            }

            puntosFeria = CargarPuntosFeriaDesdeCsv(rutaCsv);

            // Si no se pudo cargar, se usan datos de respaldo.
            if (puntosFeria == null || puntosFeria.Count == 0)
            {
                puntosFeria = new List<PuntoFeria>
                {
                    new PuntoFeria { Id = 1, Nombre = "Feria Central San José", Provincia = "San José", Canton = "San José", DireccionExacta = "Avenida Central, 50 m norte del parque" },
                    new PuntoFeria { Id = 2, Nombre = "Feria de Guadalupe", Provincia = "San José", Canton = "Goicoechea", DireccionExacta = "Contiguo a la iglesia de Guadalupe" },
                    new PuntoFeria { Id = 3, Nombre = "Feria de Alajuela", Provincia = "Alajuela", Canton = "Alajuela", DireccionExacta = "Costado oeste del estadio" },
                    new PuntoFeria { Id = 4, Nombre = "Feria de Heredia", Provincia = "Heredia", Canton = "Heredia", DireccionExacta = "Frente al parque central" },
                    new PuntoFeria { Id = 5, Nombre = "Feria de Cartago", Provincia = "Cartago", Canton = "Cartago", DireccionExacta = "Cerca de la Basílica de los Ángeles" }
                };
            }
        }

        /// <summary>
        /// Carga los puntos de feria desde un archivo CSV.
        /// El método admite separador ';' o ',' para compatibilidad con diferentes formatos.
        /// </summary>
        /// <param name="rutaArchivo">Representa la ruta completa del archivo CSV.</param>
        /// <returns>Retorna una lista de puntos de feria; si el archivo no existe o no se puede leer, retorna una lista vacía.</returns>
        private static List<PuntoFeria> CargarPuntosFeriaDesdeCsv(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                return new List<PuntoFeria>();
            }

            var lineas = File.ReadAllLines(rutaArchivo)
                             .Skip(1) // Se omite encabezado
                             .Where(l => !string.IsNullOrWhiteSpace(l))
                             .ToList();

            var lista = new List<PuntoFeria>();

            foreach (var linea in lineas)
            {
                // Soporta ';' y también ',' por compatibilidad
                var partes = linea.Contains(";")
                    ? linea.Split(';')
                    : linea.Split(',');

                if (partes.Length < 5) continue;

                if (!int.TryParse(partes[0], out int id)) continue;

                var nombre = partes[1].Trim();
                var provincia = partes[2].Trim();
                var canton = partes[3].Trim();
                var direccion = partes[4].Trim();

                lista.Add(new PuntoFeria
                {
                    Id = id,
                    Nombre = nombre,
                    Provincia = provincia,
                    Canton = canton,
                    DireccionExacta = direccion
                });
            }

            return lista;
        }

        /// <summary>
        /// Obtiene el listado de provincias disponibles en los puntos de feria cargados.
        /// </summary>
        /// <returns>Retorna una lista ordenada de provincias sin duplicados.</returns>
        public List<string> ObtenerProvincias()
        {
            return puntosFeria
                .Select(p => p.Provincia)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        /// Obtiene el listado de cantones para una provincia específica.
        /// </summary>
        /// <param name="provincia">Representa la provincia seleccionada.</param>
        /// <returns>Retorna una lista ordenada de cantones sin duplicados; si la provincia es nula o vacía, retorna una lista vacía.</returns>
        public List<string> ObtenerCantonesPorProvincia(string provincia)
        {
            if (string.IsNullOrWhiteSpace(provincia))
                return new List<string>();

            return puntosFeria
                .Where(p => p.Provincia.Equals(provincia, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Canton)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        /// Obtiene el listado de puntos de feria filtrados por provincia y cantón.
        /// </summary>
        /// <param name="provincia">Representa la provincia seleccionada.</param>
        /// <param name="canton">Representa el cantón seleccionado.</param>
        /// <returns>Retorna una lista ordenada por nombre; si los parámetros son inválidos, retorna una lista vacía.</returns>
        public List<PuntoFeria> ObtenerPuntosPorProvinciaYCanton(string provincia, string canton)
        {
            if (string.IsNullOrWhiteSpace(provincia) || string.IsNullOrWhiteSpace(canton))
                return new List<PuntoFeria>();

            return puntosFeria
                .Where(p =>
                    p.Provincia.Equals(provincia, StringComparison.OrdinalIgnoreCase) &&
                    p.Canton.Equals(canton, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Nombre)
                .ToList();
        }
    }
}
