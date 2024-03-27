using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;
        public INotyfService _notyfService { get; }

        public FacultyController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            //_mapper = mapper;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            List<Faculty> facultyList = _unitOfWork.FacultyRepository.GetAll2().ToList();
            return View(facultyList);

        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Faculty faculty)
        {

            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully create a new Faculty");
                await _unitOfWork.FacultyRepository.Add(faculty);
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
            Faculty? facultyFromDb = _unitOfWork.FacultyRepository.Get(u => u.Id == id);

            if (facultyFromDb == null)
            {
                return NotFound();
            }
            return View(facultyFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully update a Faculty");
                await _unitOfWork.FacultyRepository.Update(faculty);
                return RedirectToAction("Index");
            }
            return View();
        }

        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Faculty? facultyFromDb = _unitOfWork.FacultyRepository.Get(u => u.Id == id);

            if (facultyFromDb == null)
            {
                return NotFound();
            }
            return View(facultyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Faculty? obj = _unitOfWork.FacultyRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _notyfService.Success("You successfully delete a Faculty");
            await _unitOfWork.FacultyRepository.Delete(obj);
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Faculty> facultyList = _unitOfWork.FacultyRepository.GetAll2().ToList();
            return Json(new { data = facultyList });
        }


        #endregion
    }
}
