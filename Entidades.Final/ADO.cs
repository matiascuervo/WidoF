using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Final
{
    public class ADO
    {

        public delegate void ApellidoUsuarioExistenteEventHandler(List<Usuario> usuariosConApellido);
        public event ApellidoUsuarioExistenteEventHandler ApellidoUsuarioExistente;


        public ADO() 
        {
            ApellidoUsuarioExistente += Manejador_apellidoExistenteLog;
            ApellidoUsuarioExistente += Manejador_apellidoExistenteJSON;

        }


        private string conexion = "Server=DESKTOP-7SSNEAH\\MATIASSQL;Database=laboratorio_2;User Id=sa;Password=POLY;";


        public bool Agregar(Usuario user)
        {
            try
            {
                // Obtener todos los usuarios con el mismo apellido
                List<Usuario> usuariosConMismoApellido = ObtenerTodos(user.Apellido);
                usuariosConMismoApellido.Add(user);
                // Si hay usuarios con el mismo apellido, dispara el evento y retorna true
                if (usuariosConMismoApellido.Count > 0)
                {
                    ApellidoUsuarioExistente?.Invoke(usuariosConMismoApellido);

                    Manejadora.EscribirArchivo(usuariosConMismoApellido);
                    Manejadora.SerializarJSON(usuariosConMismoApellido, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));


                    
                }

                // Insertar el nuevo usuario en la base de datos
                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    string query = "INSERT INTO Usuarios (nombre, apellido, correo, dni, clave) VALUES (@nombre, @apellido, @correo, @dni, @clave)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar parámetros a la consulta
                        command.Parameters.AddWithValue("@nombre", user.Nombre);
                        command.Parameters.AddWithValue("@apellido", user.Apellido);
                        command.Parameters.AddWithValue("@correo", user.Correo);
                        command.Parameters.AddWithValue("@dni", user.DNI);
                        command.Parameters.AddWithValue("@clave", user.Clave);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Si se afectó al menos una fila, el usuario fue agregado con éxito
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public bool Eliminar(Usuario user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    string query = "DELETE FROM Usuarios WHERE DNI = @DNI";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DNI", user.DNI);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public bool Modificar(Usuario user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    // Actualiza los valores, excepto el DNI
                    string query = "UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Correo = @Correo, Clave = @Clave WHERE DNI = @DNI";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", user.Nombre);
                        command.Parameters.AddWithValue("@Apellido", user.Apellido);
                        command.Parameters.AddWithValue("@Correo", user.Correo);
                        command.Parameters.AddWithValue("@Clave", user.Clave);
                        command.Parameters.AddWithValue("@DNI", user.DNI);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public static List<Usuario> ObtenerTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string conexion = "Server=DESKTOP-7SSNEAH\\MATIASSQL;Database=laboratorio_2;User Id=sa;Password=POLY;";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    string query = "SELECT Nombre,apellido ,Correo, clave , DNI FROM Usuarios";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombre = reader["Nombre"].ToString();
                                string correo = reader["Correo"].ToString();
                                string apellido = reader["apellido"].ToString();
                                string clave = reader["clave"].ToString();
                                int dni = Convert.ToInt32(reader["DNI"]);

                                Usuario usuario = new Usuario
                                {
                                    Nombre = nombre,
                                    Correo = correo,
                                    DNI = dni,
                                    Clave =clave,
                                    Apellido = apellido
                                };

                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return usuarios;
        }

        public static List<Usuario> ObtenerTodos(string apellidoUsuario)
        {
            List<Usuario> usuarios = new List<Usuario>();

            string conexion = "Server=DESKTOP-7SSNEAH\\MATIASSQL;Database=laboratorio_2;User Id=sa;Password=POLY;";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    connection.Open();

                    // Filtra por apellido en la consulta SQL
                    string query = "SELECT Nombre, Correo, DNI, Apellido FROM Usuarios WHERE Apellido LIKE @ApellidoUsuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Añade el parámetro del apellido para filtrar en la consulta
                        command.Parameters.AddWithValue("@ApellidoUsuario", $"%{apellidoUsuario}%");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombre = reader["Nombre"].ToString();
                                string correo = reader["Correo"].ToString();
                                string apellido = reader["Apellido"].ToString();
                                int dni = Convert.ToInt32(reader["DNI"]);
                                


                                Usuario usuario = new Usuario
                                {
                                    Nombre = nombre,
                                    Correo = correo,
                                    Apellido = apellido,
                                    DNI = dni
                                };

                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return usuarios;
        }

        public static void Manejador_apellidoExistenteLog(List<Usuario> usuariosConMismoApellido)
        {
            bool resultado = Manejadora.EscribirArchivo(usuariosConMismoApellido);
            if (!resultado)
            {
                Console.WriteLine("Error al escribir en el archivo usuarios.log");
            }

        }

        public static void Manejador_apellidoExistenteJSON(List<Usuario> usuariosConApellido) 
        {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\usuarios_repetidos.json";
            bool resultado = Manejadora.SerializarJSON(usuariosConApellido, ruta);
            if (!resultado)
            {
                Console.WriteLine("Error al serializar el archivo usuarios_repetidos.json");
            }


        }




    }
}
