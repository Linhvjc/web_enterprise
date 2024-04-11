using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using System.Net.Sockets;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.Repositories.Implement;
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

        private static string apiKey = "AIzaSyDxbWg5cX5zoEDrQCssfBGh5CZrRkAr8ro";
        private static string Bucket = "webenterprise-8a158.appspot.com";
        private static string AuthEmail = "betngaongo@gmail.com";
        private static string AuthPassword = "betngaongo";

        public MegazineController(UniversityDbContext context, IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;      
        }

        // GET: Coordinator/Megazine

        public async Task<IActionResult> Index()
        {
            // Lấy FacultyId của người dùng từ Session
            int? currentUserFacultyId = _httpContextAccessor.HttpContext.Session.GetInt32("FacultyId");

            if (currentUserFacultyId.HasValue)
            {
                // Lọc danh sách magazines dựa trên FacultyId của người dùng hiện tại
                var megazines = _context.Megazines
                                        .Where(e => !e.IsDeleted && e.FacultyId == currentUserFacultyId)
                                        .Include(m => m.Faculty);

                return View(await megazines.ToListAsync());
            }
            else
            {
                // Trường hợp không tìm thấy FacultyId trong Session, xử lý theo ý của bạn
                // Ví dụ: Redirect hoặc hiển thị thông báo lỗi
                return Redirect("/authentication/login");
            }
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

        [HttpGet]
        public async Task<IActionResult> ContributionDetail(int id)
        {
            var contribution = await _unitOfWork.ContributionRepository.GetContributionWithRelevant(id);
            contribution.FacultyUserName = await _unitOfWork.UserRepository.FindUserName(contribution.FacultyUserName);
            if (contribution == null)
            {
                return NotFound();
            }

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var storage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            });
            var reference = storage.Child("assets").Child(contribution.FilePath);
            var listImagePath = new List<string>();

            try
            {
                // Get a download URL for the file
                var downloadUrlTask = reference.GetDownloadUrlAsync();
                var downloadUrl = await downloadUrlTask;

                if (downloadUrl != null)
                {
                    ViewBag.DocumentUrl = downloadUrl;
                }

                foreach (var image in contribution.imagePaths)
                {
                    var imageReference = storage.Child("assets").Child(image);
                    var downloadImageTask = imageReference.GetDownloadUrlAsync();
                    var imagePath = await downloadImageTask;
                    listImagePath.Add(imagePath);
                }
                ViewBag.ImagePaths = listImagePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving download URL: " + ex.Message);
            }

            return View(contribution);
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
            contribution.Status = contribution.Status == "Pending" ? "Accept" : "Pending";
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Contributions), new { id = contribution.MegazineId }); // Quay trở lại danh sách Contributions với id của Megazine
        }
    }
}