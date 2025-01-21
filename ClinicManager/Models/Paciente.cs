using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicManager.Models
{
    public class Paciente
    {
        [Key]
        [Required]
        [JsonRequired] // Asegura que el cliente envíe este valor en la solicitud
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPaciente { get; set; }
    

        [Required]
        [JsonRequired]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El nombre debe contener solo letras.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [JsonRequired]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El apellido debe contener solo letras.")]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [JsonRequired]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe contener exactamente 10 dígitos.")]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [JsonRequired]
        [Range(0, 100, ErrorMessage = "La edad debe estar entre 0 y 100.")]
        public int? Edad { get; set; } // Nullable para evitar problemas con valores predeterminados

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
