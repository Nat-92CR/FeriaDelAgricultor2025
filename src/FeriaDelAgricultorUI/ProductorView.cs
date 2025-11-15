using FeriaDelAgricultorModels;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    public partial class ProductorView : Form
    {
        private readonly Usuario usuario;

        /// <summary>
        /// Vista principal para usuarios de tipo Productor.
        /// </summary>
        /// <param name="usuario">Usuario que inició sesión.</param>
        public ProductorView(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;

            ConfigurarVistaProductor();
        }

        /// <summary>
        /// Configura el mensaje y apariencia de la vista según el productor.
        /// </summary>
        private void ConfigurarVistaProductor()
        {
            if (usuario == null)
            {
                lblBienvenidaProductor.Text = "Error: usuario no válido.";
                return;
            }

            // Seguridad extra: si por error entra un cliente aquí
            if (usuario.TipoUsuario != TipoUsuario.Productor)
            {
                MessageBox.Show(
                    "Solo los usuarios de tipo PRODUCTOR pueden acceder a esta pantalla.",
                    "Acceso denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.Close();
                return;
            }

            string nombre = $"{usuario.Name} {usuario.LastName}".Trim();

            lblBienvenidaProductor.Text =
                $"🌾 ¡Bienvenido productor {nombre}!\n" +
                $"Has iniciado sesión como PRODUCTOR.";

            // Opcional: color de fondo especial para productores
            this.BackColor = Color.Moccasin;
        }
    }
}
