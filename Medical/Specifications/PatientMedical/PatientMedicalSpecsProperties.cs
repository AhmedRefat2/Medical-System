using Medical.Models;
using System.ComponentModel.DataAnnotations;

namespace Medical.Specifications.PatientMedical_
{
    /// <summary>
    /// Specifies whether related entities should be included for User.
    /// </summary>
    public class PatientMedicalNavigations : GlobalSpecs
    {
        /// <summary>
        /// Gets or sets a value indicating whether Patient should be included.
        /// </summary>
        [Display(Name = "Enable Patient")]
        public bool EnablePatient { get; set; } = false;
    }

    /// <summary>
    /// Contains search criteria for filtering User entities.
    /// </summary>
    public class PatientMedicalSearch : GlobalSpecs
    {
        public int? Id { get; set; }
        public string? Type { get; set; }

        public string? Text { get; set; }

        public int? PatientId { get; set; }
    }

}
