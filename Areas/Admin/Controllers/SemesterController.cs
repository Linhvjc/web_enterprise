using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Semester;

namespace WebEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class SemesterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public INotyfService _notyfService { get; }
        private readonly UniversityDbContext _context;

        public SemesterController(IUnitOfWork unitOfWork, INotyfService notyfService, UniversityDbContext context, IMapper mapper)
        {
            _notyfService = notyfService;
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }


        // GET: Admin/Semesters
        public async Task<IActionResult> Index()
        {
            return _context.Semesters != null ?
                        View(await _context.Semesters.ToListAsync()) :
                        Problem("Entity set 'UniversityDbContext.Semesters'  is null.");
        }

        // GET: Admin/Semesters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate")] CreateSemester semester)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var semes = _mapper.Map<Semester>(semester);
                    await _context.AddAsync(semes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _notyfService.Error(ex.Message);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _notyfService.Error(error.ErrorMessage);
                }

            }
            return View(semester);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters.FindAsync(id);
            UpdateSemester updateSemester = _mapper.Map<UpdateSemester>(semester);
            if (semester == null)
            {
                return NotFound();
            }
            return View(updateSemester);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,StartDate,EndDate")] UpdateSemester semester)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var sem = await _unitOfWork.SemesterRepository.GetById(semester.Id);
                    _mapper.Map(semester, sem);
                    await _unitOfWork.SemesterRepository.Update(sem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterExists(semester.Id))
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
            return View(semester);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Semesters == null)
            {
                return Problem("Entity set 'UniversityDbContext.Semesters'  is null.");
            }
            var semester = await _context.Semesters.FindAsync(id);
            if (semester != null)
            {
                _context.Semesters.Remove(semester);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Semester> semesterList = _unitOfWork.SemesterRepository.GetAll2().ToList();
            return Json(new { data = semesterList });
        }
        private bool SemesterExists(int id)
        {
            return (_context.Semesters?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}