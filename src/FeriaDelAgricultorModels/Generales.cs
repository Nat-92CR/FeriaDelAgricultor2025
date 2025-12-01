using System.IO;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Clase estática que contiene configuraciones y rutas
    /// generales utilizadas en toda la aplicación.
    /// </summary>
    public static class Generales
    {
        /// <summary>
        /// Nombre (y ruta) del archivo donde se almacenan
        /// los usuarios registrados en el sistema.
        /// </summary>
        public static readonly string FileNameUsers =
            Path.Combine(Directory.GetCurrentDirectory(), "Usuario.csv");

        /// <summary>
        /// Nombre (y ruta) del archivo donde se almacenan
        /// los productos disponibles en la feria.
        /// </summary>
        public static readonly string FileNameProductos =
            Path.Combine(Directory.GetCurrentDirectory(), "Productos.csv");

        /// <summary>
        /// Nombre (y ruta) del archivo donde se almacenan
        /// los productores registrados en el sistema.
        /// </summary>
        public static readonly string FileNameProductores =
            Path.Combine(Directory.GetCurrentDirectory(), "Productores.csv");
    }
}
