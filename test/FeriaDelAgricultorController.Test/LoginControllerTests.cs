using FeriaDelAgricultorController;
using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace FeriaDelAgricultorControllerTest
{
    /// <summary>
    /// Pruebas unitarias para <see cref="LoginController"/>.
    ///
    /// Consideraciones clave del proyecto:
    /// - NO se modifica LoginController ni Generales.
    /// - Directions tiene private set, por lo que se usa el constructor público con "directions" como string.
    /// - Se evita que las pruebas corran en paralelo para no bloquear Usuario.csv.
    /// - Se usan reintentos y GC para mitigar bloqueos del archivo en Windows/OneDrive/antivirus.
    /// </summary>
    [TestClass]
    [DoNotParallelize] // Evita que MSTest ejecute esta clase en paralelo con otras.
    public class LoginControllerTests
    {
        // =========================
        // Campos privados
        // =========================
        private LoginController? _loginController;
        private UserHandler? _userHandler;

        /// <summary>
        /// Ruta real del CSV que usa producción (NO se reasigna).
        /// </summary>
        private string RutaUsuariosCsv => Generales.FileNameUsers;

        // =========================
        // Configuración / Limpieza
        // =========================

        /// <summary>
        /// Se ejecuta ANTES de cada prueba.
        /// Prepara un Usuario.csv controlado y crea el SUT (LoginController).
        /// </summary>
        [TestInitialize]
        public void InicializarPrueba()
        {
            // 1) Asegurar carpeta
            var carpeta = Path.GetDirectoryName(RutaUsuariosCsv);
            if (!string.IsNullOrWhiteSpace(carpeta))
                Directory.CreateDirectory(carpeta);

            // 2) Asegurar que no quede un handle vivo del test anterior
            ForzarLiberacionDeRecursos();

            // 3) Limpiar archivo anterior con reintentos
            IntentarEliminarConReintentos(RutaUsuariosCsv);

            // 4) Crear CSV base controlado con retry
            //    Mantengo 6 columnas coherentes con Usuario.ToString():
            //    Name,LastName,Username,Password,TipoUsuario,Directions
            EscribirLineasConReintentos(RutaUsuariosCsv, new[]
            {
                "Name,LastName,Username,Password,TipoUsuario,Directions",
                "Ana,Gomez,ana,123,Cliente,[]",
                "Admin,Root,admin,admin123,Admin,[]"
            });

            // 5) Construir dependencias reales
            //    (Si tu UserHandler usa otro constructor, aquí es donde se ajusta,
            //     pero sin tocar producción.)
            IDataHandler<Usuario> fileHandler = new FileHandler();
            _userHandler = new UserHandler(fileHandler);

            // 6) SUT
            _loginController = new LoginController(_userHandler);
        }

        /// <summary>
        /// Se ejecuta DESPUÉS de cada prueba.
        /// Limpia Usuario.csv y libera recursos para evitar bloqueos.
        /// </summary>
        [TestCleanup]
        public void LimpiarPrueba()
        {
            _loginController = null;
            _userHandler = null;

            // Forzar liberación de streams si algo quedó abierto internamente.
            ForzarLiberacionDeRecursos();

            IntentarEliminarConReintentos(RutaUsuariosCsv);
        }

        // =========================
        // Pruebas de Login
        // =========================

        /// <summary>
        /// Login debe retornar un Usuario cuando las credenciales son válidas.
        /// </summary>
        [TestMethod]
        public void IniciarSesion_CredencialesValidas_RetornaUsuario()
        {
            // Arrange
            Assert.IsNotNull(_loginController);

            // Act
            var u = _loginController.Login("ana", "123");

            // Assert
            Assert.IsNotNull(u, "Se esperaba un usuario cuando las credenciales son válidas.");
            Assert.AreEqual("ana", u.Username, "El username retornado no coincide.");
        }

        /// <summary>
        /// Login debe retornar null cuando el password es incorrecto.
        /// </summary>
        [TestMethod]
        public void IniciarSesion_ContrasenaIncorrecta_RetornaNull()
        {
            // Arrange
            Assert.IsNotNull(_loginController);

            // Act
            var u = _loginController.Login("ana", "MAL_PASSWORD");

            // Assert
            Assert.IsNull(u, "Se esperaba null cuando el password es incorrecto.");
        }

        /// <summary>
        /// Login debe retornar null cuando el usuario no existe.
        /// </summary>
        [TestMethod]
        public void IniciarSesion_UsuarioNoExiste_RetornaNull()
        {
            // Arrange
            Assert.IsNotNull(_loginController);

            // Act
            var u = _loginController.Login("noexiste", "123");

            // Assert
            Assert.IsNull(u, "Se esperaba null cuando el usuario no existe.");
        }

        // =========================
        // Pruebas de RegistrarUsuario
        // =========================

        /// <summary>
        /// RegistrarUsuario debe retornar true cuando el usuario es nuevo
        /// y debe persistirse en el CSV.
        /// </summary>
        [TestMethod]
        public void RegistrarUsuario_UsuarioNuevo_RetornaTrue_YGuardaCsv()
        {
            // Arrange
            Assert.IsNotNull(_loginController);

            // IMPORTANTE: NO usar new Usuario() ni object initializer.
            // Usar constructor público (directions como string).
            var nuevo = new Usuario(
                name: "Luis",
                lastName: "Perez",
                username: "luis",
                password: "pass1",
                directions: "[]",
                tipoUsuario: TipoUsuario.Cliente
            );

            // Act
            var ok = _loginController.RegistrarUsuario(nuevo);

            // Assert
            Assert.IsTrue(ok, "Se esperaba true al registrar un usuario nuevo.");
            Assert.IsTrue(File.Exists(RutaUsuariosCsv), "Se esperaba que Usuario.csv exista después de registrar.");

            var contenido = LeerTodoElTextoConReintentos(RutaUsuariosCsv);
            StringAssert.Contains(contenido, "luis", "Se esperaba encontrar el username 'luis' en el CSV.");
        }

        /// <summary>
        /// RegistrarUsuario debe retornar false si el username ya existe.
        /// </summary>
        [TestMethod]
        public void RegistrarUsuario_UsuarioDuplicado_RetornaFalse()
        {
            // Arrange
            Assert.IsNotNull(_loginController);

            var duplicado = new Usuario(
                name: "Otra",
                lastName: "Persona",
                username: "ana", // ya existe en CSV base
                password: "otra",
                directions: "[]",
                tipoUsuario: TipoUsuario.Cliente
            );

            // Act
            var ok = _loginController.RegistrarUsuario(duplicado);

            // Assert
            Assert.IsFalse(ok, "Se esperaba false cuando el username ya existe.");
        }

        /// <summary>
        /// RegistrarUsuario debe retornar false si los datos son inválidos
        /// (ejemplo: username vacío).
        /// </summary>
        [TestMethod]
        public void RegistrarUsuario_DatosInvalidos_RetornaFalse()
        {
            // Arrange
            Assert.IsNotNull(_loginController);

            var invalido = new Usuario(
                name: "X",
                lastName: "Y",
                username: "",      // inválido
                password: "123",
                directions: "[]",
                tipoUsuario: TipoUsuario.Cliente
            );

            // Act
            var ok = _loginController.RegistrarUsuario(invalido);

            // Assert
            Assert.IsFalse(ok, "Se esperaba false cuando el username es inválido.");
        }

        // =========================
        // Helpers anti-bloqueo
        // =========================

        /// <summary>
        /// Fuerza a liberar handles/streams que pudieron quedar vivos.
        /// Útil cuando internamente hay streams no dispuestos a tiempo.
        /// </summary>
        private static void ForzarLiberacionDeRecursos()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Escribe líneas al archivo con reintentos para evitar IOExceptions por bloqueo.
        /// </summary>
        private static void EscribirLineasConReintentos(string path, string[] lines, int attempts = 20, int delayMs = 120)
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

            // Último intento sin ocultar excepción
            File.WriteAllLines(path, lines);
        }

        /// <summary>
        /// Lee el archivo con reintentos para evitar IOExceptions por bloqueo.
        /// </summary>
        private static string LeerTodoElTextoConReintentos(string path, int attempts = 20, int delayMs = 120)
        {
            for (int i = 1; i <= attempts; i++)
            {
                try
                {
                    return File.ReadAllText(path);
                }
                catch (IOException) when (i < attempts)
                {
                    Thread.Sleep(delayMs);
                }
            }

            return File.ReadAllText(path);
        }

        /// <summary>
        /// Elimina el archivo con reintentos (si existe) para evitar IOExceptions por bloqueo.
        /// </summary>
        private static void IntentarEliminarConReintentos(string path, int attempts = 20, int delayMs = 120)
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
