using System;
using System.Windows.Forms;
using Entidades.Final_;

namespace WinFormsApp
{

    public partial class FrmLogin : Form
    {
        private Boolean usuarioLogueado;

        public Boolean UsuarioLogueado
        {
            get { return this.usuarioLogueado; }
        }

        public FrmLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {


                MessageBox.Show("La primera vez, utilizar 'juan@perez.com' y '123456'.");
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // Validación inicial
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            Login obj = new Login(txtCorreo.Text, txtClave.Text);

            // Loguear usuario
            this.usuarioLogueado = obj.Loguear();

            // Configuración del resultado y navegación
            this.DialogResult = this.usuarioLogueado ? DialogResult.OK : DialogResult.Retry;

            if (this.usuarioLogueado)
            {
                FrmPrincipal principal = new FrmPrincipal();
                this.Hide();
                principal.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales inválidas. Por favor, inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }
    }
}
