using System.ComponentModel.DataAnnotations.Schema;
using WebEnterprise.Models.Common;

namespace WebEnterprise.Models.Entities
{
    public class Comment : EntityBase
    {
        public string CommentText { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int ContributionId { get; set; }
        [ForeignKey("ContributionId")]
        public Contribution Contribution { get; set; }
    }
}
