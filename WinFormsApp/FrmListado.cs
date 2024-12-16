using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Entidades.Final;
using System.Data.SqlClient;
using System.Linq;

namespace WinFormsApp
{
    public partial class FrmListado : Form
    {
        List<Usuario> lista;

        public FrmListado()
        {
            InitializeComponent();
            //MostrarLista(); 
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StartPosition = FormStartPosition.CenterScreen;


        }

        private void FrmListado_Load(object sender, EventArgs e)
        {
            ///Utilizando la clase ADO, obtener y mostrar a todos los usuarios
            ///
            this.lista = ADO.ObtenerTodos();

            this.dataGridView1.DataSource = this.lista;

        }

        public void MostrarLista()
        {
            // Obtener los usuarios
            List<Usuario> usuarios = ADO.ObtenerTodos();

            // Verificar si hay usuarios
            if (usuarios.Count > 0)
            {
                // Concatenar los datos de los usuarios en una cadena
                string mensaje = string.Join(Environment.NewLine, usuarios.Select(u =>
                    $"Nombre: {u.Nombre}, Apellido: {u.Apellido}, Correo: {u.Correo}, DNI: {u.DNI}"));

                // Mostrar en un MessageBox
                MessageBox.Show(mensaje, "Lista de Usuarios");
            }
            else
            {
                // Mensaje si no hay usuarios
                MessageBox.Show("No se encontraron usuarios.", "Información");
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ///Agregar un nuevo usuario a la base de datos
            ///Utilizar FrmUsuario.
            ///Agregar manejadores de eventos (punto 14)

            FrmUsuario frm = new FrmUsuario();
            frm.StartPosition = FormStartPosition.CenterParent;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                Usuario nuevoUsuario = frm.MiUsuario;

                ADO ado = new ADO();
                
               
                

                bool resultado = ado.Agregar(nuevoUsuario);



                if (resultado)
                {
                    this.lista = ADO.ObtenerTodos(); // Actualiza la lista de usuarios
                    this.dataGridView1.DataSource = null;
                    this.dataGridView1.DataSource = this.lista; // Refresca el DataGridView
                    MessageBox.Show("Usuario agregado exitosamente.");
                }
                else
                {
                    MessageBox.Show("El usuario no pudo ser agregado. Verifique si ya existe un usuario con el mismo apellido.");
                }

            }
        }

       

        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Obtener el índice de la fila seleccionada
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para modificar.");
                return;
            }

            int i = this.dataGridView1.SelectedRows[0].Index;

            Usuario usuarioSeleccionado = this.lista[i];

            // Crear una instancia del formulario de usuario para editar
            FrmUsuario frm = new FrmUsuario(usuarioSeleccionado);
            frm.StartPosition = FormStartPosition.CenterParent;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                Usuario usuarioModificado = frm.MiUsuario;

                // Actualizar en la base de datos
                ADO ado = new ADO();
                
                bool resultado = ado.Modificar(usuarioModificado);

                if (resultado)
                {
                    // Refrescar la lista
                    this.lista = ADO.ObtenerTodos();
                    this.dataGridView1.DataSource = null;
                    this.dataGridView1.DataSource = this.lista;
                    MessageBox.Show("Usuario modificado exitosamente.");
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el usuario.");
                }
            }
        }

     

        private void btnEliminar_Click(object sender, EventArgs e)
{
    // Verifica que haya una fila seleccionada
    if (this.dataGridView1.SelectedRows.Count == 0)
    {
        MessageBox.Show("Seleccione un usuario para eliminar.");
        return;
    }

    // Obtiene el índice de la fila seleccionada
    int i = this.dataGridView1.SelectedRows[0].Index;

    // Verifica que el índice sea válido
    if (i < 0 || i >= this.lista.Count)
    {
        MessageBox.Show("El usuario seleccionado no es válido.");
        return;
    }

    // Obtiene el usuario de la lista en base al índice
    Usuario user = this.lista[i];

    // Abre el formulario de confirmación de eliminación
    FrmUsuario frm = new FrmUsuario(user);
    frm.StartPosition = FormStartPosition.CenterParent;

    // Si el usuario confirma la eliminación
    if (frm.ShowDialog() == DialogResult.OK)
    {
        // Crea una instancia de ADO
        ADO ado = new ADO();

        // Llama al método Eliminar de la instancia
        if (ado.Eliminar(user))
        {
            // Elimina el usuario de la lista local
            this.lista.RemoveAt(i);

            // Actualiza el DataGridView
            this.dataGridView1.DataSource = null; // Limpia el DataSource
            this.dataGridView1.DataSource = this.lista; // Reasigna la lista

            MessageBox.Show("Usuario eliminado exitosamente.");
        }
        else
        {
            MessageBox.Show("No se pudo eliminar el usuario.");
        }
    }
}



        ///Si el apellido ya existe en la base, se disparará el evento ApellidoUsuarioExistente. 
        ///Diseñarlo (de acuerdo a las convenciones vistas) en la clase ADO. 
        ///Crear el manejador necesario para que, una vez capturado el evento, se guarde:
        ///1) en un archivo de texto: 
        ///la fecha (con hora, minutos y segundos) y en un nuevo renglón, el apellido y todos
        ///los correos electrónicos para ese apellido.
        ///Se deben acumular los mensajes. 
        ///El archivo se guardará con el nombre 'usuarios.log' en la carpeta 'Mis documentos' del cliente.
        ///2) en un archivo JSON:
        ///serializar todos los objetos de tipo Usuario cuyo apellido esté repetido.
        ///El archivo se guardará en el escritorio del cliente, con el nombre 'usuarios_repetidos.json'
        ///
        ///El manejador de eventos (Manejador_apellidoExistenteLog) invocará al método (de clase) 
        ///EscribirArchivo(List<Usuario>) (se alojará en la clase Manejadora en Entidades), 
        ///que retorna un booleano indicando si se pudo escribir o no.
        ///            
        ///El manejador de eventos (Manejador_apellidoExistenteJSON) invocará al método (de clase) 
        ///SerializarJSON(List<Usuario>, string) (se alojará en la clase Manejadora en Entidades), 
        ///que retorna un booleano indicando si se pudo escribir o no.
        ///
        private void Manejador_apellidoExistenteLog(object sender, EventArgs e)
        {
            bool todoOK = false;///Reemplazar por la llamada al método de clase Manejadora.EscribirArchivo

            MessageBox.Show("Apellido repetido log!!!");

            if (todoOK)
            {
                MessageBox.Show("Se escribió correctamente!!!");
            }
            else
            {
                MessageBox.Show("No se pudo escribir!!!");
            }
        }

        private void Manejador_apellidoExistenteJSON(object sender, EventArgs e)
        {
            string path = "";///reemplazar por el path correspondiente.
            bool todoOK = false;///Reemplazar por la llamada al método de clase Manejadora.SerializarJSON

            MessageBox.Show("Apellido repetido JSON!!!");

            if (todoOK)
            {
                MessageBox.Show("Se escribió correctamente!!!");
            }
            else
            {
                MessageBox.Show("No se pudo escribir!!!");
            }
        }

       
    }
}
