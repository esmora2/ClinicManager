using ClinicManager.Controllers;
using ClinicManager.Models;
using ClinicManager.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicManagerTest.Controllers
{
    public class PacientesControllerTests
    {
        private readonly Mock<PacientesService> _mockService;
        private readonly PacientesController _controller;

        public PacientesControllerTests()
        {
            _mockService = new Mock<PacientesService>(null!); // Usamos null ya que el servicio será simulado
            _controller = new PacientesController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllPacientesAsync())
                .ReturnsAsync(new List<Paciente> { new Paciente { Nombre = "Juan", Apellido = "Perez" } });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pacientes = Assert.IsType<List<Paciente>>(okResult.Value);
            Assert.Single(pacientes);
        }

        [Fact]
        public async Task Create_InvalidTelefono_ShouldReturnBadRequest()
        {
            // Arrange
            var paciente = new Paciente { Nombre = "Juan", Apellido = "Perez", Telefono = "12345678901", Edad = 30 };

            _mockService.Setup(service => service.AddPacienteAsync(paciente))
                .ThrowsAsync(new Exception("Teléfono inválido. Debe contener solo números y no exceder 10 dígitos."));

            // Act
            var result = await _controller.Create(paciente);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Teléfono inválido. Debe contener solo números y no exceder 10 dígitos.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeletePaciente_WithCitas_ShouldReturnBadRequest()
        {
            // Arrange
            var paciente = new Paciente { IdPaciente = 1, Nombre = "Juan", Apellido = "Perez" };

            _mockService.Setup(service => service.GetPacienteByIdAsync(1))
                .ReturnsAsync(paciente);
            _mockService.Setup(service => service.DeletePacienteAsync(paciente))
                .ThrowsAsync(new Exception("No se puede eliminar el paciente porque tiene citas asociadas."));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No se puede eliminar el paciente porque tiene citas asociadas.", badRequestResult.Value);
        }
    }
}
