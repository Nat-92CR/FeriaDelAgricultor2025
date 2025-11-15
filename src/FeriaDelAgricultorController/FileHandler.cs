using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace FeriaDelAgricultorController
{
    public class FileHandler : IDataHandler<Usuario>
    {
        public List<Usuario> LoadData(string fileName)
        {
            var usuarios = new List<Usuario>();

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"No se encontró el archivo: {fileName}");
            }

            var lineas = File.ReadAllLines(fileName);

            // Saltamos la primera línea (encabezado)
            for (int i = 1; i < lineas.Length; i++)
            {
                var linea = lineas[i];
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                var partes = linea.Split(',');

                if (partes.Length < 6)
                    continue; // Evita errores si una línea está incompleta

                string name = partes[0].Trim();
                string lastName = partes[1].Trim();
                string username = partes[2].Trim();
                string password = partes[3].Trim();
                string tipoUsuarioTexto = partes[4].Trim();
                string directions = partes[5].Trim();

                // Usa el constructor con tipo de usuario
                var usuario = new Usuario(
                    name,
                    lastName,
                    username,
                    password,
                    directions,
                    tipoUsuarioTexto);

                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public bool SaveData(List<Usuario> data, string fileName) //Metodo SaveData: Guardar una Lista de Usuarios. Lista csv Usuario.csv
        
        //Funcion
        {
            using var writer = new StreamWriter(fileName);
            writer.WriteLine("Name,LastName,Username,Password,TipoUsuario,Directions");

            foreach (var user in data)
            {
                writer.WriteLine($"{user.Name},{user.LastName},{user.Username},{user.Password},{user.TipoUsuario},{(user.Directions?.FirstOrDefault() != null ? "Sin dirección" : "Sin dirección")}");

            }

            return true;
        }
    }
}
