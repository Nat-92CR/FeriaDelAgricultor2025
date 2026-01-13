using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeriaDelAgricultorController
{
    public class FileHandler : IDataHandler<Usuario>
    {
        public bool SaveData(List<Usuario> data, string fileName)
        {
            try
            {
                var ruta = ResolverRutaData(fileName);

                // Mantenemos el header de 7 columnas (como tu Usuario.csv actual),
                // aunque el modelo Usuario NO tiene City: lo dejamos vacío para no romper el CSV.
                var lines = new List<string>
                {
                    "Name,LastName,Username,Password,TipoUsuario,City,Directions"
                };

                foreach (var u in data)
                {
                    string city = "";            // Usuario NO tiene City
                    string directions = "[]";    // Usuario guarda direcciones como List<Direccion> (sin uso ahora)

                    lines.Add($"{u.Name},{u.LastName},{u.Username},{u.Password},{u.TipoUsuario},{city},{directions}");
                }

                Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
                File.WriteAllLines(ruta, lines);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Usuario> LoadData(string fileName)
        {
            try
            {
                var ruta = ResolverRutaData(fileName);

                if (!File.Exists(ruta))
                    return new List<Usuario>();

                var lineas = File.ReadAllLines(ruta)
                                 .Skip(1)
                                 .Where(l => !string.IsNullOrWhiteSpace(l))
                                 .ToList();

                var usuarios = new List<Usuario>();

                foreach (var linea in lineas)
                {
                    // Soporta ',' y ';'
                    var parts = linea.Contains(';') ? linea.Split(';') : linea.Split(',');

                    // Soportamos CSV de 6 o 7 columnas:
                    // 6: Name,LastName,Username,Password,TipoUsuario,Directions
                    // 7: Name,LastName,Username,Password,TipoUsuario,City,Directions
                    if (parts.Length < 6) continue;

                    var name = parts[0].Trim();
                    var lastName = parts[1].Trim();
                    var username = parts[2].Trim();
                    var password = parts[3].Trim();
                    var tipoTexto = parts[4].Trim();

                    string directionsInfo;

                    if (parts.Length >= 7)
                    {
                        // City existe en el CSV, pero no en el modelo.
                        // Lo “metemos” dentro de directionsInfo para no perderlo, aunque no se use aún.
                        var city = parts[5].Trim();
                        var dir = parts[6].Trim();
                        directionsInfo = $"{city} - {dir}";
                    }
                    else
                    {
                        directionsInfo = parts[5].Trim();
                    }

                    // IMPORTANTE: usamos el constructor público del modelo
                    // que acepta tipo como texto (y lo convierte a enum internamente).
                    var usuario = new Usuario(name, lastName, username, password, directionsInfo, tipoTexto);

                    usuarios.Add(usuario);
                }

                return usuarios;
            }
            catch
            {
                return new List<Usuario>();
            }
        }

        private static string ResolverRutaData(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName) && Path.IsPathRooted(fileName))
                return fileName;

            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                fileName ?? "Usuario.csv"
            );
        }
    }
}
