using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace WebEnterprise.Controllers
{
    public class FileController : Controller
    {

        [HttpGet]
        public async Task<ActionResult> DownloadFiles(string[] files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var client = new HttpClient();

                    foreach (var fileUrl in files)
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
