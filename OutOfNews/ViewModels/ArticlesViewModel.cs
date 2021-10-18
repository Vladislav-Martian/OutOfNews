using System;
using System.Collections.Generic;
using System.Linq;
using OutOfNews.Models;

namespace OutOfNews.ViewModels
{
    public sealed class PaginatedItemsViewModel<TItem>
    {
        private PaginationModel Pagination { get; set; }
        private IQueryable<TItem> Source { get; set; }
        private int Page { get; set; }
        private int PageSize { get; set; }
        private int TotalPages { get; set; }

        public PaginatedItemsViewModel(IQueryable<TItem> source, int pageSize = 12)
        {
            Source = source;
            Page = 1;
            TotalPages = (int)Math.Ceiling(source.Count() / (double)pageSize);
            Pagination = new PaginationModel(
                source.Count(),
                Page,
                pageSize);
        }

        public delegate IQueryable<TItem> AdditionalLinq(IQueryable<TItem> items);
        
        public List<TItem> GetItems(AdditionalLinq adds)
        {
            var tmp = Source.Skip((Page - 1) * PageSize).Take(PageSize);
            return adds.Invoke(tmp).ToList();
        }
        
        public bool HasPreviousPage => (Page > 1);
        public bool HasNextPage => (Page < TotalPages);

        public void NextPage()
        {
            if (HasNextPage) Page++;
        }
        
        public void PreviousPage()
        {
            if (HasPreviousPage) Page++;
        }

        public void GoToPage(int page = 1)
        {
            page--;
            Page = 1;
            for (int i = 0; i < page; i++)
            {
                NextPage();
            }
        }
        
    }
}