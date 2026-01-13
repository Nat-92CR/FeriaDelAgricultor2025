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
        private List<Productor> productores;

        /// <summary>
        /// Constructor: carga la lista de productores en memoria.
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
        /// Devuelve productores filtrados por PuntoFeriaId.
        /// </summary>
        public List<Productor> ObtenerPorPuntoFeria(int puntoFeriaId)
        {
            return productores
                .Where(p => p.PuntoFeriaId == puntoFeriaId)
                .ToList();
        }

        private List<Productor> CargarProductores()
        {
            var lista = new List<Productor>();

            try
            {
                var ruta = Generales.FileNameProductores;

                if (!File.Exists(ruta))
                    return lista;

                var lineas = File.ReadAllLines(ruta);

                // Saltar encabezado
                for (int i = 1; i < lineas.Length; i++)
                {
                    var linea = lineas[i];
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    var partes = linea.Split(';');
                    if (partes.Length < 6) continue;

                    if (!int.TryParse(partes[0], out int id))
                        continue;

                    string nombrePuesto = partes[1];
                    string ubicacion = partes[2];
                    string telefono = partes[3];

                    if (!int.TryParse(partes[4], out int puntoFeriaId))
                        continue;

                    string dueno = partes[5];

                    lista.Add(new Productor(id, nombrePuesto, ubicacion, telefono, puntoFeriaId, dueno));
                }
            }
            catch
            {
                // si falla carga, devolvemos lista vacía
                return new List<Productor>();
            }

            return lista;
        }

        // ==========================
        // ✅ CRUD PARA ADMIN (CSV)
        // ==========================

        public bool GuardarTodos(List<Productor> lista)
        {
            try
            {
                var ruta = Generales.FileNameProductores;

                var encabezado = "Id;NombrePuesto;Ubicacion;Telefono;PuntoFeriaId;Dueno";
                var lineas = new List<string> { encabezado };

                foreach (var p in lista.OrderBy(x => x.Id))
                {
                    lineas.Add($"{p.Id};{p.NombrePuesto};{p.Ubicacion};{p.Telefono};{p.PuntoFeriaId};{p.Dueno}");
                }

                File.WriteAllLines(ruta, lineas);
                productores = lista; // refrescar cache
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Crear(Productor nuevo)
        {
            var lista = productores.ToList();

            if (nuevo.Id <= 0)
            {
                var next = (lista.Count == 0) ? 1 : lista.Max(x => x.Id) + 1;
                nuevo.Id = next;
            }

            if (lista.Any(x => x.Id == nuevo.Id)) return false;

            lista.Add(nuevo);
            return GuardarTodos(lista);
        }

        public bool Actualizar(Productor actualizado)
        {
            var lista = productores.ToList();
            var idx = lista.FindIndex(x => x.Id == actualizado.Id);
            if (idx < 0) return false;

            lista[idx] = actualizado;
            return GuardarTodos(lista);
        }

        public bool Eliminar(int id)
        {
            var lista = productores.ToList();
            var obj = lista.FirstOrDefault(x => x.Id == id);
            if (obj == null) return false;

            lista.Remove(obj);
            return GuardarTodos(lista);
        }
    }
}
