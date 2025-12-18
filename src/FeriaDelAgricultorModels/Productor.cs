using System;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa un productor / puesto dentro de una feria.
    /// Incluye:
    /// - Nombre del puesto
    /// - Ubicación
    /// - Teléfono
    /// - Id del punto de feria
    /// - Nombre del dueño
    /// </summary>
    public class Productor
    {
        /// <summary>Identificador del productor (Id en Productores.csv).</summary>
        public int Id { get; set; }

        /// <summary>Nombre del puesto de feria (Agrícola San Pedro, etc.).</summary>
        public string NombrePuesto { get; set; }

        /// <summary>Ubicación o provincia asociada.</summary>
        public string Ubicacion { get; set; }

        /// <summary>Teléfono de contacto.</summary>
        public string Telefono { get; set; }

        /// <summary>Id del punto de feria al que pertenece.</summary>
        public int PuntoFeriaId { get; set; }

        /// <summary>Nombre completo del dueño del puesto.</summary>
        public string Dueno { get; set; }

        /// <summary>
        /// Constructor por defecto (útil para serialización o binding).
        /// </summary>
        public Productor()
        {
        }

        /// <summary>
        /// Constructor principal que recibe todos los datos del productor.
        /// </summary>
        public Productor(
            int id,
            string nombrePuesto,
            string ubicacion,
            string telefono,
            int puntoFeriaId,
            string dueno)
        {
            Id = id;
            NombrePuesto = nombrePuesto;
            Ubicacion = ubicacion;
            Telefono = telefono;
            PuntoFeriaId = puntoFeriaId;
            Dueno = dueno;
        }

        /// <summary>
        /// Constructor de compatibilidad (antiguo): sin PuntoFeriaId ni Dueno.
        /// </summary>
        public Productor(int id, string nombrePuesto, string ubicacion, string telefono)
            : this(id, nombrePuesto, ubicacion, telefono, 0, string.Empty)
        {
        }

        /// <summary>
        /// Constructor intermedio: asigna PuntoFeriaId y deja Dueno vacío.
        /// </summary>
        public Productor(int id, string nombrePuesto, string ubicacion, string telefono, int puntoFeriaId)
            : this(id, nombrePuesto, ubicacion, telefono, puntoFeriaId, string.Empty)
        {
        }
    }
}
