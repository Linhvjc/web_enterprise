using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Contribution;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class DashboardController : Controller
    {
        public INotyfService _notyfService { get; }

        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var barChart = new List<FacultyDetail>
            {
                new FacultyDetail() {Id = 1, Name = "Spring 2023", Computing = 65, Business =23, Designing =77, Engineering = 95},
                new FacultyDetail() {Id = 2, Name = "Summer 2023", Computing = 82, Business =53, Designing =43, Engineering = 55},
                new FacultyDetail() {Id = 3, Name = "Fall 2023", Computing = 75, Business =43, Designing =48, Engineering = 62},
                new FacultyDetail() {Id = 4, Name = "Winter 2023", Computing = 36, Business =44, Designing =22, Engineering = 34},
                new FacultyDetail() {Id = 5, Name = "Spring 2024", Computing = 65, Business =23, Designing =67, Engineering = 56}
            };

            var pieChart1 = new List<FacultyPieChart>
            {
                new FacultyPieChart() {Name = "Computing", Number=65},
                new FacultyPieChart() {Name = "Business", Number=23},
                new FacultyPieChart() {Name = "Designing", Number=67},
                new FacultyPieChart() {Name = "Engineering", Number=56}
            };
            var pieChart2 = new List<FacultyPieChart>
            {
                new FacultyPieChart() {Name = "Computing", Number=36},
                new FacultyPieChart() {Name = "Business", Number=44},
                new FacultyPieChart() {Name = "Designing", Number=22},
                new FacultyPieChart() {Name = "Engineering", Number=34}
            };

            var pieChart3 = new List<FacultyPieChart>
            {
                new FacultyPieChart() {Name = "Computing", Number=75},
                new FacultyPieChart() {Name = "Business", Number=43},
                new FacultyPieChart() {Name = "Designing", Number=48},
                new FacultyPieChart() {Name = "Engineering", Number=62}
            };

            ViewBag.BarChart = barChart;
            ViewBag.PieChart = pieChart1;
            ViewBag.PieChart2 = pieChart2;
            ViewBag.PieChart3 = pieChart3;

            var data = await _unitOfWork.FacultyRepository.GetStatisticsFaculties();
            var exContribution = await _unitOfWork.ContributionRepository.GetContributionsWithout();
            var exContribution14 = await _unitOfWork.ContributionRepository.GetContributionsWithout14();
            ViewBag.ExCon = exContribution;
            ViewBag.ExCon14 = exContribution14;
            return View(data);
        }

        [HttpGet]
        public async Task<ActionResult> ContributionList()
        {
            var semesters = await _unitOfWork.SemesterRepository.GetAll();
            ViewBag.SemesterList = new SelectList(semesters, "Name", "Name");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ContributionData(string? semester)
        {
            List<GetContributionModel> contributions = await _unitOfWork.ContributionRepository.SearchContribution(semester);

            return Json(contributions);
        }
    }
}
