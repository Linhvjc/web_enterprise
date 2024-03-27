using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Faculty
{
    public class CreateFacultyModel
    {
        [Required(ErrorMessage = "Name cannot be empty !!")]
        [StringLength(50)]
        public string Name { set; get; }

        [Required(ErrorMessage = "Description cannot be empty !!")]
        [StringLength(250)]
        public string Description { set; get; }
    }
}
