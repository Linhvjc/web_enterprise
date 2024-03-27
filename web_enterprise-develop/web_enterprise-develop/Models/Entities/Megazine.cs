using System.ComponentModel.DataAnnotations.Schema;
using WebEnterprise.Models.Common;

namespace WebEnterprise.Models.Entities
{
    public class Megazine : EntityBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImagePath { set; get; }
        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }
        public int  SemesterId { get; set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; set; }
        public List<Contribution> Contributions { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
