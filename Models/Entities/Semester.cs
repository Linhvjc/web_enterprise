using WebEnterprise.Models.Common;

namespace WebEnterprise.Models.Entities
{
    public class Semester : EntityBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Megazine> Megazines { get; set; }
    }
}

