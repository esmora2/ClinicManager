using ClinicManager.Models;
using ClinicManager.Services;
using ClinicManager.Exceptions; // Para manejar excepciones personalizadas
using Microsoft.AspNetCore.Mvc;

namespace ClinicManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly PacientesService _pacientesService;
        private const string InternalServerErrorMessage = "Error interno del servidor";

        public PacientesController(PacientesService pacientesService)
        {
            _pacientesService = pacientesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pacientes = await _pacientesService.GetAllPacientesAsync();
                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerErrorMessage, Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paciente = await _pacientesService.GetPacienteByIdAsync(id);
                if (paciente == null)
                    throw new NotFoundException("Paciente no encontrado");

                return Ok(paciente);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerErrorMessage, Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            try
            {
                await _pacientesService.AddPacienteAsync(paciente);
                return CreatedAtAction(nameof(GetById), new { id = paciente.IdPaciente }, paciente);
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
        public async Task<IActionResult> Update(int id, Paciente paciente)
        {
            if (id != paciente.IdPaciente)
                return BadRequest(new { Message = "El ID no coincide" });

            try
            {
                var existingPaciente = await _pacientesService.GetPacienteByIdAsync(id);
                if (existingPaciente == null)
                    throw new NotFoundException("Paciente no encontrado");

                await _pacientesService.UpdatePacienteAsync(paciente, existingPaciente);
                return Ok(existingPaciente);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerErrorMessage, Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var paciente = await _pacientesService.GetPacienteByIdAsync(id);
                if (paciente == null)
                    throw new NotFoundException("Paciente no encontrado");

                await _pacientesService.DeletePacienteAsync(paciente);
                return Ok(new { Message = "Paciente eliminado exitosamente" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerErrorMessage, Details = ex.Message });
            }
        }
    }
}
