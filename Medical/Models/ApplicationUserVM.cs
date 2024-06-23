using Medical.Data.Entities;

namespace Medical.Models
{
    public class ApplicationUserVM
    {
        public ApplicationUserVM(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            NationalId = user.NationalId;
            Name = user.Name;
            PhoneNumber = user.PhoneNumber;
            ImageUrl = user.ImagePath;
            QrCodeUrl = user.QR;
            UserRole = user.UserRole;
            Specification = user.Specification;
            Address = user.Address;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
            BirthDate = user.BirthDate;
        }

        public ApplicationUserVM()
        {
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }

        public UserRole UserRole { get; set; }
        public string QrCodeUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateOnly? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? Specification { get; set; }
    }

    public class PatientUserVM : ApplicationUserVM
    {
        public PatientUserVM()
        {
        }

        public PatientUserVM(ApplicationUser user, IEnumerable<PatientMedical>? medicals) : base(user)
        {
            Medicals = medicals;
        }

        public IEnumerable<PatientMedical>? Medicals { get; set; }
    }
}
