using EntidadFinanciera2M6.Data;
using EntidadFinanciera2M6.Models;
using Microsoft.EntityFrameworkCore;

namespace EntidadFinanciera2M6.Services
{
    public class TransaccionService
    {
        // Contexto de base de datos inyectado por dependencia
        private readonly EntidadFinancieraContext _db;

        public TransaccionService(EntidadFinancieraContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Realiza una transferencia entre cuentas con manejo de transacciones
        /// </summary>
        /// <param name="cuentaOrigenId">ID de la cuenta origen</param>
        /// <param name="cuentaDestinoId">ID de la cuenta destino</param>
        /// <param name="monto">Monto a transferir</param>
        /// <returns>True si la transferencia fue exitosa, False en caso contrario</returns>
        public async Task<bool> RealizarTransferenciaAsync(int cuentaOrigenId, int cuentaDestinoId, decimal monto)
        {
            // Se utiliza una transacción para asegurar la integridad de la operación
            using var transaccion = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                // Buscar las cuentas involucradas
                var cuentaOrigen = await _db.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == cuentaOrigenId);
                var cuentaDestino = await _db.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == cuentaDestinoId);

                // Validar que ambas cuentas existan
                if (cuentaOrigen == null || cuentaDestino == null)
                {
                    throw new InvalidOperationException("Una o ambas cuentas no existen");
                }

                // Validar que ambas cuentas estén activas
                if (!cuentaOrigen.Activa || !cuentaDestino.Activa)
                {
                    throw new InvalidOperationException("Una o ambas cuentas están inactivas");
                }

                // Validar que la cuenta origen tenga saldo suficiente
                if (cuentaOrigen.Saldo < monto)
                {
                    throw new InvalidOperationException("Saldo insuficiente en la cuenta origen");
                }

                // Realizar la transferencia
                cuentaOrigen.Saldo -= monto;
                cuentaDestino.Saldo += monto;

                // Registrar la transacción en la base de datos
                var nuevaTransaccion = new Transaccion
                {
                    Monto = monto,
                    Fecha = DateTime.Now,
                    Tipo = "Transferencia",
                    Descripcion = $"Transferencia de cuenta {cuentaOrigen.NumeroCuenta} a {cuentaDestino.NumeroCuenta}",
                    CuentaOrigenId = cuentaOrigenId,
                    CuentaDestinoId = cuentaDestinoId
                };

                await _db.Transacciones.AddAsync(nuevaTransaccion);
                await _db.SaveChangesAsync();
                await transaccion.CommitAsync(); // Confirmar la transacción

                return true;
            }
            catch (Exception)
            {
                // Si ocurre un error, se revierte la transacción
                await transaccion.RollbackAsync();
                throw;
            }
        }
    }
} 