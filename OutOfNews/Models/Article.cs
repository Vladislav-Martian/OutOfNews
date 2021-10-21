using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OutOfNews.Models
{
    public class Article
    {
        public int Id { get; set; }
        
        public string AuthorId { get; set; }
        
        // Default
        
        public bool Nsfw { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Location { get; set; } = null;
        
        public List<string> Tags { get; set; } = new List<string>();
        
        // Required on creating:
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
        
    }
}