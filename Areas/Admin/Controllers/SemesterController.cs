using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SemesterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notyfService { get; }

        public SemesterController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _notyfService = notyfService;
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<Semester> semesterList = _unitOfWork.SemesterRepository.GetAll2().ToList();
            return View(semesterList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Semester semester)
        {

            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully create a new Semester");
                await _unitOfWork.SemesterRepository.Add(semester);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Semester? semesterFromDb = _unitOfWork.SemesterRepository.Get(u => u.Id == id);


            if (semesterFromDb == null)
            {
                return NotFound();
            }
            return View(semesterFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Semester semester)
        {
            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully update a Semester");
                await _unitOfWork.SemesterRepository.Update(semester);
                return RedirectToAction("Index");
            }
            return View();
        }

        // Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Semester? semesterFromDb = _unitOfWork.SemesterRepository.Get(u => u.Id == id);

            if (semesterFromDb == null)
            {
                return NotFound();
            }
            return View(semesterFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Semester? semesterFromDb = _unitOfWork.SemesterRepository.Get(u => u.Id == id);
            if (semesterFromDb == null)
            {
                return NotFound();
            }
            _notyfService.Success("You successfully delete a Semester");
            await _unitOfWork.SemesterRepository.Delete(semesterFromDb);
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Semester> semesterList = _unitOfWork.SemesterRepository.GetAll2().ToList();
            return Json(new { data = semesterList });
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    var semesterToBeDeleted = _unitOfWork.SemesterRepository.Get(u => u.Id == id);
        //    if (semesterToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    await _unitOfWork.SemesterRepository.Delete(semesterToBeDeleted);
        //    return Json(new { success = true, message = "Delete Successful" });
        //}

    }
}