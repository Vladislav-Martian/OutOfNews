using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OutOfNews.Contexts;
using OutOfNews.Models;
using OutOfNews.ViewModels;

namespace OutOfNews.Controllers
{
    public class FilteredController: Controller
    {
        private readonly AppDbContext _db;

        public FilteredController(AppDbContext db)
        {
            _db = db;
        }


        // Get: 
        public IActionResult Tagged(string tag, int id = 1)
        {
            var source = _db.Articles.AsQueryable();
            // prepare model
            var model = new PaginatedItemsViewModel<Article>(source, 12)
            {
                Page = id,
                StaticFilters = (s) =>
                {
                    return s.Where(a => a.Tags.Contains(tag));
                }
            };

            return View("SearchResult", model);
        }
    }
}