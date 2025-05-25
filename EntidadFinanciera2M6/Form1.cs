using EntidadFinanciera2M6.Data;
using EntidadFinanciera2M6.Models;
using EntidadFinanciera2M6.Services;
using Microsoft.EntityFrameworkCore;

namespace EntidadFinanciera2M6
{
    public partial class Form1 : Form
    {
        // Contexto y servicios para manejar la lógica de negocio
        private readonly EntidadFinancieraContext _db;
        private readonly ClienteService _clienteService;
        private readonly TransaccionService _transaccionService;

        public Form1()
        {
            InitializeComponent();
            // Inicialización del contexto y los servicios
            _db = new EntidadFinancieraContext();
            _clienteService = new ClienteService(_db);
            _transaccionService = new TransaccionService(_db);
            // Cargar los datos al iniciar el formulario
            _ = CargarDatosAsync();
        }

        // Carga los datos de clientes y cuentas en los DataGridView
        private async Task CargarDatosAsync()
        {
            try
            {
                // Obtener clientes y cuentas activas usando los servicios
                var clientes = await _clienteService.ObtenerClientesConCuentasAsync();
                var cuentas = await _clienteService.ObtenerCuentasActivasAsync();

                // Asignar los datos a los controles de la interfaz
                dgvClientes.DataSource = clientes;
                dgvCuentas.DataSource = cuentas.Select(c => new
                {
                    c.CuentaId,
                    c.NumeroCuenta,
                    c.Saldo,
                    c.Activa,
                    c.ClienteId,
                    c.Cliente.Nombre
                }).ToList();
            }
            catch (Exception ex)
            {
                // Manejo de errores: mostrar mensaje al usuario
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para agregar un nuevo cliente
        private async void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new AgregarClienteForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Usar el servicio para agregar el cliente
                    await _clienteService.AgregarClienteAsync(form.NuevoCliente);
                    await CargarDatosAsync();
                }
            }
            catch (Exception ex)
            {
                // Mostrar error si ocurre algún problema
                MessageBox.Show($"Error al agregar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para agregar una nueva cuenta a un cliente seleccionado
        private async void btnAgregarCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un cliente primero", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var clienteId = (int)dgvClientes.SelectedRows[0].Cells["ClienteId"].Value;
                var form = new AgregarCuentasForm(clienteId);
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Usar el servicio para agregar la cuenta
                    await _clienteService.AgregarCuentaAsync(form.NuevaCuenta);
                    await CargarDatosAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar cuenta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para desactivar una cuenta seleccionada
        private async void btnDesctivarCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCuentas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una cuenta para desactivar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cuentaId = (int)dgvCuentas.SelectedRows[0].Cells["CuentaId"].Value;
                // Usar el servicio para desactivar la cuenta
                await _clienteService.DesactivarCuentaAsync(cuentaId);
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desactivar cuenta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para realizar una transferencia entre dos cuentas seleccionadas
        private async void btnTransferencia_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCuentas.SelectedRows.Count != 2)
                {
                    MessageBox.Show("Seleccione exactamente 2 cuentas", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cuentaOrigenId = (int)dgvCuentas.SelectedRows[0].Cells["CuentaId"].Value;
                var cuentaDestinoId = (int)dgvCuentas.SelectedRows[1].Cells["CuentaId"].Value;

                var form = new TransferenciaForms(cuentaOrigenId, cuentaDestinoId);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Usar el servicio de transacciones para realizar la transferencia
                    await _transaccionService.RealizarTransferenciaAsync(cuentaOrigenId, cuentaDestinoId, form.Monto);
                    MessageBox.Show("Transferencia realizada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarDatosAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la transferencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para abrir el formulario secundario (Form2)
        private void button1_Click(object sender, EventArgs e)
        {
            var form = new Form2();
            form.ShowDialog();
        }

        // Liberar recursos al cerrar el formulario
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _db.Dispose();
        }
    }
}
