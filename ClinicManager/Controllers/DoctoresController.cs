using ClinicManager.Models;
using ClinicManager.Services;
using ClinicManager.Exceptions; // Para excepciones personalizadas
using Microsoft.AspNetCore.Mvc;

namespace ClinicManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctoresController : ControllerBase
    {
        private readonly DoctoresService _doctoresService;

        // Definir constante para el mensaje repetido
        private const string InternalServerError = "Error interno del servidor";

        public DoctoresController(DoctoresService doctoresService)
        {
            _doctoresService = doctoresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctores = await _doctoresService.GetAllDoctoresAsync();
            return Ok(doctores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var doctor = await _doctoresService.GetDoctorByIdAsync(id);
                if (doctor == null)
                    return NotFound(new { Message = "Doctor no encontrado" });

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerError, Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            try
            {
                await _doctoresService.AddDoctorAsync(doctor);
                return CreatedAtAction(nameof(GetById), new { id = doctor.IdDoctor }, doctor);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
           
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Message = "Error interno del servidor",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Doctor doctor)
        {
            if (id != doctor.IdDoctor)
                return BadRequest(new { Message = "El ID no coincide" });

            try
            {
                var existingDoctor = await _doctoresService.GetDoctorByIdAsync(id);
                if (existingDoctor == null)
                    return NotFound(new { Message = "Doctor no encontrado" });

                await _doctoresService.UpdateDoctorAsync(doctor, existingDoctor);
                return Ok(existingDoctor);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerError, Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var doctor = await _doctoresService.GetDoctorByIdAsync(id);
                if (doctor == null)
                    return NotFound(new { Message = "Doctor no encontrado" });

                await _doctoresService.DeleteDoctorAsync(doctor);
                return Ok(new { Message = "Doctor eliminado exitosamente" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerError, Details = ex.Message });
            }
        }
    }
}
