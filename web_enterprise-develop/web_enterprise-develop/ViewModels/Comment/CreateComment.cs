

using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Comment
{
    public class CreateComment
    {
        [Required(ErrorMessage = "Comment cannot be empty !!")]
        public string CommentText { get; set; }
        public int ContributionId { get; set; }
        public string StudentId { get; set; }
        public int FacultyId { get; set; }
    }
}
