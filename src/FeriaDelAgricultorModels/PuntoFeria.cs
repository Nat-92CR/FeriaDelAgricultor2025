namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa un punto de feria del agricultor.
    /// </summary>
    public class PuntoFeria
    {
        /// <summary>
        /// Identificador único del punto de feria.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre descriptivo de la feria.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Provincia donde se ubica la feria.
        /// </summary>
        public string Provincia { get; set; } = string.Empty;

        /// <summary>
        /// Cantón donde se ubica la feria.
        /// </summary>
        public string Canton { get; set; } = string.Empty;

        /// <summary>
        /// Dirección más detallada o referencia.
        /// </summary>
        public string DireccionExacta { get; set; } = string.Empty;

        /// <summary>
        /// Devuelve una representación amigable del punto de feria.
        /// </summary>
        public override string ToString()
        {
            return $"{Nombre} - {Canton}, {Provincia}";
        }
    }
}
