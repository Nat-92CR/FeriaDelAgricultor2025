using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Clase encargada de leer y cargar productores desde el archivo CSV.
    /// </summary>
    public class ProductorHandler
    {
        private const string NombreArchivo = "Productores.csv";

        /// <summary>
        /// Carga los productores desde el archivo CSV ubicado en la carpeta Data.
        /// </summary>
        public List<Productor> CargarProductores()
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", NombreArchivo);

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException($"No se encontró el archivo CSV en: {ruta}");
            }

            var lineas = File.ReadAllLines(ruta)
                             .Skip(1) // saltar encabezado
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .ToList();

            var productores = new List<Productor>();

            foreach (var linea in lineas)
            {
                var partes = linea.Split(';');

                if (partes.Length < 4)
                    continue;

                productores.Add(new Productor
                {
                    Id = int.Parse(partes[0]),
                    Nombre = partes[1],
                    Ubicacion = partes[2],
                    Telefono = partes[3]
                });
            }

            return productores;
        }
    }
}
