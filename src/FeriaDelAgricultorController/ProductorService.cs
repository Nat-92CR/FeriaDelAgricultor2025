using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio encargado de leer la información de los productores
    /// desde el archivo CSV Productores.csv y exponer consultas.
    ///
    /// Formato esperado del CSV:
    /// Id;NombrePuesto;Ubicacion;Telefono;PuntoFeriaId;Dueno
    /// </summary>
    public class ProductorService
    {
        private const string NombreArchivo = "Productores.csv";

        /// <summary>Lista interna con todos los productores cargados.</summary>
        private readonly List<Productor> productores;

        /// <summary>
        /// Constructor. Al instanciar el servicio se cargan los productores
        /// desde el archivo CSV ubicado en la carpeta Data del ejecutable.
        /// </summary>
        public ProductorService()
        {
            productores = CargarProductores();
        }

        /// <summary>
        /// Devuelve todos los productores cargados.
        /// </summary>
        public IReadOnlyList<Productor> ObtenerTodos()
        {
            return productores;
        }

        /// <summary>
        /// Devuelve los productores que pertenecen a un punto de feria específico.
        /// </summary>
        /// <param name="puntoFeriaId">Id del punto de feria.</param>
        public List<Productor> ObtenerPorPuntoFeria(int puntoFeriaId)
        {
            return productores
                .Where(p => p.PuntoFeriaId == puntoFeriaId)
                .ToList();
        }

        /// <summary>
        /// Lee el archivo CSV Productores.csv (ubicado en la carpeta Data)
        /// y construye la lista de productores.
        /// </summary>
        private static List<Productor> CargarProductores()
        {
            var lista = new List<Productor>();

            // Ruta: [carpeta del exe]\Data\Productores.csv
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

                // 0 Id
                // 1 NombrePuesto
                // 2 Ubicacion
                // 3 Telefono
                // 4 PuntoFeriaId
                // 5 Dueno
                if (partes.Length < 6)
                    continue;

                if (!int.TryParse(partes[0], out int id))
                    continue;

                string nombrePuesto = partes[1].Trim();
                string ubicacion = partes[2].Trim();
                string telefono = partes[3].Trim();

                if (!int.TryParse(partes[4], out int puntoFeriaId))
                    continue;

                string dueno = partes[5].Trim();

                var productor = new Productor(
                    id,
                    nombrePuesto,
                    ubicacion,
                    telefono,
                    puntoFeriaId,
                    dueno);

                lista.Add(productor);
            }

            return lista;
        }
    }
}
