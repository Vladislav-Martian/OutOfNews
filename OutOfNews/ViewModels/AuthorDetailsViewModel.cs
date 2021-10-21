using System.Collections.Generic;
using OutOfNews.Models;

namespace OutOfNews.ViewModels
{
    public class AuthorDetailsViewModel
    {
        public string UserId { get; set; }
        public string NickName { get; set; }
        public bool Adult { get; set; }
        
        public PaginatedItemsViewModel<Article> PaginatedArticles { get; set; }

        public List<Article> GetItems(PaginatedItemsViewModel<Article>.AdditionalLinq adds = null)
        {
            return PaginatedArticles.GetItems(adds);
        }
    }
}