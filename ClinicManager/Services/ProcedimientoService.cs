using ClinicManager.Models;
using ClinicManager.Exceptions; // Excepciones personalizadas
using Microsoft.EntityFrameworkCore;

namespace ClinicManager.Services
{
    public class ProcedimientosService
    {
        private readonly AppDBContext _dbContext;

        public ProcedimientosService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Procedimiento>> GetAllProcedimientosAsync()
        {
            return await _dbContext.Procedimientos.Include(p => p.Cita).ToListAsync();
        }

        public async Task<Procedimiento?> GetProcedimientoByIdAsync(int id)
        {
            return await _dbContext.Procedimientos.Include(p => p.Cita).FirstOrDefaultAsync(p => p.IdProcedimiento == id);
        }

        public async Task AddProcedimientoAsync(Procedimiento procedimiento)
        {
            if (string.IsNullOrWhiteSpace(procedimiento.Descripcion))
                throw new ValidationException("La descripción no puede estar vacía.");

            if (procedimiento.Costo < 0)
                throw new ValidationException("El costo debe ser mayor o igual a 0.");

            var citaExiste = await _dbContext.Citas.AnyAsync(c => c.IdCita == procedimiento.IdCita);
            if (!citaExiste)
                throw new NotFoundException("La cita especificada no existe.");

            _dbContext.Procedimientos.Add(procedimiento);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProcedimientoAsync(Procedimiento procedimiento)
        {
            if (string.IsNullOrWhiteSpace(procedimiento.Descripcion))
                throw new ValidationException("La descripción no puede estar vacía.");

            if (procedimiento.Costo < 0)
                throw new ValidationException("El costo debe ser mayor o igual a 0.");

            var citaExiste = await _dbContext.Citas.AnyAsync(c => c.IdCita == procedimiento.IdCita);
            if (!citaExiste)
                throw new NotFoundException("La cita especificada no existe.");

            _dbContext.Procedimientos.Update(procedimiento);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProcedimientoAsync(Procedimiento procedimiento)
        {
            if (procedimiento == null)
                throw new NotFoundException("El procedimiento no existe.");

            _dbContext.Procedimientos.Remove(procedimiento);
            await _dbContext.SaveChangesAsync();
        }
    }
}
