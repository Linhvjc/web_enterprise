using AspNetCoreHero.ToastNotification.Abstractions;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebEnterprise.Models.Entities;

namespace WebEnterprise.Controllers
{
    public class FileController : Controller
    {
        private static string apiKey = "AIzaSyDxbWg5cX5zoEDrQCssfBGh5CZrRkAr8ro";
        private static string Bucket = "webenterprise-8a158.appspot.com";
        private static string AuthEmail = "betngaongo@gmail.com";
        private static string AuthPassword = "betngaongo";
        public INotyfService _notyfService { get; }

        public FileController(INotyfService notyfService)
        {
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFiles(string[] files)
        {

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var storage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            });
            var listFileUrls = new List<string>();
            foreach (var file in files)
            {
                try
                {
                    var downloadUrl = await storage.Child("assets").Child(file).GetDownloadUrlAsync();
                    listFileUrls.Add(downloadUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching URL for {file}: {ex.Message}");
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var client = new HttpClient();

                    foreach (var fileUrl in listFileUrls)
                    {
                        var response = await client.GetAsync(fileUrl);
                        var fileName = Path.GetFileName(fileUrl);
                        var zipEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                        using (var entryStream = zipEntry.Open())
                        using (var fileStream = await response.Content.ReadAsStreamAsync())
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }

                return File(memoryStream.ToArray(), "application/zip", "DownloadFiles.zip");
            }
        
        }
    }
}
