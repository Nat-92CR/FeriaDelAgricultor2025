using System.Text;
using FeriaDelAgricultorController;

namespace FeriaDelAgricultorConsole
{
    // ESTA ES LA VISTA-CONSOLE DE LOGIN PARA EL PROYECTO
    public class FeriaDelAgricultorConsoleV
    {
        private readonly LoginController loginController;

        public FeriaDelAgricultorConsoleV(LoginController loginController)
        {
            this.loginController = loginController;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================================");
                Console.WriteLine("   Feria del Agricultor - Login   ");
                Console.WriteLine("==================================");
                Console.Write("Username: ");
                var username = Console.ReadLine() ?? string.Empty;

                Console.Write("Password: ");
                var password = ReadPassword();

                var user = loginController.Login(username, password);   // Ahora devuelve Usuario

                if (user != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"✅ Login exitoso. ¡Bienvenido, {user.Username}!");
                    // Si quieres incluso puedes usar el tipo:
                    // Console.WriteLine($"Tipo de usuario: {user.TipoUsuario}");
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("❌ Usuario o contraseña incorrectos.");
                    Console.WriteLine("Presione una tecla para intentar de nuevo...");
                    Console.ReadKey();
                }
            }
        }

        private static string ReadPassword()
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo key;

            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return sb.ToString();
        }
    }
}
