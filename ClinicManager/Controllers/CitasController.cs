using ClinicManager.Exceptions;
using ClinicManager.Models;
using ClinicManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly CitasService _citasService;

        public CitasController(CitasService citasService)
        {
            _citasService = citasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var citas = await _citasService.GetAllCitasAsync();
            return Ok(citas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _citasService.GetCitaByIdAsync(id);
            if (cita == null)
                return NotFound(new { Message = "Cita no encontrada" });

            return Ok(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cita cita)
        {
            try
            {
                await _citasService.AddCitaAsync(cita);
                return CreatedAtAction(nameof(GetById), new { id = cita.IdCita }, cita);
            }
            catch (PastDateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cita cita)
        {
            if (id != cita.IdCita)
                return BadRequest(new { Message = "El ID no coincide" });

            try
            {
                await _citasService.UpdateCitaAsync(cita);
                return Ok(cita);
            }
            catch (PastDateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _citasService.GetCitaByIdAsync(id);
            if (cita == null)
                return NotFound(new { Message = "Cita no encontrada" });

            try
            {
                await _citasService.DeleteCitaAsync(cita);
                return Ok(new { Message = "Cita eliminada exitosamente" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
