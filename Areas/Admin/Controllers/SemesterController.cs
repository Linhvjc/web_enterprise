using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Admin")]
    public class SemesterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notyfService { get; }

        public SemesterController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _notyfService = notyfService;
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<Semester> semesterList = _unitOfWork.SemesterRepository.GetAll2().ToList();
            return View(semesterList);
        }


        [HttpGet]

    public class SemesterController : Controller
    {
        private readonly UniversityDbContext _context;

        public SemesterController(UniversityDbContext context)
        {
            _context = context;
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

        // GET: Admin/Semesters/Create

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Semester semester)
        {

            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully create a new Semester");
                await _unitOfWork.SemesterRepository.Add(semester);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Semester? semesterFromDb = _unitOfWork.SemesterRepository.Get(u => u.Id == id);


            if (semesterFromDb == null)
            {
                return NotFound();
            }
            return View(semesterFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Semester semester)
        {
            if (ModelState.IsValid)
            {
                _notyfService.Success("You successfully update a Semester");
                await _unitOfWork.SemesterRepository.Update(semester);
                return RedirectToAction("Index");
            }
            return View();
        }

        // Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)

        // POST: Admin/Semesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Id,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                _context.Add(semester);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(semester);
        }

        // GET: Admin/Semesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }

        // POST: Admin/Semesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,StartDate,EndDate,Id,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate")] Semester semester)
        {
            if (id != semester.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semester);
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

        // GET: Admin/Semesters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Semesters == null)

            {
                return NotFound();
            }

            Semester? semesterFromDb = _unitOfWork.SemesterRepository.Get(u => u.Id == id);

            if (semesterFromDb == null)
            {
                return NotFound();
            }
            return View(semesterFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Semester? semesterFromDb = _unitOfWork.SemesterRepository.Get(u => u.Id == id);
            if (semesterFromDb == null)
            {
                return NotFound();
            }
            _notyfService.Success("You successfully delete a Semester");
            await _unitOfWork.SemesterRepository.Delete(semesterFromDb);
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Semester> semesterList = _unitOfWork.SemesterRepository.GetAll2().ToList();
            return Json(new { data = semesterList });
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    var semesterToBeDeleted = _unitOfWork.SemesterRepository.Get(u => u.Id == id);
        //    if (semesterToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    await _unitOfWork.SemesterRepository.Delete(semesterToBeDeleted);
        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        #endregion




            var semester = await _context.Semesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        // POST: Admin/Semesters/Delete/5
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

        private bool SemesterExists(int id)
        {
          return (_context.Semesters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
