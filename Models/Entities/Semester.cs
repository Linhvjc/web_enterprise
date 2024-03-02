﻿using WebEnterprise.Models.Common;

namespace WebEnterprise.Models.Entities
{
    public class Semester : EntityBase
    {
        public string Name { set; get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Megazine> Megazines { get; set; }
    }
}

