using Entidades.Final;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    ///Agregar manejo de excepciones en TODOS los lugares críticos!!!

    public delegate void DelegadoThreadConParam(object param);

    public partial class FrmPrincipal : Form
    {
        protected Task hilo;
        protected CancellationTokenSource cts;

        public FrmPrincipal()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += FrmPrincipal_FormClosing;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            this.Text = "Cuervo Matias";
            MessageBox.Show(this.Text);  
            

        }

        ///
        /// CRUD
        ///
        private void listadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListado frm = new FrmListado();
            frm.StartPosition = FormStartPosition.CenterScreen;

            frm.Show(this);
        }

        ///
        /// VER LOG
        ///
        private void verLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de Log (*.log)|*.log|Todos los archivos (*.*)|*.*",
                Title = "Seleccione un archivo de log"
            };

            DialogResult rta = openFileDialog.ShowDialog(); // Abrir el diálogo para seleccionar el archivo

            if (rta == DialogResult.OK)
            {
                try
                {
                    // Leer todo el contenido del archivo seleccionado
                    string contenidoLog = File.ReadAllText(openFileDialog.FileName);

                    // Mostrar el contenido del archivo en el TextBox txtUsuariosLog
                    this.txtUsuariosLog.Clear();
                    this.txtUsuariosLog.Text = contenidoLog;
                }
                catch (Exception ex)
                {
                    // Mostrar un mensaje en caso de error al leer el archivo
                    MessageBox.Show($"Error al abrir el archivo .log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo .log", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        ///
        /// DESERIALIZAR JSON
        ///
        private void deserializarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Entidades.Final.Usuario> listado = null;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//usuarios_repetidos.json";

            // Llamar al método DeserializarJSON de Manejadora
            bool todoOK = Manejadora.DeserializarJSON(path, out listado);

            if (todoOK)
            {
                // Limpiar el contenido previo de txtUsuariosLog
                this.txtUsuariosLog.Clear();
                StringBuilder sb = new StringBuilder();
                // Mostrar el contenido deserializado en el TextBox
                foreach (var usuario in listado)
                {
                    sb.AppendLine($"Nombre: {usuario.Nombre}");
                    sb.AppendLine($"Apellido: {usuario.Apellido}");
                    sb.AppendLine($"Correo: {usuario.Correo}");
                    sb.AppendLine($"DNI: {usuario.DNI}");
                    sb.AppendLine();
                }

                this.txtUsuariosLog.Text = sb.ToString();
            }
            else
            {
                // Mostrar un mensaje de error si la deserialización falla
                MessageBox.Show("NO se pudo deserializar a JSON");
            }
        }


        ///
        /// TASK
        ///
        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.cts = new CancellationTokenSource();
            ///Se inicia el hilo.
            this.hilo = Task.Run(() => ActualizarListadoUsuarios(cts.Token)); /// inicializar tarea
            ///Se desasocia al manejador de eventos.
            this.taskToolStripMenuItem.Click -= new EventHandler(this.taskToolStripMenuItem_Click);
        }


        ///PARA ACTUALIZAR LISTADO DESDE BD EN HILO
        public void ActualizarListadoUsuarios(object param)
        {
            CancellationToken token = (CancellationToken)param;
            bool alternarColor = false;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    
                    List<Usuario> usuarios = ADO.ObtenerTodos();

                    // Actualizar el control lstUsuarios en el hilo principal
                    this.Invoke((Action)(() =>
                    {
                        MostrarUsuariosEnLista(usuarios);

                        // Alternar colores
                        if (alternarColor)
                        {
                            lstUsuarios.BackColor = Color.Black;
                            lstUsuarios.ForeColor = Color.White;
                        }
                        else
                        {
                            lstUsuarios.BackColor = Color.White;
                            lstUsuarios.ForeColor = Color.Black;
                        }

                        alternarColor = !alternarColor;
                    }));

                    
                    Thread.Sleep(1500);
                }
            }
            catch (OperationCanceledException)
            {
                // La tarea fue cancelada
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro error
                MessageBox.Show($"Error en la tarea: {ex.Message}");
            }
        }

        private void MostrarUsuariosEnLista(List<Usuario> usuarios)
        {
            lstUsuarios.Items.Clear();

            foreach (var usuario in usuarios)
            {
                lstUsuarios.Items.Add($"Nombre: {usuario.Nombre}, Apellido: {usuario.Apellido}, Correo: {usuario.Correo}, DNI: {usuario.DNI}");
            }
        }


        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel(); // Cancela la tarea
                if (hilo != null)
                {
                    hilo.Wait(); // Espera a que la tarea termine
                }
            }
        }
    }
}
