using Medical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Medical.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public string NationalId { get; set; }
        public string? QR { get; set; }

        public UserRole UserRole { get; set; }

        public string? ImagePath { get; set; }

        public DateOnly? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? Specification { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public IList<PatientMedical>? Medicals { get; set; }
    }

    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.NationalId).IsUnique();
        }
    }

    public enum Gender
    {
        male,
        femail
    }
}
