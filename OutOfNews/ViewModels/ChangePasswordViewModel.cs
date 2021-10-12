using System.ComponentModel.DataAnnotations;
using OutOfNews.Attributes;

namespace OutOfNews.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Unlike("CurrentPassword")]
        [Display(Name = "Your new password")]
        public string NewPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Comparison with Password failed")]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}