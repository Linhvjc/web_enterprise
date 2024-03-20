using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebEnterprise.Models.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }
        public List<Contribution>? Contributions { get; set; }
        [NotMapped] // This property will not be added to the database
        public string Role { get; set; }
    }
}
