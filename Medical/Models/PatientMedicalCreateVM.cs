using Medical.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical.Models
{
    public class PatientMedicalCreateVM
    {
        public int Id { get; set; }

        public ApplicationUser ?Patient { get; set; }
        public int PatientId { get; set; }
        public string Type { get; set; }

        public string? Text { get; set; }
        public IFormFile? File { get; set; }
    }

    public class PatientMedicalEditVM : PatientMedicalCreateVM
    {
        public string? FileName { get; set; }
    }
}
