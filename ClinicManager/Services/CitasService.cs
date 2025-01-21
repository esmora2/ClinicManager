using ClinicManager.Exceptions;
using ClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManager.Services
{
    public class CitasService
    {
        private readonly AppDBContext _dbContext;

        public CitasService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Cita>> GetAllCitasAsync()
        {
            return await _dbContext.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Doctor)
                .ToListAsync();
        }

        public async Task<Cita?> GetCitaByIdAsync(int id)
        {
            return await _dbContext.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(c => c.IdCita == id);
        }

        public async Task AddCitaAsync(Cita cita)
        {
            // Validar que la fecha no sea en el pasado
            if (cita.Fecha < DateTime.Now)
                throw new PastDateException("No se pueden programar citas con fechas pasadas.");

            // Validar que el paciente exista
            var pacienteExiste = await _dbContext.Pacientes.AnyAsync(p => p.IdPaciente == cita.IdPaciente);
            if (!pacienteExiste)
                throw new NotFoundException($"El paciente con ID {cita.IdPaciente} no existe.");

            // Validar que el doctor exista
            var doctorExiste = await _dbContext.Doctores.AnyAsync(d => d.IdDoctor == cita.IdDoctor);
            if (!doctorExiste)
                throw new NotFoundException($"El doctor con ID {cita.IdDoctor} no existe.");

            _dbContext.Citas.Add(cita);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCitaAsync(Cita cita)
        {
            // Validar que la fecha no sea en el pasado
            if (cita.Fecha < DateTime.Now)
                throw new PastDateException("No se pueden programar citas con fechas pasadas.");

            // Validar que el paciente exista
            var pacienteExiste = await _dbContext.Pacientes.AnyAsync(p => p.IdPaciente == cita.IdPaciente);
            if (!pacienteExiste)
                throw new NotFoundException($"El paciente con ID {cita.IdPaciente} no existe.");

            // Validar que el doctor exista
            var doctorExiste = await _dbContext.Doctores.AnyAsync(d => d.IdDoctor == cita.IdDoctor);
            if (!doctorExiste)
                throw new NotFoundException($"El doctor con ID {cita.IdDoctor} no existe.");

            _dbContext.Citas.Update(cita);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCitaAsync(Cita cita)
        {
            _dbContext.Citas.Remove(cita);
            await _dbContext.SaveChangesAsync();
        }
    }
}
