using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.ViewModels
{
    public class RegisterModel
    {
        [Required]
        public string FullName { set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string Password { set; get; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { set; get; }
        [Required]
        public string PhoneNumber { set; get; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { set; get; }
        [Required(ErrorMessage = "Faculty is required")]
        [Range(1, int.MaxValue, ErrorMessage = "FacultyId must be a valid number")]
        public int FacultyId { get; set; }
    }
}
