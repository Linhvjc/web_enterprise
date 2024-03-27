using System.ComponentModel.DataAnnotations.Schema;
using WebEnterprise.Models.Common;
using WebEnterprise.Models.Entities;

namespace WebEnterprise.Models.Entities
{
    public class Contribution : EntityBase
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } = "Pending";
        public int MegazineId { get; set; }
        [ForeignKey("MegazineId")]
        public Megazine Megazine { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Image> Images { get; set; }
    }
}