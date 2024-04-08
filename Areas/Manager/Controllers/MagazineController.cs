using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Faculty;
using WebEnterprise.ViewModels.Megazine;
using WebEnterprise.ViewModels.Semester;


namespace WebEnterprise.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class MagazineController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public INotyfService _notyfService { get; }

        public MagazineController(UniversityDbContext context, IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Coordinator/Megazine

        public async Task<IActionResult> Index()
        {
            var AllMegazines = _unitOfWork.MegazineRepository.GetAll2(includeProperties: "Faculty,Semester");
            var ViewMegazines = AllMegazines.Where(e => !e.IsDeleted);
            return View(ViewMegazines.ToList());
        }

        // GET: Magazine/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var faculties = await _unitOfWork.FacultyRepository.GetAll();
            var semesters = await _unitOfWork.SemesterRepository.GetAll();
            // Assuming you have a SemesterRepository similar to FacultyRepository
            var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(faculties);
            var viewSemesters = _mapper.Map<List<GetSemesterAdmin>>(semesters);
            ViewBag.Faculties = new SelectList(viewFacultyList, "FacultyId", "Name");
            ViewBag.Semesters = new SelectList(viewSemesters, "SemesterId", "Name");

            return View();
        }



        // POST: Magazine/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateMegazineModel megazineModel)
        {
            if (!ModelState.IsValid)
            {

                var faculties = await _unitOfWork.FacultyRepository.GetAll();
                var semesters = await _unitOfWork.SemesterRepository.GetAll();

                var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(faculties);
                var viewSemesters = _mapper.Map<List<GetSemesterAdmin>>(semesters);
                ViewBag.Faculties = new SelectList(viewFacultyList, "FacultyId", "Name");
                ViewBag.Semesters = new SelectList(viewSemesters, "SemesterId", "Name");

                return View();
            }
            else
            {
                try
                {

                    var megazine = _mapper.Map<Megazine>(megazineModel);

                    megazine.Description = "abc";
                    await _unitOfWork.MegazineRepository.Add(megazine);

                }
                catch (Exception ex)
                {
                    _notyfService.Error($"Create failed {ex.Message}");
                }

                return RedirectToAction("Index");
            }


        }


        // GET: Magazine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var megazine = await _unitOfWork.MegazineRepository.GetById(id.Value);
            if (megazine == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<EditMegazineModel>(megazine); // Assuming your CreateMegazineModel can be used for edits too.
            var faculties = await _unitOfWork.FacultyRepository.GetAll();
            var semesters = await _unitOfWork.SemesterRepository.GetAll();
            var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(faculties);
            var viewSemesters = _mapper.Map<List<GetSemesterAdmin>>(semesters);
            ViewBag.Faculties = new SelectList(viewFacultyList, "FacultyId", "Name");
            ViewBag.Semesters = new SelectList(viewSemesters, "SemesterId", "Name");

            return View(model);
        }




        // POST: Magazine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMegazineModel megazineModel)
        {
            if (id != megazineModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var megazine = _mapper.Map<Megazine>(megazineModel);
                    await _unitOfWork.MegazineRepository.Update(megazine);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MegazineExists(megazineModel.Id))
                    {
                        return NotFound();
                    }

                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(megazineModel);
        }

        private async Task<bool> MegazineExists(int id)
        {
            var megazine = await _unitOfWork.MegazineRepository.GetById(id);
            return megazine != null;
        }


        // GET: Magazine/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var megazine = await _unitOfWork.MegazineRepository.GetById(id.Value);
            if (megazine == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<EditMegazineModel>(megazine); // Assuming your CreateMegazineModel can be used for edits too.
            var faculties = await _unitOfWork.FacultyRepository.GetAll();
            var semesters = await _unitOfWork.SemesterRepository.GetAll();
            var viewFacultyList = _mapper.Map<List<GetFacultyModel>>(faculties);
            var viewSemesters = _mapper.Map<List<GetSemesterAdmin>>(semesters);
            ViewBag.Faculties = new SelectList(viewFacultyList, "FacultyId", "Name");
            ViewBag.Semesters = new SelectList(viewSemesters, "SemesterId", "Name");

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, EditMegazineModel megazineModel)
        {
            if (id != megazineModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var megazine = _mapper.Map<Megazine>(megazineModel);

                    await _unitOfWork.MegazineRepository.Update(megazine);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MegazineExists(megazineModel.Id))
                    {
                        return NotFound();
                    }

                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(megazineModel);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var AllMegazines = _unitOfWork.MegazineRepository.GetAll2(includeProperties: "Faculty,Semester");
            var ViewMegazines = AllMegazines.Where(e => !e.IsDeleted)
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.Description,
                    m.StartDate,
                    m.EndDate,
                    m.ImagePath,
                    m.FacultyId,
                    Faculty = new
                    {
                        m.Faculty.Name,
                    },
                    Semester = new
                    {
                        m.Semester.Name,
                    },
                }).ToList();
            return Json(new { data = ViewMegazines });
        }
        #endregion
    }
}