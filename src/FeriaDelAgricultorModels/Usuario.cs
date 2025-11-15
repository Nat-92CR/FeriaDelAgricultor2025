using System;
using System.Collections.Generic;

namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Model representing a customer / user.
    /// </summary>
    public class Usuario
    {

        internal Usuario() 
        {
        }
            
        /// <summary>
        /// Initializes a new instance of the <see cref="Usuario"/> class.
        /// Este constructor se usa cuando se carga el CSV:
        /// Name,LastName,Username,Password,Directions
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="directions">The directions string from the file.</param>
        public Usuario(string name, string lastName, string username, string password, string directions)
        {
            this.Name = name;
            this.LastName = lastName;
            this.Username = username;
            this.Password = password;

            // Lista de direcciones inicializada
            this.Directions = new List<Direccion>();
            this.CreateDirectionsFromStringArray(directions);

            // Por defecto, todos se consideran Cliente.
            this.TipoUsuario = TipoUsuario.Cliente;
        }

        /// <summary>
        /// Constructor alternativo que permite crear usuarios indicando el tipo (Cliente o Productor).
        /// </summary>
        public Usuario(
            string name,
            string lastName,
            string username,
            string password,
            string directions,
            TipoUsuario tipoUsuario)
            : this(name, lastName, username, password, directions)
        {
            this.TipoUsuario = tipoUsuario;
        }

        /// <summary>
        /// Constructor alternativo para cuando el tipo de usuario viene como texto (por ejemplo desde CSV).
        /// </summary>
        public Usuario(
            string name,
            string lastName,
            string username,
            string password,
            string directions,
            string tipoUsuarioTexto)
            : this(name, lastName, username, password, directions)
        {
            if (Enum.TryParse(tipoUsuarioTexto, true, out TipoUsuario tipo))
            {
                this.TipoUsuario = tipo;
            }
            else
            {
                // Si no se puede parsear, se deja como Cliente por defecto.
                this.TipoUsuario = TipoUsuario.Cliente;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de usuario: Cliente o Productor.
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; }

        /// <summary>
        /// Lista de direcciones del usuario.
        /// </summary>
        public List<Direccion> Directions { get; private set; }

        /// <summary>
        /// Convierte el usuario en una línea CSV, incluyendo el tipo de usuario.
        /// </summary>
        public override string ToString()
        {
            // Formato: Name,LastName,Username,Password,TipoUsuario,[]
            return $"{this.Name},{this.LastName},{this.Username},{this.Password},{this.TipoUsuario},[]{Environment.NewLine}";
        }

        /// <summary>
        /// Metodos privados van despues de las propiedades
        /// Crea la lista de direcciones a partir del texto del archivo.
        /// Hoy no la usamos, por eso está vacío.
        /// </summary>
        /// <param name="directionsInfo">Información de direcciones en formato texto.</param>
        private void CreateDirectionsFromStringArray(string directionsInfo)
        {
            // TODO: cuando el profe pida trabajar con direcciones,
            // aquí parseas "directionsInfo" y llenas this.Directions.
        }
    }
}
