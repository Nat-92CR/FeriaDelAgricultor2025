using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorWeb.Services
{
    /// <summary>
    /// Servicio responsable de mantener la información del usuario autenticado
    /// durante el alcance de la sesión (Scoped) en la aplicación Blazor.
    /// </summary>
    public class SessionUsuarioService
    {
        /// <summary>
        /// Obtiene el usuario autenticado actual en la sesión.
        /// </summary>
        public Usuario? UsuarioActual { get; private set; }

        /// <summary>
        /// Indica si existe un usuario autenticado en la sesión.
        /// </summary>
        public bool EstaAutenticado => UsuarioActual != null;

        /// <summary>
        /// Establece el usuario autenticado actual en la sesión.
        /// </summary>
        /// <param name="usuario">Usuario que queda autenticado.</param>
        public void IniciarSesion(Usuario usuario)
        {
            UsuarioActual = usuario;
        }

        /// <summary>
        /// Elimina el usuario autenticado actual, dejando la sesión sin autenticación.
        /// </summary>
        public void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
