using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.ViewModels.Contribution;
using Firebase.Auth;
using Firebase.Storage;
using System.Diagnostics;
using AutoMapper;
using WebEnterprise.Repositories.Abstraction;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Text;
using WebEnterprise.Models.Entities;
using X.PagedList;
using AspNetCoreHero.ToastNotification.Notyf;

namespace WebEnterprise.Controllers
{
    public class ContributionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;
        public INotyfService _notyfService { get; }
        private static string apiKey = "AIzaSyDxbWg5cX5zoEDrQCssfBGh5CZrRkAr8ro";
        private static string Bucket = "webenterprise-8a158.appspot.com";
        private static string AuthEmail = "betngaongo@gmail.com";
        private static string AuthPassword = "betngaongo";


        public ContributionController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostEnvironment,
            IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService, IEmailSender emailSender)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;
            _emailSender = emailSender;
        }
        [HttpGet]
        public async Task<IActionResult> ContributionList(int id, string query, int? page)
        {
            ViewBag.Id = id;
            ViewBag.StoredQuery = query;
            _httpContextAccessor.HttpContext.Session.SetInt32("MegazineId", id);
            List<GetContributionModel> contributions = await _unitOfWork.ContributionRepository.SearchContribution(id, query);
            contributions = contributions.Where(c => c.Status == "Accept").ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var storage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            });
            var listFile = new List<string>();
            try
            {
                foreach (var con in contributions)
                {
                    var conReference = storage.Child("assets").Child(con.FilePath);
                    var downloadConTask = conReference.GetDownloadUrlAsync();
                    var conPath = await downloadConTask;
                    listFile.Add(conPath);
                }
                ViewBag.ContributionPaths = listFile;
            }
            catch (Exception ex)
            {
                ViewBag.ContributionPaths = null;
            }

            return View(contributions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Post(int megazineId)
        {
            var model = new CreateContribution
            {
                UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId"),
                MegazineId = megazineId
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateContribution contribution)
        {
            if (ModelState.IsValid)
            {
                var fileUpload = contribution.File;
                FileStream fileStream;
                if (fileUpload.Length > 0)
                {
                    string folderName = "documentFile";
                    string path = Path.Combine(_hostEnvironment.WebRootPath, $"files/{folderName}");
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileUpload.FileName)}";

                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                    var cancellation = new CancellationTokenSource();

                    using (var stream = fileUpload.OpenReadStream())
                    {
                        var task = new FirebaseStorage(
                            Bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                ThrowOnCancel = true
                            })
                            .Child("assets")
                            .Child($"{uniqueFileName}")
                            .PutAsync(stream, cancellation.Token);
                        try
                        {
                            //var status = await task;
                            _notyfService.Success("success");
                            var contributionReal = _mapper.Map<Contribution>(contribution);
                            contributionReal.FilePath = uniqueFileName;
                            contributionReal.Status = "Pending";
                            await _unitOfWork.ContributionRepository.Add(contributionReal);

                            var currentFacultyId = _httpContextAccessor.HttpContext.Session.GetInt32("FacultyId");
                            var receiver = await _unitOfWork.UserRepository.findCoorEmail((int)currentFacultyId);
                            var subject = "Send a notification";
                            var message = "There is a student contributing a new blog";
                            await _emailSender.SendEmailAsync(receiver, subject, message);
                        }
                        catch (Exception ex)
                        {
                            _notyfService.Information("failed:" + ex.Message);
                            Debug.WriteLine(ex);
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyfService.Information("bad model ");
                var model = new CreateContribution
                {
                    UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId"),
                    MegazineId = contribution.MegazineId
                };
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            var contribution = await _unitOfWork.ContributionRepository.GetContributionWithRelevant(id);
            contribution.FacultyUserName = await _unitOfWork.UserRepository.FindUserName(contribution.FacultyUserName);

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
        [HttpGet]
        public async Task<ActionResult<ListContributionStudent>> GetContributionStudent()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var listContributions = new ListContributionStudent();
            listContributions.GetContributionStudents = await _unitOfWork.ContributionRepository.GetAllContributionStudents(userId);

            return View(listContributions);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> UpdateContribution(UpdateContribution updateContribution)
        {
            if (ModelState.IsValid)
            {
                var fileUpload = updateContribution.File;
                FileStream fileStream;
                if (fileUpload.Length > 0)
                {
                    string folderName = "documentFile";
                    string path = Path.Combine(_hostEnvironment.WebRootPath, $"files/{folderName}");
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileUpload.FileName)}";

                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                    var cancellation = new CancellationTokenSource();

                    using (var stream = fileUpload.OpenReadStream())
                    {
                        var task = new FirebaseStorage(
                            Bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                ThrowOnCancel = true
                            })
                            .Child("assets")
                            .Child($"{uniqueFileName}")
                            .PutAsync(stream, cancellation.Token);
                        try
                        {
                            var contribution = await _unitOfWork.ContributionRepository.GetById(updateContribution.Id);
                            if (contribution == null)
                            {
                                _notyfService.Information("failed:" + updateContribution.Id);
                                return RedirectToAction("GetContributionStudent", "Contribution");
                            }
                            contribution.FilePath = uniqueFileName;
                            _mapper.Map(updateContribution, contribution);
                            await _unitOfWork.ContributionRepository.Update(contribution);
                            _notyfService.Success("success");
                        }
                        catch (Exception ex)
                        {
                            _notyfService.Information("failed:" + ex.Message);
                            Debug.WriteLine(ex);
                        }
                    }
                }

                return RedirectToAction("GetContributionStudent", "Contribution");
            }
            else
            {
                _notyfService.Error("Updating failed");

                return RedirectToAction("GetContributionStudent", "Contribution");
            }
        }
    }

}
