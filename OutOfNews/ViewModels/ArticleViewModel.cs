using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OutOfNews.Models;

namespace OutOfNews.ViewModels
{
    public class ArticleViewModel
    {
        public string AuthorId { get; set; }
        
        public string Location { get; set; } = null;
        
        public string Tags { get; set; } = null;
        
        [Required]
        public bool Nsfw { get; set; } = false;
        
        [Required]
        [Display(Name="Article heading")]
        [StringLength(64)]
        public string Heading { get; set; }
        
        [Required]
        [Display(Name="Article short description")]
        [StringLength(256)]
        public string ShortDescription { get; set; }
        
        [Required]
        [Display(Name="Article Long description")]
        public string LongDescription { get; set; }

        public Article Compile()
        {
            List<string> tags;
            try
            {
                tags = Tags.Trim()
                    .Replace(", ", ",")
                    .Replace(" ,", ",")
                    .Split(",")
                    .ToList();
            }
            catch (Exception e)
            {
                tags = new List<string>();
            }
            
            // tags => empty or filled list
            
            Article article = new Article()
            {
                AuthorId = AuthorId,
                Heading = Heading,
                ShortDescription = ShortDescription,
                LongDescription = LongDescription,
                Location = Location,
                Nsfw = Nsfw,
                Tags = tags,
                CreatedAt = DateTime.Now
            };
            return article;
        }
    }
}