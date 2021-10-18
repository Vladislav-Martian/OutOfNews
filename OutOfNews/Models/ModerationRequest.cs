using System;
using System.ComponentModel.DataAnnotations;

namespace OutOfNews.Models
{
    
    public enum AdminRequestVariant
    {
        RoleChange,
        DeleteContent,
        Other
    }
    
    public class ModerationRequest
    {
        /// <summary>
        /// Id column in db
        /// </summary>
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public string UserId { get; set; }

        /// <summary>
        /// Short message
        /// </summary>
        [Required]
        public string Heading { get; set; }

        /// <summary>
        /// Long message, markdown friendly
        /// </summary>
        public string Message { get; set; } = null;

        public AdminRequestVariant RequestVariant { get; set; } = AdminRequestVariant.Other;
        
    }
}