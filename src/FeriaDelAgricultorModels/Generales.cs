using System;
using System.IO;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Clase estática que centraliza rutas y configuraciones generales utilizadas por la aplicación.
    /// Nota: Se usa AppDomain.CurrentDomain.BaseDirectory para que funcione igual en WinForms, Consola y Blazor,
    /// siempre que los CSV se copien al output en una carpeta "Data".
    /// </summary>
    public static class Generales
    {
        private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string FileNameUsers =
            Path.Combine(BasePath, "Data", "Usuario.csv");

        public static readonly string FileNameProductos =
            Path.Combine(BasePath, "Data", "Productos.csv");

        public static readonly string FileNameProductores =
            Path.Combine(BasePath, "Data", "Productores.csv");

        public static readonly string FileNamePuntosFeria =
            Path.Combine(BasePath, "Data", "PuntosFeria.csv");

        public static readonly string FileNameFacturas =
            Path.Combine(BasePath, "Data", "Facturas.csv");
    }
}
