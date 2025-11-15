using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Handler class to manage user data (load, authentication, etc.).
    /// </summary>
    public class UserHandler
    {
        // 👇 AQUÍ SÍ usamos el genérico con Usuario / Implementacion -No abstraccion 
        private readonly IDataHandler<Usuario> dataHandler;
        private List<Usuario> users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHandler"/> class.
        /// </summary>
        /// <param name="dataHandler">The data handler instance (e.g. FileHandler).</param>
        public UserHandler(IDataHandler<Usuario> dataHandler)
        {
            this.dataHandler = dataHandler;
            this.users = new List<Usuario>();
        }

        /// <summary>
        /// Loads users from the file.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>True if users are loaded successfully; otherwise, false.</returns>
        public bool LoadUsers(string filePath)
        {
            try
            {
                // Llama al método del dataHandler específico de Usuario
                this.users = this.dataHandler.LoadData(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Finds a user matching the given username and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The matching user or null if not found.</returns>
        public Usuario GetUserByCredentials(string username, string password)
        {
            foreach (var user in this.users)
            {
                if (user.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                    user.Password == password)
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all users loaded in memory.
        /// </summary>
        public List<Usuario> GetAllUsers()
        {
            return this.users;
        }
    }
}
