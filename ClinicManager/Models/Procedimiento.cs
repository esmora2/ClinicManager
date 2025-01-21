using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicManager.Models
{
    public class Procedimiento
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonRequired] // Asegura que el cliente envíe este valor en la solicitud
        public int IdProcedimiento { get; set; } // Nullable para evitar problemas con valores predeterminados

        [Required]
        [JsonRequired]
        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [JsonRequired]
        [Range(0.0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0.")]
        public decimal? Costo { get; set; } // Nullable para evitar problemas con valores predeterminados

        [Required]
        [JsonRequired]
        public int? IdCita { get; set; } // Nullable para evitar problemas con valores predeterminados

        public Cita? Cita { get; set; }
    }
}
