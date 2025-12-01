using FeriaDelAgricultorModels;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Controlador responsable de manejar la lógica de autenticación
    /// y registro de usuarios en el sistema.
    /// </summary>
    public class LoginController
    {
        /// <summary>
        /// Manejador de usuarios que se encarga de la carga,
        /// búsqueda y persistencia de la información de los usuarios.
        /// </summary>
        public readonly UserHandler userHandler;

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="LoginController"/>.
        /// </summary>
        /// <param name="userHandler">
        /// Instancia de <see cref="UserHandler"/> que se utilizará
        /// para realizar las operaciones sobre los usuarios.
        /// </param>
        public LoginController(UserHandler userHandler)
        {
            this.userHandler = userHandler;
        }

        /// <summary>
        /// Intenta autenticar un usuario con el nombre de usuario
        /// y la contraseña proporcionados.
        /// </summary>
        /// <param name="userName">Nombre de usuario ingresado.</param>
        /// <param name="password">Contraseña ingresada.</param>
        /// <returns>
        /// Un objeto <see cref="Usuario"/> si las credenciales son válidas;
        /// de lo contrario, <c>null</c>.
        /// </returns>
        public Usuario Login(string userName, string password)
        {
            return this.userHandler.GetUserByCredentials(userName, password);
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema y guarda la información
        /// en el archivo correspondiente.
        /// </summary>
        /// <param name="nuevo">
        /// Objeto <see cref="Usuario"/> con los datos del nuevo usuario.
        /// </param>
        /// <returns>
        /// <c>true</c> si el usuario se registró y se guardó correctamente;
        /// <c>false</c> si ocurrió algún problema en el proceso.
        /// </returns>
        public bool RegistrarUsuario(Usuario nuevo)
        {
            // 1. Intentar insertar el usuario en la lista en memoria.
            var insertado = this.userHandler.InsertUser(nuevo);

            // Si no se pudo insertar (por ejemplo, usuario duplicado),
            // devolvemos false sin intentar guardar en archivo.
            if (!insertado)
            {
                return false;
            }

            // 2. Guardar la lista actualizada de usuarios en el archivo CSV.
            //    Utilizamos la ruta definida en la clase Generales.
            return this.userHandler.SaveUsers(Generales.FileNameUsers);
        }
    }
}
