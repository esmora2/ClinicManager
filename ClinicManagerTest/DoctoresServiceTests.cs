using ClinicManager;
using ClinicManager.Models;
using ClinicManager.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ClinicManagerTest
{
    public class DoctoresServiceTests
    {
        private readonly Mock<AppDBContext> _mockDbContext;
        private readonly DoctoresService _doctoresService;
        private readonly Mock<DbSet<Doctor>> _mockSet;

        public DoctoresServiceTests()
        {
            _mockDbContext = new Mock<AppDBContext>();
            _mockSet = new Mock<DbSet<Doctor>>();
            _mockDbContext.Setup(m => m.Doctores).Returns(_mockSet.Object);
            _doctoresService = new DoctoresService(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetAllDoctoresAsync_ReturnsListOfDoctores()
        {
            // Arrange
            var doctores = new List<Doctor>
            {
                new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" },
                new Doctor { IdDoctor = 2, Nombre = "Ana", Apellido = "Gomez", Especialidad = "Pediatría" }
            }.AsQueryable();

            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(doctores.Provider);
            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(doctores.Expression);
            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(doctores.ElementType);
            _mockSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(doctores.GetEnumerator());

            // Act
            var result = await _doctoresService.GetAllDoctoresAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Juan", result[0].Nombre);
            Assert.Equal("Ana", result[1].Nombre);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };
            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(doctor);

            // Act
            var result = await _doctoresService.GetDoctorByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Juan", result!.Nombre);
        }

        [Fact]
        public async Task AddDoctorAsync_AddsDoctor_WhenValid()
        {
            // Arrange
            var doctor = new Doctor { Nombre = "Sofia", Apellido = "Martinez", Especialidad = "Neurología" };

            // Act
            await _doctoresService.AddDoctorAsync(doctor);

            // Assert
            _mockSet.Verify(m => m.Add(It.IsAny<Doctor>()), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AddDoctorAsync_ThrowsException_WhenNombreOrApellidoInvalid()
        {
            // Arrange
            var doctor = new Doctor { Nombre = "Juan123", Apellido = "Perez", Especialidad = "Cardiología" };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _doctoresService.AddDoctorAsync(doctor));
        }

        [Fact]
        public async Task UpdateDoctorAsync_UpdatesDoctor_WhenValid()
        {
            // Arrange
            var existingDoctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };
            var updatedDoctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Neurología" };

            // Act
            await _doctoresService.UpdateDoctorAsync(updatedDoctor, existingDoctor);

            // Assert
            Assert.Equal("Neurología", existingDoctor.Especialidad);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ThrowsException_WhenNombreOrApellidoChanged()
        {
            // Arrange
            var existingDoctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología" };
            var updatedDoctor = new Doctor { IdDoctor = 1, Nombre = "Carlos", Apellido = "Lopez", Especialidad = "Neurología" };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _doctoresService.UpdateDoctorAsync(updatedDoctor, existingDoctor));
        }

        [Fact]
        public async Task DeleteDoctorAsync_DeletesDoctor_WhenNoCitasExist()
        {
            // Arrange
            var doctor = new Doctor { IdDoctor = 1, Nombre = "Juan", Apellido = "Perez", Especialidad = "Cardiología", Citas = new List<Cita>() };

            // Act
            await _doctoresService.DeleteDoctorAsync(doctor);

            // Assert
            _mockSet.Verify(m => m.Remove(It.IsAny<Doctor>()), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_ThrowsException_WhenCitasExist()
        {
            // Arrange
            var doctor = new Doctor
            {
                IdDoctor = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Especialidad = "Cardiología",
                Citas = new List<Cita> { new Cita { IdCita = 1, Lugar = "Hospital" } }
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _doctoresService.DeleteDoctorAsync(doctor));
        }
    }
}
