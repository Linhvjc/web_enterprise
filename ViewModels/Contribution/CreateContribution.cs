using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Contribution
{
    public class CreateContribution
    {
        [Required(ErrorMessage = "File cannot be empty !!")]
        public IFormFile File { get; set; }
        [Required(ErrorMessage = "Title cannot be empty !!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "MegazineId cannot be empty !!")]
        public int MegazineId { get; set; }
        [Required(ErrorMessage = "UserId cannot be empty !!")]
        public string UserId { get; set; }
    }
}
