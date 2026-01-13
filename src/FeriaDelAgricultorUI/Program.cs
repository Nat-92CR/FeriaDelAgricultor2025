using FeriaDelAgricultorController;
using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var userController = LoadControllerService();
            Application.Run(new LoginView(userController));
        }

        /// <summary>
        /// Carga el controlador principal de usuarios y valida si los datos existen.
        /// Si el archivo CSV no puede cargarse, la aplicación se cierra mostrando un mensaje.
        /// </summary>
        private static LoginController LoadControllerService()
        {
            try
            {
                // Handler de usuarios (archivo CSV)
                var userHandler = new UserHandler(new FileHandler());

                // ✅ LoadUsers es VOID en tu implementación: se llama y luego se valida por cantidad.
                userHandler.LoadUsers(Generales.FileNameUsers);

                // Si no cargó nada, se considera error de datos.
                if (userHandler.GetAllUsers() == null || userHandler.GetAllUsers().Count == 0)
                {
                    MessageBox.Show(
                        "No se pudieron cargar los usuarios desde la fuente de datos.\nLa aplicación se cerrará.",
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
                return null!; // nunca se alcanza
            }
        }
    }
}
