using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Clase encargada de administrar los usuarios del sistema.
    /// Se encarga de:
    /// - Cargar usuarios desde el archivo CSV
    /// - Validar credenciales de inicio de sesión
    /// - Registrar nuevos usuarios
    /// - Guardar los usuarios nuevamente en el archivo CSV
    /// </summary>
    public class UserHandler
    {
        private List<Usuario> users;
        private readonly IDataHandler<Usuario> dataHandler;

        /// <summary>
        /// Constructor que inicializa el handler y carga los usuarios.
        /// </summary>
        public UserHandler(IDataHandler<Usuario> dataHandler)
        {
            this.dataHandler = dataHandler;
            this.users = new List<Usuario>();
            LoadUsers(Generales.FileNameUsers);
        }

        /// <summary>
        /// Carga los usuarios desde el archivo CSV.
        /// </summary>
        /// <param name="filePath">Ruta del archivo CSV.</param>
        public void LoadUsers(string filePath)
        {
            try
            {
                this.users = this.dataHandler.LoadData(filePath);
            }
            catch
            {
                this.users = new List<Usuario>();
            }
        }

        /// <summary>
        /// Devuelve todos los usuarios cargados.
        /// </summary>
        public List<Usuario> GetAllUsers()
        {
            return this.users;
        }

        /// <summary>
        /// Busca un usuario por credenciales (username + password).
        /// </summary>
        public Usuario? GetUserByCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            return this.users.FirstOrDefault(u =>
                u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Password.Equals(password.Trim(), StringComparison.Ordinal));
        }

        /// <summary>
        /// Inserta un nuevo usuario si no existe ya el username.
        /// </summary>
        public bool InsertUser(Usuario nuevo)
        {
            if (nuevo == null) return false;

            if (string.IsNullOrWhiteSpace(nuevo.Username) || string.IsNullOrWhiteSpace(nuevo.Password))
                return false;

            var existe = this.users.Any(u =>
                u.Username.Equals(nuevo.Username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existe) return false;

            this.users.Add(nuevo);
            return true;
        }

        /// <summary>
        /// Guarda los usuarios al archivo CSV.
        /// </summary>
        public bool SaveUsers(string filePath)
        {
            try
            {
                return this.dataHandler.SaveData(this.users, filePath);
            }
            catch
            {
                return false;
            }
        }

        // ==========================
        // ✅ MÉTODOS PARA ADMIN
        // ==========================

        public bool UpdateUser(Usuario actualizado)
        {
            if (actualizado == null) return false;

            var idx = this.users.FindIndex(u =>
                u.Username.Equals(actualizado.Username, StringComparison.OrdinalIgnoreCase));

            if (idx < 0) return false;

            this.users[idx] = actualizado;
            return SaveUsers(Generales.FileNameUsers);
        }

        public bool DeleteUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            var u = this.users.FirstOrDefault(x =>
                x.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (u == null) return false;

            this.users.Remove(u);
            return SaveUsers(Generales.FileNameUsers);
        }

        public bool SetRole(string username, TipoUsuario nuevoRol)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            var u = this.users.FirstOrDefault(x =>
                x.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (u == null) return false;

            u.TipoUsuario = nuevoRol;
            return SaveUsers(Generales.FileNameUsers);
        }

        public bool ResetPassword(string username, string nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(nuevaPassword))
                return false;

            var u = this.users.FirstOrDefault(x =>
                x.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (u == null) return false;

            u.Password = nuevaPassword.Trim();
            return SaveUsers(Generales.FileNameUsers);
        }
    }
}
