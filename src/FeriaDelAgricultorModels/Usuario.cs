using System;
using System.Collections.Generic;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Representa un usuario del sistema (Cliente o Productor).
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Constructor vacío utilizado internamente cuando se requiere una instancia sin datos.
        /// </summary>
        internal Usuario()
        {
        }

        /// <summary>
        /// Constructor principal utilizado para cargar usuarios desde el archivo CSV.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="lastName">Apellido del usuario.</param>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <param name="directions">Cadena con información de direcciones (sin uso por ahora).</param>
        public Usuario(string name, string lastName, string username, string password, string directions)
        {
            this.Name = name;
            this.LastName = lastName;
            this.Username = username;
            this.Password = password;

            this.Directions = new List<Direccion>();
            this.CreateDirectionsFromStringArray(directions);

            // Valor por defecto
            this.TipoUsuario = TipoUsuario.Cliente;
        }

        /// <summary>
        /// Constructor utilizado cuando el tipo de usuario se define explícitamente (desde el registro).
        /// </summary>
        public Usuario(string name, string lastName, string username, string password, string directions, TipoUsuario tipoUsuario)
            : this(name, lastName, username, password, directions)
        {
            this.TipoUsuario = tipoUsuario;
        }

        /// <summary>
        /// Constructor utilizado cuando el tipo de usuario viene como texto desde el CSV.
        /// </summary>
        public Usuario(string name, string lastName, string username, string password, string directions, string tipoUsuarioTexto)
            : this(name, lastName, username, password, directions)
        {
            if (Enum.TryParse(tipoUsuarioTexto, true, out TipoUsuario tipo))
            {
                this.TipoUsuario = tipo;
            }
            else
            {
                this.TipoUsuario = TipoUsuario.Cliente;
            }
        }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de usuario para iniciar sesión.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de usuario (Cliente o Productor).
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; }

        /// <summary>
        /// Lista de direcciones del usuario (sin uso por el momento).
        /// </summary>
        public List<Direccion> Directions { get; private set; }

        /// <summary>
        /// Convierte el usuario en una línea de texto compatible con CSV.
        /// </summary>
        public override string ToString()
        {
            return $"{this.Name},{this.LastName},{this.Username},{this.Password},{this.TipoUsuario},[]";
        }

        /// <summary>
        /// Procesa la cadena de direcciones (no implementado).
        /// </summary>
        private void CreateDirectionsFromStringArray(string directionsInfo)
        {
            // Aquí se agregará la lógica cuando el profesor solicite manejar direcciones reales.
        }
    }
}
