namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa los métodos de pago disponibles en el sistema.
    /// </summary>
    public enum MetodoPago
    {
        /// <summary>
        /// Pago mediante Sinpe Móvil.
        /// </summary>
        Sinpe,

        /// <summary>
        /// Pago en efectivo.
        /// </summary>
        Efectivo,

        /// <summary>
        /// Pago con tarjeta de crédito o débito.
        /// </summary>
        Tarjeta
    }
}
