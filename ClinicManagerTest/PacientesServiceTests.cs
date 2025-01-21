using ClinicManager.Models;
using ClinicManager.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System;
using Xunit;
using ClinicManager;

namespace ClinicManagerTest.Services
{
    public class PacientesServiceTests
    {
        private readonly PacientesService _service;
        private readonly Mock<DbSet<Paciente>> _mockSet;
        private readonly Mock<AppDBContext> _mockContext;

        public PacientesServiceTests()
        {
            _mockSet = new Mock<DbSet<Paciente>>();
            _mockContext = new Mock<AppDBContext>();
            _mockContext.Setup(m => m.Pacientes).Returns(_mockSet.Object);

            _service = new PacientesService(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllPacientesAsync_ShouldReturnList()
        {
            // Act
            var result = await _service.GetAllPacientesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Paciente>>(result);
        }

        [Fact]
        public async Task AddPacienteAsync_InvalidTelefono_ShouldThrowException()
        {
            // Arrange
            var paciente = new Paciente { Nombre = "Juan", Apellido = "Perez", Telefono = "12345678901", Edad = 30 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddPacienteAsync(paciente));
            Assert.Equal("Teléfono inválido. Debe contener solo números y no exceder 10 dígitos.", exception.Message);
        }


        [Fact]

        public async Task DeletePacienteAsync_WithCitas_ShouldThrowException()
        {
            // Arrange
            var paciente = new Paciente
            {
                IdPaciente = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Citas = new List<Cita> { new Cita { IdCita = 1, Lugar = "Consulta" } }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeletePacienteAsync(paciente));
            Assert.Equal("No se puede eliminar el paciente porque tiene citas asociadas.", exception.Message);
        }
    }
}
