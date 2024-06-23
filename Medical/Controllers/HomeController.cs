using Medical.Data.Entities;
using Medical.Models;
using Medical.Repositories;
using Medical.Specifications.ApplicationUser_;
using Medical.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace Medical.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationUserRepo _userRepo;

        public HomeController(ILogger<HomeController> logger, ApplicationUserRepo repo)
        {
            _logger = logger;
            _userRepo = repo;
        }

        [AllowAnonymous]
        public IActionResult Ads() =>View();
        
        [AllowAnonymous]
        public new ActionResult NotFound() =>View();

        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var users = await _userRepo.GetAllAsync(new ApplicationUserSpecifications(new()));
                var user = users?.FirstOrDefault(u => u.UserName == User.Identity?.Name);

                var adminModel = new HomeAdminsVM()
                {
                    User = user,
                    TotalAdmins = users.Count(u => u.UserRole == UserRole.Admin),
                    TotalDoctors = users.Count(u => u.UserRole == UserRole.Doctor),
                    TotalPatients = users.Count(u => u.UserRole == UserRole.Patient),
                };

                return View("Admins", adminModel);
            }
            else if (User.IsInRole("Doctor"))
            {
                return View("Doctors");
            }
            else if (User.IsInRole("Patient"))
            {
                var specs = new BaseGlobalSpecs<ApplicationUserNavigations, ApplicationUserSearch>()
                {
                    Search = new() { UserName = User.Identity.Name },
                    Navigations = new() { EnableMedicalsReports = true }
                };

                var patient = await _userRepo.GetAsync(new(specs));

                if (patient == null) { NotFound(); }

                return View("Patients",patient);
            }

            return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
