using System.ComponentModel.DataAnnotations;

namespace OutOfNews.ViewModels
{
    public class ChangeContactsViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Change main username")]
        public string Email { get; set; } = null;

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Change nickname")]
        public string PhoneNumber { get; set; } = null;
    }
}