using System;
using System.Windows.Forms;
using Entidades.Final;
namespace WinFormsApp

{
    public partial class FrmUsuario : Form
    {   
        
        
        private Usuario miUsuario;

        public Usuario MiUsuario
        {
            get { return this.miUsuario; }
        }

        public FrmUsuario()
        {
            InitializeComponent();
        }

        public FrmUsuario(Usuario user) : this()
        {
            this.txtNombre.Text = user.Nombre;
            this.txtApellido.Text = user.Apellido;
            this.txtDni.Text = user.DNI.ToString();
            this.txtCorreo.Text = user.Correo;
            this.txtClave.Text = user.Clave;
            this.txtDni.ReadOnly = true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Verifica que todos los campos tengan datos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text) ||
                string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            // Verifica si el DNI es un número válido
            if (!int.TryParse(txtDni.Text, out int dni))
            {
                MessageBox.Show("DNI inválido.");
                return;
            }

            // Crea el nuevo usuario con los datos del formulario
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
              
            string correo = txtCorreo.Text;
            string clave = txtClave.Text;

            Usuario user = new Usuario(apellido, clave, dni, correo, nombre);
            this.miUsuario = user;

            this.DialogResult = DialogResult.OK;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
