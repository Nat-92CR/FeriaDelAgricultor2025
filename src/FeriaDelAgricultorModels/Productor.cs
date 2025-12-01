using System;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa un productor registrado en la feria.
    /// </summary>
    public class Productor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string Telefono { get; set; }

        public Productor() { }

        public Productor(int id, string nombre, string ubicacion, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Ubicacion = ubicacion;
            Telefono = telefono;
        }
    }
}
