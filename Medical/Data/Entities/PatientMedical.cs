using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical.Data.Entities
{
    public class PatientMedical
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public ApplicationUser Patient { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; }

        public string? Text { get; set; }
        public string? FileName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
