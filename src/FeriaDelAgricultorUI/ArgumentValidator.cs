using System;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    /// <summary>
    /// Clase de utilidades para validar argumentos de formularios.
    /// Se encarga de mostrar mensajes, limpiar y enfocar controles.
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// Valida que un TextBox no esté vacío.
        /// </summary>
        /// <param name="textBox">Control a validar.</param>
        /// <param name="fieldDisplayName">Nombre del campo para mostrar en el mensaje.</param>
        /// <returns>true si tiene contenido; false en caso contrario.</returns>
        public static bool ValidateRequired(TextBox textBox, string fieldDisplayName)
        {
            if (textBox == null)
                throw new ArgumentNullException(nameof(textBox));

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show($"El campo {fieldDisplayName} no puede estar vacío.");
                textBox.Clear();
                textBox.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida los campos necesarios para el login.
        /// </summary>
        /// <param name="txtUserName">TextBox del nombre de usuario.</param>
        /// <param name="txtPassword">TextBox de la contraseña.</param>
        /// <returns>true si ambos campos son válidos; false en caso contrario.</returns>
        public static bool ValidateLogin(TextBox txtUserName, TextBox txtPassword)
        {
            if (!ValidateRequired(txtUserName, "nombre de usuario"))
            {
                return false;
            }

            if (!ValidateRequired(txtPassword, "contraseña"))
            {
                return false;
            }

            return true;
        }
    }
}
