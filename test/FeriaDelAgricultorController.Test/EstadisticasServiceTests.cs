using FeriaDelAgricultorController;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace FeriaDelAgricultorControllerTest
{
    /// <summary>
    /// Pruebas unitarias para <see cref="EstadisticasService"/>.
    ///
    /// Puntos clave (según tu código):
    /// - EstadisticasService lee Facturas.csv desde: AppDomain.CurrentDomain.BaseDirectory\Data\Facturas.csv
    /// - ProductoService lee Productos.csv desde: AppDomain.CurrentDomain.BaseDirectory\Data\Productos.csv
    /// - Facturas.csv usa separador ';' y 15 columnas:
    ///   Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;Productor;Producto;Cantidad;PrecioUnitario;TotalLinea;SubtotalFactura;ImpuestoFactura;TotalFactura
    /// - Fecha se parsea con formato exacto "yyyy-MM-dd".
    /// - Se evita paralelización para no bloquear archivos CSV.
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class EstadisticasServiceTests
    {
        // =========================
        // SUT + dependencias reales
        // =========================
        private ProductoService? _productoService;
        private EstadisticasService? _estadisticasService;

        // =========================
        // Rutas reales (producción)
        // =========================
        private string RutaProductosCsv => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Productos.csv");
        private string RutaFacturasCsv => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Facturas.csv");

        /// <summary>
        /// Se ejecuta ANTES de cada TestMethod.
        /// Prepara Productos.csv y Facturas.csv controlados, y crea el SUT.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // Asegurar carpeta Data
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));

            // Evitar handles vivos del test anterior
            ForceReleaseFileHandles();

            // Limpiar archivos con retry
            TryDeleteWithRetry(RutaProductosCsv);
            TryDeleteWithRetry(RutaFacturasCsv);

            // 1) Productos.csv base (separador ';')
            WriteAllLinesWithRetry(RutaProductosCsv, new[]
            {
                "Productor;NombreProducto;Precio;Cantidad;UnidadMedida",
                "Puesto Verde;Lechuga;1200;8;Unidades",
                "Puesto Verde;Jugo Naranja;1500.5;10;Litros",
                "Puesto Central;Papa;900;25;Kilogramos"
            });

            // 2) Facturas.csv base (15 columnas, separador ';')
            //    Usamos 2 compras distintas para que se puedan agrupar:
            //    - ana compra Lechuga (2) y Jugo Naranja (1) el 2026-01-01
            //    - ana compra Papa (5) el 2026-02-01
            WriteAllLinesWithRetry(RutaFacturasCsv, new[]
            {
                "Fecha;Usuario;Provincia;Canton;Distrito;Detalles;MetodoPago;Productor;Producto;Cantidad;PrecioUnitario;TotalLinea;SubtotalFactura;ImpuestoFactura;TotalFactura",
                "2026-01-01;ana;SJ;Central;Carmen;[];Tarjeta;Puesto Verde;Lechuga;2;1200;2400;2400;312;2712",
                "2026-01-01;ana;SJ;Central;Carmen;[];Tarjeta;Puesto Verde;Jugo Naranja;1;1500.5;1500.5;2400;312;2712",
                "2026-02-01;ana;SJ;Central;Carmen;[];Efectivo;Puesto Central;Papa;5;900;4500;4500;585;5085"
            });

            // 3) Crear dependencias reales en el orden correcto:
            //    ProductoService lee Productos.csv en su constructor
            _productoService = new ProductoService();
            _estadisticasService = new EstadisticasService(_productoService);
        }

        /// <summary>
        /// Se ejecuta DESPUÉS de cada TestMethod.
        /// Libera referencias y elimina archivos para evitar contaminación y bloqueos.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _estadisticasService = null;
            _productoService = null;

            ForceReleaseFileHandles();

            TryDeleteWithRetry(RutaProductosCsv);
            TryDeleteWithRetry(RutaFacturasCsv);
        }

        // =========================
        // Inventario (Admin)
        // =========================

        /// <summary>
        /// ObtenerInventarioPorProductor debe retornar resumen y detalle
        /// basados en Productos.csv (vía ProductoService).
        /// </summary>
        [TestMethod]
        public void ObtenerInventarioPorProductor_RetornaResumenYDetalle()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var r = _estadisticasService.ObtenerInventarioPorProductor();

            // Assert
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Detalle.Count >= 3, "Se esperaba al menos 3 productos en el detalle.");
            Assert.IsTrue(r.Resumen.Count >= 2, "Se esperaba al menos 2 productores en el resumen.");

            // Verificar que exista "Puesto Verde"
            var existePuestoVerde = false;
            foreach (var item in r.Resumen)
            {
                if (string.Equals(item.Productor, "Puesto Verde", StringComparison.OrdinalIgnoreCase))
                {
                    existePuestoVerde = true;
                    Assert.IsTrue(item.CantidadProductos >= 2, "Puesto Verde debería tener al menos 2 productos.");
                }
            }

            Assert.IsTrue(existePuestoVerde, "Se esperaba encontrar 'Puesto Verde' en el resumen.");
        }

        // =========================
        // Reportes por compras
        // =========================

        /// <summary>
        /// ObtenerProductoresMasComprados debe agrupar por Productor y sumar TotalLinea/Cantidad,
        /// contando compras por factura (ClaveFactura).
        /// </summary>
        [TestMethod]
        public void ObtenerProductoresMasComprados_RetornaOrdenadoPorMonto()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var lista = _estadisticasService.ObtenerProductoresMasComprados(desde: null, hasta: null);

            // Assert
            Assert.IsNotNull(lista);
            Assert.IsTrue(lista.Count >= 2, "Se esperaban al menos 2 productores.");

            // Debe existir Puesto Central y Puesto Verde
            bool hayCentral = false;
            bool hayVerde = false;

            foreach (var p in lista)
            {
                if (p.Productor == "Puesto Central") hayCentral = true;
                if (p.Productor == "Puesto Verde") hayVerde = true;
            }

            Assert.IsTrue(hayCentral, "Se esperaba 'Puesto Central'.");
            Assert.IsTrue(hayVerde, "Se esperaba 'Puesto Verde'.");
        }

        /// <summary>
        /// ObtenerProductosMasComprados debe agrupar por Producto y sumar cantidades/montos.
        /// </summary>
        [TestMethod]
        public void ObtenerProductosMasComprados_RetornaProductosAgrupados()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var lista = _estadisticasService.ObtenerProductosMasComprados(desde: null, hasta: null);

            // Assert
            Assert.IsNotNull(lista);
            Assert.IsTrue(lista.Count >= 3, "Se esperaban al menos 3 productos.");

            // Verificar que Papa existe y que su cantidad total sea 5
            bool papaOk = false;
            foreach (var item in lista)
            {
                if (item.Producto == "Papa")
                {
                    papaOk = true;
                    Assert.AreEqual(5, item.CantidadTotal, "Papa debería sumar 5 unidades/kilos (según el CSV).");
                }
            }

            Assert.IsTrue(papaOk, "Se esperaba encontrar 'Papa' en el reporte.");
        }

        /// <summary>
        /// ObtenerGastoPorMes debe agrupar por año/mes y sumar TotalLinea.
        /// </summary>
        [TestMethod]
        public void ObtenerGastoPorMes_RetornaMesesConMonto()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var lista = _estadisticasService.ObtenerGastoPorMes(desde: null, hasta: null);

            // Assert
            Assert.IsNotNull(lista);
            Assert.IsTrue(lista.Count >= 2, "Se esperaban al menos 2 meses (enero y febrero).");

            bool eneroExiste = false;
            bool febreroExiste = false;

            foreach (var m in lista)
            {
                if (m.Anio == 2026 && m.MesNumero == 1) eneroExiste = true;
                if (m.Anio == 2026 && m.MesNumero == 2) febreroExiste = true;
            }

            Assert.IsTrue(eneroExiste, "Se esperaba el mes 1/2026.");
            Assert.IsTrue(febreroExiste, "Se esperaba el mes 2/2026.");
        }

        /// <summary>
        /// ObtenerUsuariosTop debe agrupar por usuario y contar compras (por factura) y monto total.
        /// </summary>
        [TestMethod]
        public void ObtenerUsuariosTop_RetornaUsuarioAna()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var lista = _estadisticasService.ObtenerUsuariosTop(desde: null, hasta: null);

            // Assert
            Assert.IsNotNull(lista);
            Assert.IsTrue(lista.Count >= 1, "Se esperaba al menos 1 usuario.");

            bool anaOk = false;
            foreach (var u in lista)
            {
                if (u.Usuario == "ana")
                {
                    anaOk = true;
                    Assert.IsTrue(u.NumeroCompras >= 2, "Ana debería tener al menos 2 compras (enero y febrero).");
                    Assert.IsTrue(u.MontoTotal > 0, "El monto total de Ana debería ser > 0.");
                }
            }

            Assert.IsTrue(anaOk, "Se esperaba encontrar el usuario 'ana'.");
        }

        /// <summary>
        /// ObtenerResumenAdmin debe calcular: NumeroCompras, TotalVentas, TotalImpuestos, TicketPromedio.
        /// OJO: este método usa TotalFactura e ImpuestoFactura (por factura agrupada).
        /// </summary>
        [TestMethod]
        public void ObtenerResumenAdmin_CalculaTotales()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var r = _estadisticasService.ObtenerResumenAdmin(desde: null, hasta: null);

            // Assert
            Assert.IsNotNull(r);
            Assert.IsTrue(r.NumeroCompras >= 2, "Se esperaban 2 compras (facturas) mínimo.");
            Assert.IsTrue(r.TotalVentas > 0, "TotalVentas debería ser > 0.");
            Assert.IsTrue(r.TotalImpuestos >= 0, "TotalImpuestos no debería ser negativo.");
            Assert.IsTrue(r.TicketPromedio >= 0, "TicketPromedio no debería ser negativo.");
        }

        /// <summary>
        /// ObtenerResumenUsuario debe filtrar por usuario y calcular totales.
        /// </summary>
        [TestMethod]
        public void ObtenerResumenUsuario_FiltraPorAna()
        {
            // Arrange
            Assert.IsNotNull(_estadisticasService);

            // Act
            var r = _estadisticasService.ObtenerResumenUsuario(desde: null, hasta: null, usuario: "ana");

            // Assert
            Assert.IsNotNull(r);
            Assert.IsTrue(r.NumeroCompras >= 2, "Ana debería tener al menos 2 compras (enero y febrero).");
            Assert.IsTrue(r.TotalVentas > 0, "TotalVentas debería ser > 0.");
        }

        // =========================
        // Helpers anti-bloqueo IO
        // =========================

        /// <summary>
        /// Fuerza liberación de handles/streams que pudieron quedar vivos.
        /// </summary>
        private static void ForceReleaseFileHandles()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Escribe líneas al archivo con reintentos para evitar IOExceptions por bloqueo.
        /// </summary>
        private static void WriteAllLinesWithRetry(string path, string[] lines, int attempts = 25, int delayMs = 120)
        {
            for (int i = 1; i <= attempts; i++)
            {
                try
                {
                    File.WriteAllLines(path, lines);
                    return;
                }
                catch (IOException) when (i < attempts)
                {
                    Thread.Sleep(delayMs);
                }
            }

            File.WriteAllLines(path, lines);
        }

        /// <summary>
        /// Elimina el archivo con reintentos (si existe) para evitar IOExceptions por bloqueo.
        /// </summary>
        private static void TryDeleteWithRetry(string path, int attempts = 25, int delayMs = 120)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            for (int i = 1; i <= attempts; i++)
            {
                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    return;
                }
                catch (IOException) when (i < attempts)
                {
                    Thread.Sleep(delayMs);
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
