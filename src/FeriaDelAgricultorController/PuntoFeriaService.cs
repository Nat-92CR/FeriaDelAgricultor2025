using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    public class PuntoFeriaService
    {
        private readonly List<PuntoFeria> puntosFeria;

        public PuntoFeriaService(string rutaCsv = null)
        {
            // ✅ Ruta por defecto: [BaseDirectory]\Data\PuntosFeria.csv
            if (string.IsNullOrWhiteSpace(rutaCsv))
            {
                rutaCsv = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Data",
                    "PuntosFeria.csv"
                );
            }

            puntosFeria = CargarPuntosFeriaDesdeCsv(rutaCsv);

            // ✅ Si no se pudo cargar, usar datos por defecto (tu fallback actual)
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

        private static List<PuntoFeria> CargarPuntosFeriaDesdeCsv(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                return new List<PuntoFeria>();
            }

            var lineas = File.ReadAllLines(rutaArchivo)
                             .Skip(1)
                             .Where(l => !string.IsNullOrWhiteSpace(l))
                             .ToList();

            var lista = new List<PuntoFeria>();

            foreach (var linea in lineas)
            {
                // ✅ Soporta ';' y también ',' por compatibilidad
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

        public List<string> ObtenerProvincias()
        {
            return puntosFeria
                .Select(p => p.Provincia)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();
        }

        public List<string> ObtenerCantonesPorProvincia(string provincia)
        {
            if (string.IsNullOrWhiteSpace(provincia)) return new List<string>();

            return puntosFeria
                .Where(p => p.Provincia.Equals(provincia, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Canton)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();
        }

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
