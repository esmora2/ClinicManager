using ClinicManager.Models;
using ClinicManager.Services;
using ClinicManager.Exceptions; // Para excepciones personalizadas
using Microsoft.AspNetCore.Mvc;

namespace ClinicManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedimientosController : ControllerBase
    {
        private readonly ProcedimientosService _procedimientosService;

        // Constante para el mensaje de error interno
        private const string InternalServerError = "Error interno del servidor";

        public ProcedimientosController(ProcedimientosService procedimientosService)
        {
            _procedimientosService = procedimientosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var procedimientos = await _procedimientosService.GetAllProcedimientosAsync();
            return Ok(procedimientos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var procedimiento = await _procedimientosService.GetProcedimientoByIdAsync(id);
                if (procedimiento == null)
                    return NotFound(new { Message = "Procedimiento no encontrado" });

                return Ok(procedimiento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerError, Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Procedimiento procedimiento)
        {
            try
            {
                await _procedimientosService.AddProcedimientoAsync(procedimiento);
                return CreatedAtAction(nameof(GetById), new { id = procedimiento.IdProcedimiento }, procedimiento);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Procedimiento procedimiento)
        {
            if (id != procedimiento.IdProcedimiento)
                return BadRequest(new { Message = "El ID no coincide" });

            try
            {
                await _procedimientosService.UpdateProcedimientoAsync(procedimiento);
                return Ok(procedimiento);
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
                var procedimiento = await _procedimientosService.GetProcedimientoByIdAsync(id);
                if (procedimiento == null)
                    return NotFound(new { Message = "Procedimiento no encontrado" });

                await _procedimientosService.DeleteProcedimientoAsync(procedimiento);
                return Ok(new { Message = "Procedimiento eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = InternalServerError, Details = ex.Message });
            }
        }
    }
}
