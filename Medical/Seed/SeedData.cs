using Medical.Data.Entities;
using Medical.Helper;
using Medical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Data
{
    public static class Seeder
    {
        public static async Task Seed(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            if (userManager.Users.Any())
            {
                return;
            }

            // Seed users
            var users = new[]
            {
                new
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    Name = "Admin User",
                    NationalId = "11111111111111",
                    PhoneNumber = "1234567890",
                    Role = UserRole.Admin,
                    Password = "Admin123!",
                    Gender = Gender.male,
                    Address = "123 Admin St.",
                    Specification = "Administrator",
                    BirthDate = new DateOnly(1980, 1, 1),
                    ImagePath = "/images/admin.png"
                },
                new
                {
                    UserName = "doctor",
                    Email = "doctor@example.com",
                    Name = "Doctor User",
                    NationalId = "22222222222222",
                    PhoneNumber = "1234567891",
                    Role = UserRole.Doctor,
                    Password = "Doctor123!",
                    Gender = Gender.male,
                    Address = "456 Doctor Ave.",
                    Specification = "General Practitioner",
                    BirthDate = new DateOnly(1985, 5, 15),
                    ImagePath = "/images/doctor.png"
                },
                new
                {
                    UserName = "patient",
                    Email = "patient@example.com",
                    Name = "Patient User",
                    NationalId = "33333333333333",
                    PhoneNumber = "1234567892",
                    Role = UserRole.Patient,
                    Password = "Patient123!",
                    Gender = Gender.femail,
                    Address = "789 Patient Blvd.",
                    Specification = "N/A",
                    BirthDate = new DateOnly(1990, 8, 25),
                    ImagePath = "/images/patient.png"
                }
            };

            foreach (var userInfo in users)
            {
                if (await userManager.FindByNameAsync(userInfo.UserName) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userInfo.UserName,
                        Email = userInfo.Email,
                        Name = userInfo.Name,
                        NationalId = userInfo.NationalId,
                        PhoneNumber = userInfo.PhoneNumber,
                        UserRole = userInfo.Role,
                        Gender = userInfo.Gender,
                        Address = userInfo.Address,
                        Specification = userInfo.Specification,
                        BirthDate = userInfo.BirthDate,
                        ImagePath = userInfo.ImagePath,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role.ToString());

                        // Generate and save QR code
                        var qrCodeImageUrl = GenerateQrCodeImage(user.Id, configuration["baseUrl"]);
                        var path = configuration["images:upload"];
                        user.QR = DocumentSettings.SaveFile(user.Name, qrCodeImageUrl, path);
                        await userManager.UpdateAsync(user);
                    }
                }
            }
        }

        private static byte[] GenerateQrCodeImage(int userId, string baseUrl)
        {
            // Generate QR code
            string url = $"{baseUrl}/Account/Profile/{userId}";
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImageBytes = qrCode.GetGraphic(20);

            // Return the byte array of the QR code image
            return qrCodeImageBytes;
        }
    }
}
