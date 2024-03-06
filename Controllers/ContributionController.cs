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
        public async Task<IActionResult> ContributionList(int id)
        {
            ViewBag.Id = id;
            var contributions = await _unitOfWork.ContributionRepository.GetAllContributions(id);
            var ContributionList = new ContributionList
            {
                Contributions = contributions
            };
            var currentFacultyId = _httpContextAccessor.HttpContext.Session.GetInt32("FacultyId");
            var receiver = await _unitOfWork.UserRepository.findCoorEmail((int)currentFacultyId);
            _notyfService.Information(receiver);
            return View(ContributionList);
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
                                ThrowOnCancel = true // when you cancel the upload, an exception is thrown. By default, no exception is thrown
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
            DetailContribution detailContribution = _mapper.Map<DetailContribution>(contribution);

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var storage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken), 
            });
            var reference = storage.Child("assets").Child(detailContribution.FilePath);
            try
            {
                // Get a download URL for the file
                var downloadUrlTask = reference.GetDownloadUrlAsync();
                var downloadUrl = await downloadUrlTask;

                if (downloadUrl != null)
                {
                    ViewBag.DocumentUrl = downloadUrl;
                }            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving download URL: " + ex.Message);
            }
            //ViewBag.DocumentUrl = filePath;

            return View(detailContribution);
        }
       

        //[HttpPost]
        //public IActionResult OnPost(string FileName)
        //{
           
        //    string projectRootPath = _hostEnvironment.ContentRootPath;
        //    string outputPath = Path.Combine(projectRootPath, "wwwroot", "files");
        //    string storagePath = Path.Combine(projectRootPath, "storage");
        //    int pageCount = 0;
        //    string imageFilesFolder = Path.Combine(outputPath, Path.GetFileName(FileName).Replace(".", "_"));
        //    if (!Directory.Exists(imageFilesFolder))
        //    {
        //        Directory.CreateDirectory(imageFilesFolder);
        //    }
        //    string imageFilesPath = Path.Combine(imageFilesFolder, "page-{0}.png");
        //    using (Viewer viewer = new Viewer( Path.Combine(storagePath, FileName)))
        //    {
        //        ViewInfo info = viewer.GetViewInfo(ViewInfoOptions.ForPngView(false));
        //        pageCount = info.Pages.Count;
        //        PngViewOptions options = new PngViewOptions(imageFilesPath);
        //        viewer.View(options);
        //    }
        //    return new JsonResult(pageCount);
        //}
    }

}
