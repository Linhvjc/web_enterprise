using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    public class HomeController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HomeController(IUserAuthenticationService service, UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
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
    }
}
