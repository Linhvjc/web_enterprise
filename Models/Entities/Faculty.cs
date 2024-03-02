using WebEnterprise.Models.Common;

namespace WebEnterprise.Models.Entities
{
    public class Faculty : EntityBase
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public List<Megazine> Megazines { get; set; }
    }
}
