using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.ViewModels.Contribution;
using Microsoft.AspNetCore.Hosting;
using Firebase.Auth;
using Firebase.Storage;
using System.Diagnostics;
using AutoMapper;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.Models.Entities;
using AspNetCoreHero.ToastNotification.Abstractions;
using WebEnterprise.ViewModels;

namespace WebEnterprise.Controllers
{
    public class ContributionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notyfService { get; }
        private static string apiKey = "AIzaSyDxbWg5cX5zoEDrQCssfBGh5CZrRkAr8ro";
        private static string Bucket = "webenterprise-8a158.appspot.com";
        private static string AuthEmail = "betngaongo@gmail.com";
        private static string AuthPassword = "betngaongo";

        public ContributionController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostEnvironment,
            IUnitOfWork unitOfWork, IMapper mapper, INotyfService notyfService)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notyfService = notyfService;
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
                            .Child($"{uniqueFileName}.{Path.GetExtension(fileUpload.FileName).Substring(1)}")
                            .PutAsync(stream, cancellation.Token);
                        try
                        {
                            var status = await task;
                            _notyfService.Information(status);
                            var contributionReal = _mapper.Map<Contribution>(contribution);
                            contributionReal.FilePath = uniqueFileName;
                            contributionReal.Status = "Pending";
                            await _unitOfWork.ContributionRepository.Add(contributionReal);                          
                        }
                        catch (Exception ex)
                        {
                            _notyfService.Information("failed");
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
    }
}
