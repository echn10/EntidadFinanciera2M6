using EntidadFinanciera2M6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntidadFinanciera2M6
{
    /// <summary>
    /// Formulario que permite agregar un nuevo cliente
    /// </summary>

    public partial class AgregarClienteForm : Form
    {
        /// <summary>
        /// Cliente creado desde el formulario
        /// </summary>

        public Cliente NuevoCliente { get; private set; }

        /// <summary>
        /// Constructor del formulario AgregarClienteForm
        /// </summary>
        /// 
        public AgregarClienteForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Valida los campos y asigna el nuevo cliente si es válido
        /// </summary>

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtIdentificacion.Text))
            {
                MessageBox.Show("Todos los campos son requeridos");
                return;
            }
            NuevoCliente = new Cliente
            {
                Nombre = txtNombre.Text,
                Identificacion = txtIdentificacion.Text
            };
            DialogResult = DialogResult.OK;
            Close();


        }

        /// <summary>
        /// Cierra el formulario sin guardar cambios
        /// </summary>

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
