using FeriaDelAgricultorController;
using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.IO;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    internal static class Program
    {

        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// Aquí se inicializa la configuración y se abre la ventana de inicio de sesión.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicializa la configuración visual de Windows Forms.
            ApplicationConfiguration.Initialize();

            // Carga los servicios necesarios para el controlador principal.
            var userController = LoadControllerService();

            // Inicia la vista principal del login.
            Application.Run(new LoginView(userController));
        }

        /// <summary>
        /// Carga el servicio principal de usuarios y valida si los datos existen.
        /// Si el archivo CSV no puede cargarse, la aplicación se cierra mostrando un mensaje.
        /// </summary>
        /// <returns>Una instancia del controlador de inicio de sesión (LoginController).</returns>
        private static LoginController LoadControllerService()
        {
            try
            {
                var userHandler = new UserHandler(new FileHandler());

                var couldLoadUsers = userHandler.LoadUsers(Generales.FileNameUsers);

                if (!couldLoadUsers)
                {
                    MessageBox.Show(
                        "⚠️ No se pudieron cargar los usuarios desde la fuente de datos.\nLa aplicación se cerrará.",
                        "Error de carga",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    Environment.Exit(0);
                }

                return new LoginController(userHandler);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al intentar iniciar la aplicación:\n{ex.Message}",
                    "Error crítico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(0);
                return null!;
            }
        }
    }
}
