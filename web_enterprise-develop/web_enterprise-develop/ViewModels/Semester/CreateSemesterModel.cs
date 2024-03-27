using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Semester
{
    public class CreateSemesterModel
    {
        [Required(ErrorMessage = "Name cannot be empty !!")]
        [StringLength(50)]
        public string? Name { set; get; }
        [Required(ErrorMessage = "StartDate cannot be empty !!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate cannot be empty !!")]
        public DateTime EndDate { get; set; }




    }
}
