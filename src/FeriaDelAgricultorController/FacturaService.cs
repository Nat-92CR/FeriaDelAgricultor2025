using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Servicio responsable de crear facturas.
    /// Cumple con SRP: solo se encarga de la lógica de facturación.
    /// </summary>
    public class FacturaService
    {
        /// <summary>
        /// Crea una factura de ejemplo para un cliente.
        /// Más adelante se puede reemplazar por el carrito real.
        /// </summary>
        /// <param name="cliente">Usuario que realiza la compra.</param>
        /// <returns>Instancia de <see cref="Factura"/> lista para mostrarse.</returns>
        public Factura CrearFacturaEjemplo(Usuario cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente));
            }

            //Aquí está la lista de productos que antes tenías en el botón
            var productos = new List<Producto>()
            {
                new Producto
                {
                    UnidadMedida = UnidadMedida.Unidades,
                    Precio = 10m,
                    NombreProducto = "Sandías",
                    Productor = "Josué",
                    Cantidad = 2,
                },
                new Producto
                {
                    UnidadMedida = UnidadMedida.Kilogramos,
                    Precio = 1000m,
                    NombreProducto = "Tomate",
                    Productor = "Edwin",
                    Cantidad = 4,
                },
                new Producto
                {
                    UnidadMedida = UnidadMedida.Litros,
                    Precio = 1500m,
                    NombreProducto = "Fresco de frutas",
                    Productor = "Allison",
                    Cantidad = 2,
                },
            };

            //Creacion de la factura
            var factura = new Factura
            {
                Cliente = cliente,
                Fecha = DateTime.Now,
                MetodoPago = MetodoPago.Tarjeta,
                PorcentajeImpuesto = 13,
                Productos = productos,
                Direccion = new Direccion()
            };

            return factura;
        }
    }
}
