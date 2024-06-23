using Medical.Data.Entities;

namespace Medical.Models
{
    public class PatientProfile
    {
        public ApplicationUserVM Patient {  get; set; }

        public IList<PatientMedical> Medicals { get; set; }
    }
}
