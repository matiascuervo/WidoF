using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Final
{
    public static class Manejadora
    {



        public static bool EscribirArchivo(List<Usuario> usuarios)
        {
            try
            {
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//usuarios.log";
                using (StreamWriter writer = new StreamWriter(ruta, true)) // 'true' para acumular mensajes
                {
                    string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    writer.WriteLine(fecha);

                    foreach (var usuario in usuarios)
                    {
                        writer.WriteLine($"{usuario.Apellido}: {usuario.Correo}");
                    }
                    writer.WriteLine(); // Dejar una línea en blanco entre entradas
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool SerializarJSON(List<Usuario> usuarios, string rutaArchivo)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(rutaArchivo, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }




        public static bool DeserializarJSON(string rutaArchivo, out List<Usuario> usuarios)
        {
            usuarios = new List<Usuario>();
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    string jsonString = File.ReadAllText(rutaArchivo);
                    usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonString);
                    return true;
                }
                else
                {
                    Console.WriteLine($"El archivo {rutaArchivo} no existe.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar JSON: {ex.Message}");
                return false;
            }
        }



    }
}







    

