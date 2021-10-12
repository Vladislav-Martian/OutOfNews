using System.ComponentModel.DataAnnotations;

namespace OutOfNews.ViewModels
{
    public class ChangeNameViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Change nickname")]
        public string NickName { get; set; } = null;
    }
}