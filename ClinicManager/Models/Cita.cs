using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicManager.Models
{
    public class Cita
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonRequired] // Asegura que el cliente envíe este valor en la solicitud
        public int IdCita { get; set; }



        [Required]
        [JsonRequired]
        public DateTime? Fecha { get; set; } // Ahora es anulable

        [Required]
        [JsonRequired]
        public int? IdPaciente { get; set; } // Ahora es anulable
        public Paciente? Paciente { get; set; }

        [Required]
        [JsonRequired]
        public int? IdDoctor { get; set; } // Ahora es anulable
        public Doctor? Doctor { get; set; }

        [Required]
        public string Lugar { get; set; } = string.Empty;

        public ICollection<Procedimiento> Procedimientos { get; set; } = new List<Procedimiento>();
    }
}
