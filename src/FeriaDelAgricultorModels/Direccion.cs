namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa la dirección de entrega del usuario.
    /// </summary>
    public class Direccion
    {
        /// <summary>
        /// Provincia.
        /// </summary>
        public string Province { get; set; } = string.Empty;

        /// <summary>
        /// Cantón.
        /// </summary>
        public string Canton { get; set; } = string.Empty;

        /// <summary>
        /// Distrito.
        /// </summary>
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// Señales / otras referencias.
        /// </summary>
        public string OtherDetails { get; set; } = string.Empty;

        /// <summary>
        /// Indica si es la dirección principal.
        /// </summary>
        public bool IsPrincipal { get; set; } = false;
    }
}
