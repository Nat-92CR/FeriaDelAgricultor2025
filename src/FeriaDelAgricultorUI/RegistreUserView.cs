using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    public partial class RegisterUserView : Form
    {
        private readonly LoginController loginController;

        public RegisterUserView(LoginController loginController)
        {
            InitializeComponent();
            this.loginController = loginController;

            CargarTiposUsuario();
        }

        private void CargarTiposUsuario()
        {
            cbTipoUsuario.Items.Clear();
            cbTipoUsuario.Items.Add("Cliente");
            cbTipoUsuario.Items.Add("Productor");
            cbTipoUsuario.SelectedIndex = 0;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string usuario = txtUsername.Text.Trim();
            string contraseña = txtPassword.Text.Trim();
            string tipo = cbTipoUsuario.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(contraseña) ||
                string.IsNullOrWhiteSpace(tipo))
            {
                MessageBox.Show("Todos los campos son obligatorios.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // CREA EL USUARIO USANDO EL CONSTRUCTOR CORRECTO
            var nuevo = new Usuario(
                nombre,
                apellido,
                usuario,
                contraseña,
                "",
                tipo
            );

            bool registrado = this.loginController.RegistrarUsuario(nuevo);


            if (registrado)
            {
                MessageBox.Show("Usuario registrado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("El usuario ya existe. Intente otro nombre de usuario.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

