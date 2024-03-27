using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net.Sockets;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Contribution;
using WebEnterprise.ViewModels.Imgae;

namespace WebEnterprise.Controllers
{
    public class ImageController : Controller
    {
        private static string apiKey = "AIzaSyDxbWg5cX5zoEDrQCssfBGh5CZrRkAr8ro";
        private static string Bucket = "webenterprise-8a158.appspot.com";
        private static string AuthEmail = "betngaongo@gmail.com";
        private static string AuthPassword = "betngaongo";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notyfService { get; }

        public ImageController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostEnvironment, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _notyfService = notyfService;
        }

        [HttpPost]
        public async Task<ActionResult> UploadImage(CreateImage createImage)
        {
            if (ModelState.IsValid)
            {

                if (createImage.EndSemesterDate < DateTime.Now)
                {
                    _notyfService.Information("You can't upload images due to the expiration of the semester ");

                    return RedirectToAction("Detail", "Contribution", new { id = createImage.ContributionId });
                }
                else
                {

                    var uploadTasks = new List<Task>();
                    var errors = new List<string>();
                    var successfulUploads = new List<string>();

                    foreach (var fileUpload in createImage.FilePaths)
                    {
                        if (fileUpload.Length > 0)
                        {
                            uploadTasks.Add(UploadFileToFirebaseAsync(createImage.ContributionId, fileUpload, successfulUploads, errors));
                        }
                    }

                    await Task.WhenAll(uploadTasks);

                    if (errors.Any())
                    {
                        foreach (var error in errors)
                        {
                            _notyfService.Error($"Failed to upload: {error}");
                        }
                    }
                    foreach (var fileName in successfulUploads)
                    {
                        _notyfService.Success($"Successfully uploaded: {fileName}");
                    }

                    return RedirectToAction("Detail", "Contribution", new { id = createImage.ContributionId });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _notyfService.Error($"Validation error: {error.ErrorMessage}");
                }

                return RedirectToAction("Detail", "Contribution", new { id = createImage.ContributionId });
            }
        }

        private async Task UploadFileToFirebaseAsync(int contributionId, IFormFile fileUpload, List<string> successfulUploads, List<string> errors)
        {
            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileUpload.FileName)}";
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var authResponse = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                var cancellationToken = new CancellationTokenSource();

                using (var stream = fileUpload.OpenReadStream())
                {
                    var uploadTask = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(authResponse.FirebaseToken),
                            ThrowOnCancel = true // This will throw an exception on cancellation/token expiration
                        })
                        .Child("assets")
                        .Child(uniqueFileName)
                        .PutAsync(stream, cancellationToken.Token);

                    // Wait for the upload task to complete
                    await uploadTask;

                    // Assuming Image and CreateImage are your data models and _mapper maps one to the other
                    var image = new Image();
                    image.ContributionId = contributionId;
                    image.FilePath = uniqueFileName;
                    await _unitOfWork.ImageRepository.Add(image);

                    successfulUploads.Add(uniqueFileName);
                }
            }
            catch (Exception ex)
            {
                errors.Add(uniqueFileName);
                Debug.WriteLine($"Error uploading {uniqueFileName}: {ex.Message}");
            }
        }
    }
}
