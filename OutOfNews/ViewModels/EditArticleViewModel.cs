using System.ComponentModel.DataAnnotations;

namespace OutOfNews.ViewModels
{
    public class EditArticleViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        
        public string Location { get; set; } = null;
        
        public string Tags { get; set; } = null;
        
        public bool Nsfw { get; set; } = false;

        [Display(Name = "Article heading")]
        [StringLength(64)]
        public string Heading { get; set; } = null;

        [Display(Name = "Article short description")]
        [StringLength(256)]
        public string ShortDescription { get; set; } = null;

        [Display(Name = "Article Long description")]
        public string LongDescription { get; set; } = null;
    }
}