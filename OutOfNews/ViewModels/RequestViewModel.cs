using System;
using System.ComponentModel.DataAnnotations;
using OutOfNews.Models;

namespace OutOfNews.ViewModels
{
    public class RequestViewModel
    {
        /// <summary>
        /// Short message
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string Heading { get; set; }

        /// <summary>
        /// Long message, markdown friendly
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = null;

        public AdminRequestVariant RequestVariant { get; set; } = AdminRequestVariant.Other;
    }
}