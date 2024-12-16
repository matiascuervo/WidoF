using System;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;



namespace Entidades.Final_
{
    public class Login
    {
        private string Email;
        private string Password;

        public string email
        {
            get { return Email; }
        }

        public string password
        {
            get { return Password; }
        }

        public Login(string correo, string clave)
        {
            this.Email = correo;
            this.Password = clave;


        }


        string connectionString = "Server=DESKTOP-7SSNEAH\\MATIASSQL;Database=laboratorio_2;User Id=sa;Password=POLY;";


        public bool Loguear()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Cambiar correo por nombre en la consulta SQL
                    string query = "SELECT COUNT(1) FROM [dbo].[usuarios] WHERE [nombre] = @nombre AND [clave] = @clave";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Cambiar parámetro para capturar nombre en lugar de correo
                        command.Parameters.AddWithValue("@nombre", email.Trim()); // Cambiar email por el nombre del usuario
                        command.Parameters.AddWithValue("@clave", password.Trim());

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        Console.WriteLine($"Resultado de la consulta: {count}"); // Depuración
                        return count == 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar loguear: {ex.Message}");
                    return false;
                }
            }
        }




        
    }
}
