using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels;

namespace WebEnterprise.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public INotyfService _notyfService { get; }

        public AuthenticationController(IUserAuthenticationService service, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, INotyfService notyfService)
        {
            _service = service;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _notyfService = notyfService;
        }

        public IActionResult Register()
        {
            ViewData["Message"] = "Registration Page";
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _userManager.FindByEmailAsync(user.Email);
                var result = await _service.LoginAsync(user);
                if (result.StatusCode == 1)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("Email", user.Email);
                    _httpContextAccessor.HttpContext.Session.SetString("UserId", checkUser.Id);
                    _httpContextAccessor.HttpContext.Session.SetString("UserName", checkUser.FullName);
                    _httpContextAccessor.HttpContext.Session.SetInt32("FacultyId", checkUser.FacultyId);
                    _notyfService.Success("Login Successfully");
                    return RedirectToAction("Index", "Home");
                    
                }
                else
                {
                    TempData["msg"] = result.Message;
                    return View(user);
                }
            }
            else
            {
                return View();
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.TaskLogoutAsync();
            return View("Login");
        }
    }
}
