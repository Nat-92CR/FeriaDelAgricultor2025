using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeriaDelAgricultorController
{
    /// <summary>
    /// Handler class to manage user data (load, authentication, register, save).
    /// </summary>
    public class UserHandler
    {
        private readonly IDataHandler<Usuario> dataHandler;
        private List<Usuario> users;

        public UserHandler(IDataHandler<Usuario> dataHandler)
        {
            this.dataHandler = dataHandler;
            this.users = new List<Usuario>();
        }

        /// <summary>
        /// Load user list from CSV using FileHandler.
        /// </summary>
        public bool LoadUsers(string filePath)
        {
            try
            {
                this.users = this.dataHandler.LoadData(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate login.
        /// </summary>
        public Usuario GetUserByCredentials(string username, string password)
        {
            return this.users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        public List<Usuario> GetAllUsers()
        {
            return this.users;
        }

        /// <summary>
        /// Insert a new user.
        /// </summary>
        public bool InsertUser(Usuario nuevo)
        {
            if (this.users.Any(u => u.Username.Equals(nuevo.Username, StringComparison.OrdinalIgnoreCase)))
                return false;

            this.users.Add(nuevo);
            return true;
        }

        /// <summary>
        /// Save users to CSV file.
        /// </summary>
        public bool SaveUsers(string filePath)
        {
            return this.dataHandler.SaveData(this.users, filePath);
        }

        internal bool SaveUsers(object fileNameUsers)
        {
            throw new NotImplementedException();
        }
    }
}
