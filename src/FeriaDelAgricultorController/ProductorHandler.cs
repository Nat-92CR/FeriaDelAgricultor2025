using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Clase encargada de leer y cargar productores desde el archivo CSV.
    /// </summary>
    public class ProductorHandler
    {
        /// <summary>
        /// Nombre del archivo CSV que contiene los productores.
        /// </summary>
        private const string NombreArchivo = "Productores.csv";

        /// <summary>
        /// Carga los productores desde el archivo CSV ubicado en la carpeta
        /// bin\Debug\net9.0-windows (misma ruta que el ejecutable).
        /// </summary>
        /// <returns>Lista de productores cargados desde el archivo.</returns>
        public List<Productor> CargarProductores()
        {
            var productores = new List<Productor>();

            // Ruta completa al CSV (sin subcarpeta "Data" en tu proyecto actual).
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NombreArchivo);

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException($"No se encontró el archivo CSV en: {ruta}");
            }

            var lineas = File.ReadAllLines(ruta)
                             .Skip(1) // saltar encabezado
                             .Where(l => !string.IsNullOrWhiteSpace(l));

            foreach (var linea in lineas)
            {
                var partes = linea.Split(';');

                // Esperamos 5 columnas: Id;Nombre;Ubicacion;Telefono;PuntoFeriaId
                if (partes.Length < 5)
                {
                    continue;
                }

                if (!int.TryParse(partes[0], out int id))
                {
                    continue;
                }

                string nombre = partes[1].Trim();
                string ubicacion = partes[2].Trim();
                string telefono = partes[3].Trim();

                int puntoFeriaId = 0;
                int.TryParse(partes[4], out puntoFeriaId);

                var productor = new Productor(
                    id,
                    nombre,
                    ubicacion,
                    telefono,
                    puntoFeriaId);

                productores.Add(productor);
            }

            return productores;
        }
    }
}
