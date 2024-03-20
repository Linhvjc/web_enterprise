using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UniversityDbContext _db;
        public AccountController(IUserAuthenticationService service, UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper, UniversityDbContext db)
        {
            _service = service;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var facultyList = await _unitOfWork.FacultyRepository.GetAll();
            var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(facultyList);
            ViewBag.FacultyList = new SelectList(viewFacultyList, "FacultyId", "Name");
            ViewData["Message"] = "Create Account Page";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var facultyList = await _unitOfWork.FacultyRepository.GetAll();
                var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(facultyList);
                ViewBag.FacultyList = new SelectList(viewFacultyList, "FacultyId", "Name");
                return View(model);
            }
            model.Role = model.Role;
            model.FacultyId = model.FacultyId;
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction("index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> objUserList = _db.Users.Include(u => u.Faculty).ToList();

            var userRoles = _db.UserRoles.ToList(); // get the user roles
            var roles = _db.Roles.ToList(); // get the roles

            foreach (var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId; //get the role id of the user
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name; // get the role name of the user
            }
            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id) //[FromBody] is a parameter attribute that tells the framework to get the value from the request body
        {

            var objFromDb = _db.Users.FirstOrDefault(u => u.Id == id); // get the user from the database

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            /* LockoutEnd is a property of the IdentityUser class that represents the end of the lockout period for the user.
            If the user is not locked out, this value is null. */
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now) // if the user is currently locked
            {
                //user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now; // set the lockout end to the current time
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(100); // lock the user out for 100 years
            }
            _db.SaveChanges();

            return Json(new { success = true, message = "Operation Successful" });
        }

        #endregion
    }
}
