using System;
using System.ComponentModel.DataAnnotations;

namespace OutOfNews.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User name")]
        public string Username { get; set; }
 
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Born date")]
        public DateTime Born { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
 
        [Required]
        [Compare("Password", ErrorMessage = "Comparison with Password failed")]
        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        public string PasswordConfirm { get; set; }
    }
}