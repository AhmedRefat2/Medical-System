using Medical.Repositories;
using Medical.Specifications.PatientMedical_;
using Medical.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Medical.Models;
using Medical.Data.Entities;
using Medical.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Medical.Controllers
{
    public class PatientMedicalsController : Controller
    {
        private readonly PatientMedicalRepo _patientMedicalRepo;
        private readonly ApplicationUserRepo _applicationUserRepo;
        private readonly IConfiguration _configuration;

        public PatientMedicalsController(PatientMedicalRepo patientMedicalRepo, ApplicationUserRepo applicationUserRepo, IConfiguration configuration)
        {
            _patientMedicalRepo = patientMedicalRepo;
            _applicationUserRepo = applicationUserRepo;
            _configuration = configuration;
        }

        // GET: PatientMedicalsController
        public async Task<IActionResult> Index(int patientId, BaseGlobalSpecs<PatientMedicalNavigations, PatientMedicalSearch>? specs)
        {
            specs ??= new();
            specs.Search ??= new() { PatientId = patientId };
            var specification = new PatientMedicalSpecifications(specs);
            ViewBag.Patient = await _applicationUserRepo.GetByIdAsync(patientId);

            var entities = await _patientMedicalRepo.GetAllAsync(specification, specs.Pagination);
            return View(entities);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: PatientMedicalsController/Create
        public async Task<IActionResult> Create(int patientId)
        {
            var user = await _applicationUserRepo.GetByIdAsync(patientId);
            return View(new PatientMedicalCreateVM { PatientId = patientId, Patient = user });
        }

        [Authorize(Roles = "Admin,Doctor")]
        // POST: PatientMedicalsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientMedicalCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var patientMedical = new PatientMedical
                {
                    PatientId = model.PatientId,
                    Type = model.Type,
                    Text = model.Text,
                };

                if (model.File != null)
                {
                    var fileName = GetValidatedFileName(model.File.FileName);
                    var folderPath = _configuration["reports:upload"];

                    patientMedical.FileName = DocumentSettings.SaveFile(fileName, model.File, folderPath);
                }

                await _patientMedicalRepo.AddAsync(patientMedical);

                if (await _patientMedicalRepo.CommitAsync() > 0)
                {
                    TempData["Message"] = $"Report {patientMedical.Type}, Added Successfully";
                    return RedirectToAction(nameof(Index), new { patientId = model.PatientId });
                }

                ModelState.AddModelError("", "Data Not Valid");
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the error (uncomment the line below after adding a logger)
                // _logger.LogError(ex, "Error occurred while creating a patient medical record.");
                ModelState.AddModelError("", "Something Went Wrong");
                return View(model);
            }
        }

        private string GetValidatedFileName(string fileName)
        {
            return Path.GetFileName(fileName.Length > 200 ? fileName.Substring(0, 200) : fileName);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: PatientMedicalsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var patientMedical = await _patientMedicalRepo.GetAsync(new(new() { Search = new() { Id = id }, Navigations = new() { EnablePatient = true } }));
            if (patientMedical == null)
            {
                return NotFound();
            }

            var model = new PatientMedicalEditVM
            {
                Id = patientMedical.Id,
                Patient = patientMedical.Patient,
                PatientId = patientMedical.PatientId,
                Type = patientMedical.Type,
                Text = patientMedical.Text,
                FileName = patientMedical.FileName
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // POST: PatientMedicalsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientMedicalEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var patientMedical = await _patientMedicalRepo.GetByIdAsync(id);
                if (patientMedical == null)
                {
                    return NotFound();
                }

                patientMedical.Type = model.Type;
                patientMedical.Text = model.Text;
                patientMedical.UpdatedAt = DateTime.UtcNow;

                if (model.File != null)
                {
                    var fileName = GetValidatedFileName(model.File.FileName);
                    var folderPath = _configuration["reports:upload"];

                    if (!string.IsNullOrEmpty(patientMedical.FileName))
                        DocumentSettings.RemoveFile(Path.Combine(folderPath, patientMedical.FileName));

                    patientMedical.FileName = DocumentSettings.SaveFile(fileName, model.File, folderPath);
                }

                _patientMedicalRepo.Update(patientMedical);

                if (await _patientMedicalRepo.CommitAsync() > 0)
                {
                    TempData["Message"] = $"Report {patientMedical.Type}, Updated Successfully";
                    return RedirectToAction(nameof(Index), new { patientId = model.PatientId });
                }

                ModelState.AddModelError("", "Data Not Valid");
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the error (uncomment the line below after adding a logger)
                // _logger.LogError(ex, "Error occurred while editing a patient medical record.");
                ModelState.AddModelError("", "Something Went Wrong");
                return View(model);
            }
        }


        [Authorize(Roles = "Admin,Doctor")]
        // POST: PatientMedicalsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patientMedical = await _patientMedicalRepo.GetByIdAsync(id);
                if (patientMedical == null)
                {
                    return NotFound();
                }

                _patientMedicalRepo.Delete(patientMedical);

                if (await _patientMedicalRepo.CommitAsync() > 0)
                {
                    if (!string.IsNullOrEmpty(patientMedical.FileName))
                    {
                        var folderPath = _configuration["reports:upload"];
                        DocumentSettings.RemoveFile(Path.Combine(folderPath, patientMedical.FileName));
                    }
                    TempData["Message"] = $"Report {patientMedical.Type}, Deleted Successfully";
                    return RedirectToAction(nameof(Index), new { patientId = patientMedical.PatientId });
                }

                ModelState.AddModelError("", "Data Not Valid");
                return View(patientMedical);
            }
            catch (Exception ex)
            {
                // Log the error (uncomment the line below after adding a logger)
                // _logger.LogError(ex, "Error occurred while deleting a patient medical record.");
                ModelState.AddModelError("", "Something Went Wrong");
                return View();
            }
        }
    }
}
