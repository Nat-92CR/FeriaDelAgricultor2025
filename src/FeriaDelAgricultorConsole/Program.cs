//Permitir que la consola “vea” los controladores y modelos
using FeriaDelAgricultorConsole;
using FeriaDelAgricultorController;
using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;


namespace FeriaDelAgricultorConsole
{
    /// <summary>
    /// Entry point class for the console application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// 
        /// Execute this three exercises.
        /// 
        /// sum of elements in a list of integers
        /// ispalindrome
        /// revert a string
        /// 
        /// obtener el maximo y el minimo en una lista de enteros. [ 25, 22, 7, 15, 10 ] min => 7 , max => 25
        /// obtener el indice en un string. "Hello, World!" buscar la letra "o" => indice 4
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("==================================");
            Console.WriteLine("   Feria del Agricultor (Consola) ");
            Console.WriteLine("==================================");
            Console.WriteLine();

            var loginController = BuildLoginController();
            var loginView = new FeriaDelAgricultorConsoleV(loginController);
            loginView.Run();
        }

        private static LoginController BuildLoginController()
        {
            //Ruta donde se encuentra el archivo Usuario.csv
            const string CostumersFilePath = @"C:\Users\natal\OneDrive\Escritorio\PROYECT3Q\Usuario.csv";

            var userHandler = new UserHandler(new FileHandler<Usuario>());
            var couldLoadUsers = userHandler.LoadUsers(CostumersFilePath);

            if (!couldLoadUsers)
            {
                Console.WriteLine("Could not load users from data source. The application will close.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            return new LoginController(userHandler);
        }
    }
}