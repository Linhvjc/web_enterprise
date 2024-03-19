using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Contribution
{
    public class UpdateContribution
    {
            [Required(ErrorMessage = "File cannot be empty !!")]
            public IFormFile File { get; set; }
            [Required(ErrorMessage = "Title cannot be empty !!")]
            public string Title { get; set; }
            [Required(ErrorMessage = "Id cannot be empty !!")]
            public int Id { get; set; }
    }
}
