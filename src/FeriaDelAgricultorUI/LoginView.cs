using FeriaDelAgricultorController;
using FeriaDelAgricultorModels;
using System;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    public partial class LoginView : Form
    {
        private readonly LoginController loginController;

        public LoginView(LoginController loginController)
        {
            InitializeComponent();
            this.loginController = loginController;
            
        }

        private bool Login(string userName, string password)
        {
            // Ahora recibimos el usuario desde el controlador
            Usuario user = this.loginController.Login(userName, password);

            if (user != null)
            {
                MessageBox.Show(
                    $"Inicio de sesión exitoso." +
                    $"\nBienvenido {user.Name}.{user.LastName}");

                //Aquí decidimos a qué vista va según el tipo
                if (user.TipoUsuario == TipoUsuario.Cliente)
                {
                    // Vista para clientes
                    var mainView = new MainMenuView(user);
                    mainView.Show();
                }
                else if (user.TipoUsuario == TipoUsuario.Productor)
                {
                    // Vista para productores (luego la creamos bien)
                    var productorView = new ProductorView(user);
                    productorView.Show();
                }

                // Ocultamos la ventana de login
                this.Hide();
                return true;
            }

            MessageBox.Show("Usuario o contraseña incorrectos.");
            return false;
        }


        private void BtnLoginClick(object sender, EventArgs e)
        {
            // 1. Validar argumentos usando ArgumentValidator
            if (!ArgumentValidator.ValidateLogin(this.txt_UserName, this.txt_Password))
            {
                return;
            }

            // 2. Obtener los valores de los TextBox
            var userName = this.txt_UserName.Text;
            var password = this.txt_Password.Text;

            // 3. Intentar login
            Login(userName, password);
        }


        private void txt_UserName_TextChanged(object sender, EventArgs e)
        {
            // Si no necesitas reaccionar al cambio de texto, lo dejamos vacío.
        }
    }
}
