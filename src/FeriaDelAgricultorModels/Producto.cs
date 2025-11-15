using System;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa un producto disponible en la Feria del Agricultor.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public int Id => this.GetHashCode();

        /// <summary>
        /// Nombre del producto (por ejemplo: Tomate, Papa, Lechuga).
        /// </summary>
        public string NombreProducto { get; set; } = string.Empty;

        /// <summary>
        /// Precio unitario del producto.
        /// </summary>
        public decimal Precio { get; set; } = 0;

        /// <summary>
        /// Cantidad disponible o seleccionada del producto.
        /// </summary>
        public int Cantidad { get; set; } = 0;

        /// <summary>
        /// Unidad de medida del producto (kilogramos, unidades, etc.).
        /// </summary>
        public UnidadMedida UnidadMedida { get; set; } = UnidadMedida.Unidades;

        /// <summary>
        /// Nombre del productor que ofrece este producto.
        /// </summary>
        public string Productor { get; set; } = string.Empty;
    }
}
