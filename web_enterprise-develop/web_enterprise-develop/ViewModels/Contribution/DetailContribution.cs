using WebEnterprise.ViewModels.Comment;
using WebEnterprise.ViewModels.Imgae;

namespace WebEnterprise.ViewModels.Contribution
{
    public class DetailContribution
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string FullName { get; set; }
        public string MegazineName { get; set; }
        public string FacultyName { get; set; }
        public int FacultyId { get; set; }
        public string ProfilePicture { get; set; }
        public int numberContribution { get; set; }
        public string UserId { get; set; }
        public DateTime EndSemesterDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> imagePaths { get; set; }

        public CreateImage CreateImage { get; set; }
        public CreateComment CreateComment { get; set; }
        public string Comment { get; set; }
        public string FacultyUserName { get; set; }
        public DateTime CreatedCommentDate { get; set; }
    }
}
