using System;
using System.Collections.Generic;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa una compra realizada por un cliente en la Feria del Agricultor.
    /// </summary>
    public class Factura
    {
        /// <summary>
        /// Identificador único de la compra.
        /// </summary>
        public int Id => this.GetHashCode();

        /// <summary>
        /// Usuario que realizó la compra.
        /// </summary>
        public Usuario Cliente { get; set; } = new Usuario("", "", "", "", "");

        /// <summary>
        /// Lista de productos incluidos en la compra.
        /// </summary>
        public List<Producto> Productos { get; set; } = new List<Producto>();

        /// <summary>
        /// Dirección de entrega seleccionada por el cliente.
        /// </summary>
        public Direccion Direccion { get; set; } = new Direccion();

        /// <summary>
        /// Fecha en que se realizó la compra.
        /// </summary>
        public DateTime Fecha { get; set; } = DateTime.Now;

        /// <summary>
        /// Método de pago utilizado por el cliente.
        /// </summary>
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Efectivo;

        /// <summary>
        /// Porcentaje de impuesto aplicado a la compra.
        /// </summary>
        public int PorcentajeImpuesto { get; set; } = 13;

        /// <summary>
        /// Calcula el monto total de la compra sumando precio * cantidad.
        /// </summary>
        /// <returns>Total de la compra sin aplicar redondeos adicionales.</returns>
        public decimal ObtenerTotal()
        {
            decimal total = 0m;

            foreach (var producto in this.Productos)
            {
                total += producto.Precio * producto.Cantidad;
            }

            return total;
        }
    }
}
