using System.ComponentModel.DataAnnotations;

namespace OutOfNews.FormModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}