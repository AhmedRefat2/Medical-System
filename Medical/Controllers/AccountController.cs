using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Medical.Data.Entities;
using Medical.Models;
using Medical.Helper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Medical.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(loginVM.UserName);
                    if (user != null)
                    {
                        var result = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                        if (result)
                        {
                            await _signInManager.SignInAsync(user, false);

                            return RedirectToAction("Index", "Home");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                }
            }

            return View(loginVM);
        }

        public async Task<ActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create(UserRole? userRole)
        {
            return View(new CreateUserVM() { Role = userRole ?? UserRole.Patient });
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM createUserVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(createUserVM.UserName);
                    if (user != null)
                    {
                        ModelState.AddModelError("", "User already exists.");
                        return View(createUserVM);
                    }

                    user = new ApplicationUser
                    {
                        UserName = createUserVM.UserName,
                        Email = createUserVM.Email,
                        NationalId = createUserVM.NationalId,
                        Name = createUserVM.Name,
                        PhoneNumber = createUserVM.PhoneNumber,
                        UserRole = createUserVM.Role,
                        Address = createUserVM.Address,
                        BirthDate = createUserVM.BirthDate,
                        Gender = createUserVM.Gender,
                        Specification = createUserVM.Specification, 
                        
                    };

                    var result = await _userManager.CreateAsync(user, createUserVM.Password);
                    if (result.Succeeded)
                    {
                        if (createUserVM.Image != null)
                        {
                            var path = _configuration["images:upload"];
                            user.ImagePath = DocumentSettings.SaveFile(user.Name, createUserVM.Image, path);

                            var qrCodeImageUrl = GenerateQrCodeImage(user.Id);
                            user.QR = DocumentSettings.SaveFile(user.Name, qrCodeImageUrl, path);
                            await _userManager.UpdateAsync(user);
                        }

                        TempData["Message"] = $"User {user.Name}, Added Successfully";

                        switch (createUserVM.Role)
                        {
                            case UserRole.Admin:
                                await _userManager.AddToRoleAsync(user, "Admin");
                                return RedirectToAction("Index", "Admins");
                            case UserRole.Doctor:
                                await _userManager.AddToRoleAsync(user, "Doctor");
                                return RedirectToAction("Index", "Doctors");
                            case UserRole.Patient:
                                await _userManager.AddToRoleAsync(user, "Patient");
                                return RedirectToAction("Index", "Patients");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    // 2627 is the SQL Server error code for a unique constraint violation (duplicate key)
                    ModelState.AddModelError("", "A user with this information already exists.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Something went wrong. Please try again.");
                }
            }

            return View(createUserVM);
        }

        private byte[] GenerateQrCodeImage(int userId)
        {
            // Generate QR code
            string url = Url.Action("Profile", "Account", new { id = userId }, Request.Scheme);
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImageBytes = qrCode.GetGraphic(20);

            // Return the byte array of the QR code image
            return qrCodeImageBytes;
        }

        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                var model = new EditUserVM
                {
                    Name = user.Name,
                    Email = user.Email,
                    NationalId = user.NationalId,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.UserRole,
                    UserName = user.UserName,
                    Address = user.Address,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Specification = user.Specification,

                };
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, EditUserVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.NationalId = model.NationalId;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.UserName;
                    user.Address = model.Address;
                    user.BirthDate = model.BirthDate;
                    user.Gender = model.Gender;
                    user.Specification = model.Specification; 
                        
                    user.UpdatedAt = DateTime.UtcNow;

                    if (model.Image != null)
                    {
                        var path = _configuration["images:upload"];

                        if (!string.IsNullOrEmpty(user.ImagePath))
                        {
                            DocumentSettings.RemoveFile(path + "/" + user.ImagePath);
                        }
                        user.ImagePath = DocumentSettings.SaveFile(user.Name, model.Image, path);
                    }

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var res = await _userManager.ResetPasswordAsync(user, token, model.Password);

                        if (!res.Succeeded)
                        {
                            foreach (var error in res.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(model);
                        }
                    }
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["Message"] = $"User {user.Name}, Updated Successfully";

                        switch (user.UserRole)
                        {
                            case UserRole.Admin:
                                return RedirectToAction("Index", "Admins");
                            case UserRole.Doctor:
                                return RedirectToAction("Index", "Doctors");
                            case UserRole.Patient:
                                return RedirectToAction("Index", "Patients");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    ModelState.AddModelError("", "A user with this information already exists.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Something went wrong. Please try again.");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                // Remove associated image file if it exists
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    var path = _configuration["images:upload"];
                    DocumentSettings.RemoveFile(Path.Combine(path, user.ImagePath));
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    switch (user.UserRole)
                    {
                        case UserRole.Admin:
                            return RedirectToAction("Index", "Admins");
                        case UserRole.Doctor:
                            return RedirectToAction("Index", "Doctors");
                        case UserRole.Patient:
                            return RedirectToAction("Index", "Patients");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(user);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View();
            }
        }

        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            if (user.UserRole == UserRole.Patient)
                return RedirectToAction(nameof(PatientsController.Profile), "Patients", new { patientId = user.Id });


            var model = new ApplicationUserVM(user);

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
