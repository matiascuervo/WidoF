using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Final
{
    public class Usuario
    {
        private string apellido;
        private string clave;
        private string correo;
        private int dni;
        private string nombre;

        public Usuario(string apellido, int dni, string correo, string nombre)
        {
            this.apellido = apellido; 
            this.correo = correo;
            this.dni = dni;
            this.nombre = nombre;
        }

        public Usuario(string apellido, string clave, int dni, string correo, string nombre)
        {
            
            this.clave = clave;
            this.apellido = apellido;
            this.correo = correo;
            this.dni = dni;
            this.nombre = nombre;


        }

        public Usuario() { }


       


        public override string ToString()
        {
            return $"¨{Nombre} , {apellido}, DNI:{dni},Correo {correo} ";
        }

        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }

        }
        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }
        public string Correo
        {
            get { return correo; }
            set { correo = value; }

        }

        public int DNI 
        {
            get { return dni; } set { dni = value; }
        }

        public string Nombre 
        {
            get { return nombre; } set { nombre = value; }
        } 

        

    }
}
