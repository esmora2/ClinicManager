using ClinicManager.Exceptions;
using ClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManager.Services
{
    public class DoctoresService
    {
        private readonly AppDBContext _dbContext;

        public DoctoresService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Doctor>> GetAllDoctoresAsync()
        {
            return await _dbContext.Doctores.ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _dbContext.Doctores
                .Include(d => d.Citas)
                .FirstOrDefaultAsync(d => d.IdDoctor == id);
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            if (!EsValidoNombreApellido(doctor.Nombre) || !EsValidoNombreApellido(doctor.Apellido))
                throw new ValidationException("El nombre y apellido deben contener solo letras.");

            _dbContext.Doctores.Add(doctor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor, Doctor existingDoctor)
        {
            if (doctor.Nombre != existingDoctor.Nombre || doctor.Apellido != existingDoctor.Apellido)
                throw new ValidationException("No se permite cambiar el nombre o apellido del doctor.");

            existingDoctor.Especialidad = doctor.Especialidad;

            _dbContext.Doctores.Update(existingDoctor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(Doctor doctor)
        {
            if (doctor.Citas != null && doctor.Citas.Count > 0) // Reemplazo de Any() por Count > 0
                throw new ValidationException("No se puede eliminar el doctor porque tiene citas asociadas.");

            _dbContext.Doctores.Remove(doctor);
            await _dbContext.SaveChangesAsync();
        }

        // Validación: Solo letras
        private static bool EsValidoNombreApellido(string input) // Método marcado como static
        {
            return input.All(char.IsLetter);
        }
    }
}
