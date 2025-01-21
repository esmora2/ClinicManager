using ClinicManager.Controllers;
using ClinicManager.Models;
using ClinicManager.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicManagerTest
{
    public class DoctoresControllerTests
    {
        private readonly Mock<DoctoresService> _mockDoctoresService;
        private readonly DoctoresController _controller;

        public DoctoresControllerTests()
        {
            _mockDoctoresService = new Mock<DoctoresService>();
            _controller = new DoctoresController(_mockDoctoresService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfDoctores()
        {
            // Arrange
            var doctores = new List<Doctor>
            {
                new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" },
                new Doctor { IdDoctor = 2, Nombre = "Ana", Apellido = "Gomez", Especialidad = "Pediatría" }
            };

            _mockDoctoresService.Setup(s => s.GetAllDoctoresAsync()).ReturnsAsync(doctores);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDoctores = Assert.IsType<List<Doctor>>(okResult.Value);
            Assert.Equal(2, returnedDoctores.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenDoctorExists()
        {
            // Arrange
            var doctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };

            _mockDoctoresService.Setup(s => s.GetDoctorByIdAsync(1)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDoctor = Assert.IsType<Doctor>(okResult.Value);
            Assert.Equal(1, returnedDoctor.IdDoctor);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            _mockDoctoresService.Setup(s => s.GetDoctorByIdAsync(1)).ReturnsAsync((Doctor?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenDoctorIsValid()
        {
            // Arrange
            var doctor = new Doctor { Nombre = "Sofia", Apellido = "Martinez", Especialidad = "Neurología" };

            // Act
            var result = await _controller.Create(doctor);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockDoctoresService.Verify(s => s.AddDoctorAsync(doctor), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var doctor = new Doctor { Nombre = "123", Apellido = "Martinez", Especialidad = "Neurología" };
            _mockDoctoresService.Setup(s => s.AddDoctorAsync(doctor)).ThrowsAsync(new Exception("Invalid input"));

            // Act
            var result = await _controller.Create(doctor);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid input", badRequest.Value);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenDoctorIsValid()
        {
            // Arrange
            var existingDoctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };
            var updatedDoctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Neurología" };

            _mockDoctoresService.Setup(s => s.GetDoctorByIdAsync(1)).ReturnsAsync(existingDoctor);

            // Act
            var result = await _controller.Update(1, updatedDoctor);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockDoctoresService.Verify(s => s.UpdateDoctorAsync(updatedDoctor, existingDoctor), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var doctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };

            // Act
            var result = await _controller.Update(2, doctor);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            var doctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };
            _mockDoctoresService.Setup(s => s.GetDoctorByIdAsync(1)).ReturnsAsync((Doctor?)null);

            // Act
            var result = await _controller.Update(1, doctor);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDoctorIsDeleted()
        {
            // Arrange
            var doctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };
            _mockDoctoresService.Setup(s => s.GetDoctorByIdAsync(1)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockDoctoresService.Verify(s => s.DeleteDoctorAsync(doctor), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            _mockDoctoresService.Setup(s => s.GetDoctorByIdAsync(1)).ReturnsAsync((Doctor?)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
