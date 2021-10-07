using System.ComponentModel.DataAnnotations;

namespace OutOfNews.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Display(Name = "Do remember?")]
        public bool RememberMe { get; set; }
         
        public string ReturnUrl { get; set; }
    }
}