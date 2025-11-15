using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Controlador responsable del proceso de login.
    /// </summary>
    public class LoginController

    {
        public readonly UserHandler userHandler;

        public LoginController(UserHandler userHandler)
        {
            this.userHandler = userHandler;
        }

        /// <summary>
        /// Intenta iniciar sesión y devuelve el usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="password">Contraseña.</param>
        /// <returns>
        /// El usuario autenticado o null si las credenciales son inválidas.
        /// </returns>
        public Usuario Login(string userName, string password)
        {
            return this.userHandler.GetUserByCredentials(userName, password);
        }
    }
}
