using FeriaDelAgricultorModels;
using System.Collections.Generic;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio encargado de manejar la lógica de productores.
    /// </summary>
    public class ProductorService
    {
        private readonly ProductorHandler handler = new ProductorHandler();

        /// <summary>
        /// Retorna todos los productores del archivo CSV.
        /// </summary>
        public List<Productor> ObtenerProductores()
        {
            return handler.CargarProductores();
        }
    }
}
