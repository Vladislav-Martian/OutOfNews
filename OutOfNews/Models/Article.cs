using System;
using System.Collections.Generic;

namespace OutOfNews.Models
{
    public class Article
    {
        public bool Nsfw { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Location { get; set; } = null;
        
        public List<string> Tags { get; set; } = new List<string>();
        
        // Required on creating:
        
        public string Heading { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string LongDescription { get; set; }
        
    }
}