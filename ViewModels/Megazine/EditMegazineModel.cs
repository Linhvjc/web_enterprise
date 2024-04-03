using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels.Megazine
{
    public class EditMegazineModel
    {
        [Required(ErrorMessage = "Name cannot be empty !!")]
        [StringLength(50)]
        public string Name { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Faculty cannot be empty !!")]
        public int FacultyId { get; set; }

        [Required(ErrorMessage = "Semester cannot be empty !!")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "Start date cannot be empty !!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date cannot be empty !!")]
        public DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }


}
