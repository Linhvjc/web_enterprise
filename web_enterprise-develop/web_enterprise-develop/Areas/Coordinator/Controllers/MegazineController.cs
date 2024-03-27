
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Contribution;
using WebEnterprise.ViewModels.Faculty;
using WebEnterprise.ViewModels.Megazine;
using WebEnterprise.ViewModels.Semester;


namespace WebEnterprise.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    public class MegazineController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public INotyfService _notyfService { get; }

        public MegazineController(UniversityDbContext context, IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
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
            var megazines = _context.Megazines.Where(e => !e.IsDeleted ).Include(m => m.Faculty);
            return View(await megazines.ToListAsync());
        }

        // GET: Magazine/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var faculties = await _unitOfWork.FacultyRepository.GetAll();
            var semesters = await _unitOfWork.SemesterRepository.GetAll();
            // Assuming you have a SemesterRepository similar to FacultyRepository
            var fal = _mapper.Map<List<GetFacultyModel>>(faculties);
            var ses = _mapper.Map<List<GetSemesterAdmin>>(semesters);
            ViewBag.Faculties = new SelectList(fal, "FacultyId", "Name");
            ViewBag.Semesters = new SelectList(ses, "SemesterId", "Name");

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

                var fal = _mapper.Map<List<GetFacultyModel>>(faculties);
                var ses = _mapper.Map<List<GetSemesterAdmin>>(semesters);
                ViewBag.Faculties = new SelectList(fal, "FacultyId", "Name");
                ViewBag.Semesters = new SelectList(ses, "SemesterId", "Name");

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
            var fal = _mapper.Map<List<GetFacultyModel>>(faculties);
            var ses = _mapper.Map<List<GetSemesterAdmin>>(semesters);
            ViewBag.Faculties = new SelectList(fal, "FacultyId", "Name");
            ViewBag.Semesters = new SelectList(ses, "SemesterId", "Name");

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
            var fal = _mapper.Map<List<GetFacultyModel>>(faculties);
            var ses = _mapper.Map<List<GetSemesterAdmin>>(semesters);
            ViewBag.Faculties = new SelectList(fal, "FacultyId", "Name");
            ViewBag.Semesters = new SelectList(ses, "SemesterId", "Name");

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









        [HttpGet]
        public async Task<IActionResult> Contributions(int id)
        {

            int? facultyId = _httpContextAccessor.HttpContext.Session.GetInt32("FacultyId");

            // Đặt FacultyId vào ViewBag để truyền nó vào view
            ViewBag.FacultyId = facultyId;

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var megazine = await _context.Megazines
                                         .Include(m => m.Contributions)
                                             .ThenInclude(c => c.User)
                                             .ThenInclude(u => u.Faculty)
                                         .FirstOrDefaultAsync(m => m.Id == id);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            if (megazine == null)
            {
                return NotFound();
            }

            return View(megazine.Contributions);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleContributionStatus(int id)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            // Đổi trạng thái giữa "Append" và "Accept"
            contribution.Status = contribution.Status == "Append" ? "Accept" : "Append";
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Contributions), new { id = contribution.MegazineId }); // Quay trở lại danh sách Contributions với id của Megazine
        }





    }
}
