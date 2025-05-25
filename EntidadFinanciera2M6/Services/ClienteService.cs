using EntidadFinanciera2M6.Data;
using EntidadFinanciera2M6.Models;
using Microsoft.EntityFrameworkCore;

namespace EntidadFinanciera2M6.Services
{
    public class ClienteService
    {
        // Contexto de base de datos inyectado por dependencia
        private readonly EntidadFinancieraContext _db;

        public ClienteService(EntidadFinancieraContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtiene todos los clientes con sus cuentas relacionadas
        /// </summary>
        public async Task<List<Cliente>> ObtenerClientesConCuentasAsync()
        {
            // Incluye las cuentas relacionadas usando Include
            return await _db.Clientes
                .Include(c => c.Cuentas)
                .ToListAsync();
        }

        /// <summary>
        /// Agrega un nuevo cliente al sistema
        /// </summary>
        public async Task<Cliente> AgregarClienteAsync(Cliente cliente)
        {
            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new ArgumentException("El nombre del cliente no puede estar vacío");
            }

            await _db.Clientes.AddAsync(cliente);
            await _db.SaveChangesAsync();
            return cliente;
        }

        /// <summary>
        /// Obtiene todas las cuentas activas con información del cliente
        /// </summary>
        public async Task<List<Cuenta>> ObtenerCuentasActivasAsync()
        {
            // Solo cuentas activas y con información del cliente
            return await _db.Cuentas
                .Include(c => c.Cliente)
                .Where(c => c.Activa)
                .ToListAsync();
        }

        /// <summary>
        /// Agrega una nueva cuenta para un cliente
        /// </summary>
        public async Task<Cuenta> AgregarCuentaAsync(Cuenta cuenta)
        {
            // Validar que el ID del cliente sea válido
            if (cuenta.ClienteId <= 0)
            {
                throw new ArgumentException("El ID del cliente no es válido");
            }

            // Verificar que el cliente exista
            var cliente = await _db.Clientes.FindAsync(cuenta.ClienteId);
            if (cliente == null)
            {
                throw new InvalidOperationException("El cliente especificado no existe");
            }

            await _db.Cuentas.AddAsync(cuenta);
            await _db.SaveChangesAsync();
            return cuenta;
        }

        /// <summary>
        /// Desactiva una cuenta existente
        /// </summary>
        public async Task<bool> DesactivarCuentaAsync(int cuentaId)
        {
            var cuenta = await _db.Cuentas.FindAsync(cuentaId);
            if (cuenta == null)
            {
                throw new InvalidOperationException("La cuenta especificada no existe");
            }

            cuenta.Activa = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
} 