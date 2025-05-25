using EntidadFinanciera2M6.Data;
using Microsoft.EntityFrameworkCore;
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
    /// Formulario que permite ingresar el monto para realizar una transferencia entre cuentas
    /// </summary>

    public partial class TransferenciaForms : Form
    {
        /// <summary>
        /// Monto a transferir ingresado por el usuario
        /// </summary>

        public decimal Monto { get; set; }
        private int _cuentaOrignId;
        private int _cuentaDestinoId;
        private EntidadFinancieraContext _Ef;

        /// <summary>
        /// Constructor que recibe las cuentas involucradas y carga sus datos
        /// </summary>

        public TransferenciaForms(int cuentaOrigenId, int cuentaDestinoId)
        {
            _cuentaOrignId = cuentaOrigenId;
            _cuentaDestinoId = cuentaDestinoId;
            _Ef = new EntidadFinancieraContext();
            InitializeComponent();
            CargarCuentas();
        }

        /// <summary>
        /// Muestra la información de las cuentas en pantalla
        /// </summary>

        private void CargarCuentas()
        {
            var cuentaOrigen = _Ef.Cuentas.
                Include(c => c.Cliente).
                First(c => c.CuentaId == _cuentaOrignId);

            var cuentaDestino = _Ef.Cuentas.
                Include(c => c.Cliente).
                First(c => c.CuentaId == _cuentaDestinoId);

            lblCuentaOrigen.Text = $"Cuenta Origen: {cuentaOrigen.Cliente.Nombre} - {cuentaOrigen.NumeroCuenta}";
            lblCuentaDestino.Text = $"Cuenta Destino: {cuentaDestino.Cliente.Nombre} - {cuentaDestino.NumeroCuenta}";
            lblSaldo.Text = $"Saldo Disponible {cuentaOrigen.Saldo:c}";
        }

        /// <summary>
        /// Valida el monto ingresado y finaliza la operación si es válido
        /// </summary>

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (numMonto.Value > 0)
            {
                Monto = numMonto.Value;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            MessageBox.Show("El monto tiene que ser mayor a 0");
        }
    }
}
