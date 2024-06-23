using Medical.Models;
using Medical.Repositories;
using Medical.Specifications;
using Medical.Specifications.ApplicationUser_;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationUserRepo _userRepo;

        public DoctorsController(ApplicationUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(BaseGlobalSpecs<ApplicationUserNavigations, ApplicationUserSearch> specs)
        {
            specs.Search ??= new() { UserRole = UserRole.Doctor };

            var specification = new ApplicationUserSpecifications(specs);

            var entities = await _userRepo.GetAllAsync(specification, specs.Pagination);
            return View(entities);
        }

        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> Search(string nationalId)
        {
            if(nationalId == null)
            {
                return View("~/Views/Home/Doctors.cshtml");
            }
            // Create specifications for searching users
            var specs = new BaseGlobalSpecs<ApplicationUserNavigations, ApplicationUserSearch>
            {
                Search = new ApplicationUserSearch { UserRole = UserRole.Patient, NationalId = nationalId }
            };

            // Create specifications object
            var specification = new ApplicationUserSpecifications(specs);

            // Get users based on the specifications
            var entities = await _userRepo.GetAllAsync(specification);
            var userViewModels = entities.Select(user => new ApplicationUserVM(user)).ToList();

            ViewBag.patientNationalID = nationalId;

            // Return the view with the search results
            // Navigate to the "Doctors" view in the "Home" controller
            return View("~/Views/Home/Doctors.cshtml", userViewModels);
        }

    }
}
