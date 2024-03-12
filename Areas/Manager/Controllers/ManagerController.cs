using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public INotyfService _notyfService { get; }
        public ManagerController(IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
               return View();
        }
        [HttpGet]
        public async Task<IActionResult> FacultyArticlesAsync(int facultyId)
        {
            var faculty = await _unitOfWork.FacultyRepository.GetById(facultyId);
            var facultyArticles = await _unitOfWork.MegazineRepository.GetById(facultyId);
            var viewFacultyArticles = _mapper.Map<List<GetFacultyModel>>(facultyArticles);
            ViewBag.FacultyArticles = new SelectList(viewFacultyArticles, "FacultyId", "Name");
            ViewData["Message"] = "Faculty Articles Page";
            return View();
        }
    }
}
