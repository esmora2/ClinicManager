using ClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManager
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Procedimiento> Procedimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración específica para Procedimiento
            modelBuilder.Entity<Procedimiento>()
                .Property(p => p.Costo)
                .HasPrecision(18, 2);

            // Configuración de relaciones entre entidades
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.IdPaciente);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Doctor)
                .WithMany(d => d.Citas)
                .HasForeignKey(c => c.IdDoctor);

            modelBuilder.Entity<Procedimiento>()
                .HasOne(p => p.Cita)
                .WithMany(c => c.Procedimientos)
                .HasForeignKey(p => p.IdCita);

            base.OnModelCreating(modelBuilder);
        }
    }
}
