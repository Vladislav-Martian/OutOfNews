﻿using System;

namespace OutOfNews.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
 
        public PaginationModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
 
        public bool HasPreviousPage => (PageNumber > 1);
        public bool HasNextPage => (PageNumber < TotalPages);
    }
}