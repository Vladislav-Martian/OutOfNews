﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OutOfNews.Contexts;
using OutOfNews.Extensions;
using OutOfNews.Models;
using OutOfNews.ViewModels;

namespace OutOfNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppDbContext _db;
        private UserManager<User> _userManager;
        private IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, AppDbContext db, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            Configuration = configuration;
        }

        public IActionResult Index(int id = 1)
        {
            PaginatedItemsViewModel<Article> articles;
            if (User.Identity != null 
                && User.Identity.IsAuthenticated
                && User.GetLoggedInUser(_userManager).IsAdult(
                    int.Parse(Configuration["Restrictions:NSFWAge"]),
                    bool.Parse(Configuration["Restrictions:UnauthorizedAdult"])))
            {
                // adult
                articles = new PaginatedItemsViewModel<Article>(
                    _db.Articles
                        .AsQueryable()
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.Id))
                {
                    Page = id // view page
                };
            }
            else
            {
                // not adult
                articles = new PaginatedItemsViewModel<Article>(
                    _db.Articles
                        .AsQueryable()
                        .Where(a => !a.Nsfw)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.Id))
                {
                    Page = id // view page
                };
            }
            
            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}