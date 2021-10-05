using System;
using System.ComponentModel.DataAnnotations;

namespace OutOfNews.FormModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Password confirmation required!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirmation failed")]
        public string ConfirmPassword { get; set; }

        #region For user defaults and config

        [DataType(DataType.Text)] public string Name { get; set; } = null;
        
        [DataType(DataType.Text)] public string Nickname { get; set; } = null;

        [DataType(DataType.Date)] public DateTime? BornDate { get; set; } = null;
        
        #endregion
    }
}