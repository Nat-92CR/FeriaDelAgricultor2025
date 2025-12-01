using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio para manejo de los puntos de feria del agricultor.
    /// Permite obtener provincias, cantones y ferias según ubicación.
    /// </summary>
    public class PuntoFeriaService
    {
        private readonly List<PuntoFeria> puntosFeria;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PuntoFeriaService"/>.
        /// </summary>
        /// <param name="rutaArchivo">Ruta al archivo CSV de puntos de feria.</param>
        public PuntoFeriaService(string rutaArchivo)
        {
            puntosFeria = CargarPuntosFeriaDesdeCsv(rutaArchivo);
        }

        /// <summary>
        /// Obtiene la lista completa de puntos de feria.
        /// </summary>
        public IReadOnlyList<PuntoFeria> ObtenerTodos()
        {
            return puntosFeria;
        }

        /// <summary>
        /// Obtiene la lista de provincias disponibles.
        /// </summary>
        public List<string> ObtenerProvincias()
        {
            return puntosFeria
                .Select(p => p.Provincia)
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

        /// <summary>
        /// Obtiene los cantones para una provincia dada.
        /// </summary>
        public List<string> ObtenerCantonesPorProvincia(string provincia)
        {
            return puntosFeria
                .Where(p => p.Provincia.Equals(provincia, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Canton)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        /// <summary>
        /// Obtiene los puntos de feria para una provincia y cantón específicos.
        /// </summary>
        public List<PuntoFeria> ObtenerPuntosPorProvinciaYCanton(string provincia, string canton)
        {
            return puntosFeria
                .Where(p =>
                    p.Provincia.Equals(provincia, StringComparison.OrdinalIgnoreCase) &&
                    p.Canton.Equals(canton, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Nombre)
                .ToList();
        }

        /// <summary>
        /// Carga los puntos de feria desde un archivo CSV.
        /// Si el archivo no existe o está vacío, se cargan datos por defecto.
        /// </summary>
        private static List<PuntoFeria> CargarPuntosFeriaDesdeCsv(string rutaArchivo)
        {
            var lista = new List<PuntoFeria>();

            try
            {
                if (!string.IsNullOrWhiteSpace(rutaArchivo) && File.Exists(rutaArchivo))
                {
                    // Saltamos encabezado
                    var lineas = File.ReadAllLines(rutaArchivo).Skip(1);

                    foreach (var linea in lineas)
                    {
                        if (string.IsNullOrWhiteSpace(linea))
                        {
                            continue;
                        }

                        var columnas = linea.Split(',');

                        // Id,Nombre,Provincia,Canton,DireccionExacta
                        if (columnas.Length < 5)
                        {
                            continue;
                        }

                        if (!int.TryParse(columnas[0], out int id))
                        {
                            continue;
                        }

                        var punto = new PuntoFeria
                        {
                            Id = id,
                            Nombre = columnas[1].Trim(),
                            Provincia = columnas[2].Trim(),
                            Canton = columnas[3].Trim(),
                            DireccionExacta = columnas[4].Trim(),
                        };

                        lista.Add(punto);
                    }
                }
            }
            catch
            {
                // En caso de error al leer el archivo, continuamos con la lista por defecto.
            }

            // Si no se pudo cargar nada del CSV, usamos datos por defecto.
            if (!lista.Any())
            {
                lista.AddRange(new[]
                {
                    new PuntoFeria
                    {
                        Id = 1,
                        Nombre = "Feria Central San José",
                        Provincia = "San José",
                        Canton = "San José",
                        DireccionExacta = "Avenida Central, 50 m norte del parque"
                    },
                    new PuntoFeria
                    {
                        Id = 2,
                        Nombre = "Feria de Guadalupe",
                        Provincia = "San José",
                        Canton = "Goicoechea",
                        DireccionExacta = "Contiguo a la iglesia de Guadalupe"
                    },
                    new PuntoFeria
                    {
                        Id = 3,
                        Nombre = "Feria de Alajuela",
                        Provincia = "Alajuela",
                        Canton = "Alajuela",
                        DireccionExacta = "Costado oeste del estadio"
                    },
                    new PuntoFeria
                    {
                        Id = 4,
                        Nombre = "Feria de Heredia",
                        Provincia = "Heredia",
                        Canton = "Heredia",
                        DireccionExacta = "Frente al parque central"
                    },
                    new PuntoFeria
                    {
                        Id = 5,
                        Nombre = "Feria de Cartago",
                        Provincia = "Cartago",
                        Canton = "Cartago",
                        DireccionExacta = "Cerca de la Basílica de los Ángeles"
                    }
                });
            }

            return lista;
        }
    }
}
