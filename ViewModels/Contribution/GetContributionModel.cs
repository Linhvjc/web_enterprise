namespace WebEnterprise.ViewModels.Contribution
{
    public class GetContributionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public int ReplyCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Megazine { get; set; }
        public string FilePath { get; set; }
        public String Status { get; set; } = "Accept";
    }
}
