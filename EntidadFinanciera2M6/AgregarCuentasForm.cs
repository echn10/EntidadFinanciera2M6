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
    /// Formulario para agregar una nueva cuenta a un cliente
    /// </summary>

    public partial class AgregarCuentasForm : Form
    {
        /// <summary>
        /// Cuenta creada desde el formulario
        /// </summary>
        public Cuenta NuevaCuenta { get; private set; }
        private int _cienteId;

        /// <summary>
        /// Constructor del formulario, recibe el ID del cliente
        /// </summary>

        public AgregarCuentasForm(int clienteId)
        {
            InitializeComponent();
            _cienteId = clienteId;
        }

        /// <summary>
        /// Crea la cuenta si el número es válido y cierra el formulario
        /// </summary>

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNumCuenta.Text))
            {
                MessageBox.Show("El numero de cuenta es requerido");
                return;
            }
            else
            {
                NuevaCuenta = new Cuenta
                {
                    NumeroCuenta = txtNumCuenta.Text,
                    Saldo = numSaldoInicial.Value,
                    ClienteId = _cienteId,
                    Activa = true
                };
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancela la operación y cierra el formulario
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; 
            Close();
        }
    }

}

