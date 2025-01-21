using ClinicManager.Controllers;
using ClinicManager.Exceptions;
using ClinicManager.Models;
using ClinicManager.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ClinicManagerTest
{
    public class CitasControllerTests
    {
        private readonly Mock<CitasService> _citasServiceMock;
        private readonly CitasController _citasController;

        public CitasControllerTests()
        {
            _citasServiceMock = new Mock<CitasService>();
            _citasController = new CitasController(_citasServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfCitas()
        {
            // Arrange
            var citas = new List<Cita>
            {
                new Cita { IdCita = 1, Lugar = "Consultorio 101" },
                new Cita { IdCita = 2, Lugar = "Consultorio 102" }
            };
            _citasServiceMock.Setup(service => service.GetAllCitasAsync()).ReturnsAsync(citas);

            // Act
            var result = await _citasController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCitas = Assert.IsType<List<Cita>>(okResult.Value);
            Assert.Equal(2, returnedCitas.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCitaDoesNotExist()
        {
            // Arrange
            _citasServiceMock.Setup(service => service.GetCitaByIdAsync(1)).ReturnsAsync((Cita?)null);

            // Act
            var result = await _citasController.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
            Assert.Equal("Cita no encontrada", notFoundResult.Value?.ToString());
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenCitaExists()
        {
            // Arrange
            var cita = new Cita { IdCita = 1, Lugar = "Consultorio 101" };
            _citasServiceMock.Setup(service => service.GetCitaByIdAsync(1)).ReturnsAsync(cita);

            // Act
            var result = await _citasController.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCita = Assert.IsType<Cita>(okResult.Value);
            Assert.Equal(1, returnedCita.IdCita);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionResult_WhenValidCitaIsProvided()
        {
            // Arrange
            var cita = new Cita { Fecha = DateTime.Now.AddDays(1), IdPaciente = 1, IdDoctor = 1, Lugar = "Consultorio 101" };
            _citasServiceMock.Setup(service => service.AddCitaAsync(cita)).Returns(Task.CompletedTask);

            // Act
            var result = await _citasController.Create(cita);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CitasController.GetById), createdAtResult.ActionName);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenPastDateExceptionIsThrown()
        {
            // Arrange
            var cita = new Cita { Fecha = DateTime.Now.AddDays(-1), IdPaciente = 1, IdDoctor = 1, Lugar = "Consultorio 101" };
            _citasServiceMock.Setup(service => service.AddCitaAsync(cita))
                .ThrowsAsync(new PastDateException("No se pueden programar citas con fechas pasadas."));

            // Act
            var result = await _citasController.Create(cita);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Contains("No se pueden programar citas con fechas pasadas.", badRequestResult.Value?.ToString());
        }
    }
}
