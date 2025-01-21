using ClinicManager.Models;
using ClinicManager.Exceptions; // Importar excepciones personalizadas
using Microsoft.EntityFrameworkCore;

namespace ClinicManager.Services
{
    public class PacientesService
    {
        private readonly AppDBContext _dbContext;

        public PacientesService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Paciente>> GetAllPacientesAsync()
        {
            return await _dbContext.Pacientes.ToListAsync();
        }

        public async Task<Paciente?> GetPacienteByIdAsync(int id)
        {
            return await _dbContext.Pacientes
                .Include(p => p.Citas)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        public async Task AddPacienteAsync(Paciente paciente)
        {
            if (!EsValidoNombreApellido(paciente.Nombre) || !EsValidoNombreApellido(paciente.Apellido))
                throw new ValidationException("Nombre y apellido deben contener solo letras.");

            if (!EsTelefonoValido(paciente.Telefono))
                throw new ValidationException("Teléfono inválido. Debe contener solo números y no exceder 10 dígitos.");

            _dbContext.Pacientes.Add(paciente);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePacienteAsync(Paciente paciente, Paciente existingPaciente)
        {
            if (paciente.Nombre != existingPaciente.Nombre || paciente.Apellido != existingPaciente.Apellido)
                throw new BusinessRuleException("No se permite cambiar el nombre o apellido del paciente.");

            if (!EsTelefonoValido(paciente.Telefono))
                throw new ValidationException("Teléfono inválido. Debe contener solo números y no exceder 10 dígitos.");

            existingPaciente.Telefono = paciente.Telefono;
            existingPaciente.Edad = paciente.Edad;

            _dbContext.Pacientes.Update(existingPaciente);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePacienteAsync(Paciente paciente)
        {
            if (paciente.Citas != null && paciente.Citas.Count > 0)
                throw new BusinessRuleException("No se puede eliminar el paciente porque tiene citas asociadas.");

            _dbContext.Pacientes.Remove(paciente);
            await _dbContext.SaveChangesAsync();
        }

        // Validación: Solo letras
        private static bool EsValidoNombreApellido(string input)
        {
            return input.All(char.IsLetter);
        }

        // Validación: Solo números y máximo 10 dígitos
        private static bool EsTelefonoValido(string telefono)
        {
            return telefono.All(char.IsDigit) && telefono.Length <= 10;
        }
    }
}
