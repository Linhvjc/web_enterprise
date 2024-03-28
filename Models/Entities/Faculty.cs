using System.ComponentModel.DataAnnotations;
using WebEnterprise.Models.Common;

namespace WebEnterprise.Models.Entities
{
    public class Faculty : EntityBase
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Megazine> Megazines { get; set; }
    }
}
