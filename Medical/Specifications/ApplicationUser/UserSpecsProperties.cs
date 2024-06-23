using Medical.Models;
using System.ComponentModel.DataAnnotations;

namespace Medical.Specifications.ApplicationUser_
{
    /// <summary>
    /// Specifies whether related entities should be included for User.
    /// </summary>
    public class ApplicationUserNavigations : GlobalSpecs
    {
        /// <summary>
        /// Gets or sets a value indicating whether MedicalsReports should be included.
        /// </summary>
        [Display(Name = "Enable MedicalsReports")]
        public bool EnableMedicalsReports { get; set; }
    }

    /// <summary>
    /// Contains search criteria for filtering User entities.
    /// </summary>
    public class ApplicationUserSearch : GlobalSpecs
    {
        /// <summary>
        /// The ID of the User to filter.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The national ID of the User to filter.
        /// </summary>
        public string? NationalId { get; set; }

        /// <summary>
        /// The name of the User to filter.
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// The UserName of the User to filter.
        /// </summary>
        public string? UserName { get; set; }
        
        /// <summary>
        /// The email of the User to filter.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The phone number of the User to filter.
        /// </summary>
        public string? PhoneNumber { get; set; }

        public UserRole? UserRole { get; set; }
    }

}
