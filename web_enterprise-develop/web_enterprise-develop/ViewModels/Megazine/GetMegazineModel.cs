﻿namespace WebEnterprise.ViewModels.Megazine
{
    public class GetMegazineModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FacultyName { get; set; }
        public string ImagePath { set; get; }
        public DateTime CreatedDate { get; set; }
    }
}
