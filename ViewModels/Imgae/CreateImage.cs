using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Imgae
{
    public class CreateImage
    {
        [Required(ErrorMessage = "File cannot be empty !!")]
        public List<IFormFile> FilePaths { get; set; }
        [Required(ErrorMessage = "ContributionId cannot be empty !!")]
        public int ContributionId { get; set; }
        public DateTime EndSemesterDate { get; set; }
    }
}
