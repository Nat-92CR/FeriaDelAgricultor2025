using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa una factura generada a partir de los productos comprados por un cliente.
    /// Incluye subtotal, impuesto, descuento y total final.
    /// </summary>
    public class Factura
    {
        /// <summary>
        /// Identificador único de la factura.
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
        /// Dirección seleccionada por el cliente para la entrega (si aplica).
        /// </summary>
        public Direccion Direccion { get; set; } = new Direccion();

        /// <summary>
        /// Fecha en la que se realizó la compra.
        /// </summary>
        public DateTime Fecha { get; private set; } = DateTime.Now;

        /// <summary>
        /// Método de pago utilizado por el cliente.
        /// </summary>
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Efectivo;

        /// <summary>
        /// Porcentaje de impuesto aplicado (13% por defecto en Costa Rica).
        /// </summary>
        public int PorcentajeImpuesto { get; set; } = 13;

        /// <summary>
        /// Descuento aplicado a la compra (si viene desde el carrito).
        /// </summary>
        public decimal Descuento { get; set; } = 0m;

        // ============================================================
        // =============== MÉTODOS DE CÁLCULO =========================
        // ============================================================

        /// <summary>
        /// Calcula el subtotal sumando precio * cantidad de cada producto.
        /// No incluye impuestos ni descuentos.
        /// </summary>
        public decimal ObtenerSubtotal()
        {
            return this.Productos.Sum(p => p.Precio * p.Cantidad);
        }

        /// <summary>
        /// Calcula el subtotal después del descuento aplicado.
        /// </summary>
        public decimal ObtenerSubtotalConDescuento()
        {
            decimal subtotal = ObtenerSubtotal();

            if (Descuento < 0)
                Descuento = 0;

            if (Descuento > subtotal)
                Descuento = subtotal;

            return subtotal - Descuento;
        }

        /// <summary>
        /// Calcula el monto del impuesto.
        /// </summary>
        public decimal ObtenerImpuesto()
        {
            return Math.Round(
                ObtenerSubtotalConDescuento() * (PorcentajeImpuesto / 100m),
                2);
        }

        /// <summary>
        /// Calcula el total final de la factura.
        /// </summary>
        public decimal ObtenerTotal()
        {
            return ObtenerSubtotalConDescuento() + ObtenerImpuesto();
        }

        /// <summary>
        /// Texto con resumen breve de la factura.
        /// </summary>
        public override string ToString()
        {
            return $"Factura #{Id} - Cliente: {Cliente?.Username}, Total: ₡{ObtenerTotal():0}";
        }

        // ===============================================================
        // =============== DATOS DEL PUNTO DE FERIA (NUEVO) ===============
        // ===============================================================

        /// <summary>
        /// Identificador del punto de feria donde se realizará la compra/entrega.
        /// </summary>
        public int PuntoFeriaId { get; set; }

        /// <summary>
        /// Nombre o descripción del punto de feria (provincia, cantón, nombre).
        /// </summary>
        public string PuntoFeriaDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Dirección exacta del punto de feria (para mostrar en FacturaView).
        /// </summary>
        public string DireccionFeria { get; set; } = string.Empty;
    }
}
