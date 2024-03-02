using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountController(IUserAuthenticationService service, UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _service = service;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            var facultyList = await _unitOfWork.FacultyRepository.GetAll();
            var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(facultyList);
            ViewBag.FacultyList = new SelectList(viewFacultyList, "FacultyId", "Name");
            ViewData["Message"] = "Create Account Page";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var facultyList = await _unitOfWork.FacultyRepository.GetAll();
                var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(facultyList);
                ViewBag.FacultyList = new SelectList(viewFacultyList, "FacultyId", "Name");
                return View(model);
            }
            model.Role = "User";
            model.FacultyId = model.FacultyId;
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction("index");
        }
    }
}
