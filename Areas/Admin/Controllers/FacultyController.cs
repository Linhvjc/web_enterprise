using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public INotyfService _notyfService { get; }

        public FacultyController(IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index() { return View(); }

        [HttpGet]
        public IActionResult CreateFaculty()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaculty([Bind("Name,Description")] CreateFacultyModel facultyModel)
        {
            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully create a new Faculty");
                var faculty = _mapper.Map<Faculty>(facultyModel);
                await _unitOfWork.FacultyRepository.Add(faculty);
                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }
    }
}
