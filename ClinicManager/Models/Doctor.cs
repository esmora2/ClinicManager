using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicManager.Models
{
    public class Doctor
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDoctor { get; set; } // Se cambia a nullable para evitar problemas con valores predeterminados

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
        public string Especialidad { get; set; } = string.Empty;

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
