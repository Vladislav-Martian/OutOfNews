using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OutOfNews.Contexts;
using OutOfNews.Extensions;
using OutOfNews.Models;
using OutOfNews.ViewModels;

namespace OutOfNews.Controllers
{
    public class AuthorController : Controller
    {
        #region Injects

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public AuthorController(UserManager<User> userManager, IConfiguration configuration, AppDbContext db)
        {
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
        }

        #endregion
        
        
        // GET: Author
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Details));
        }

        // GET: Author/Details/5
        [Authorize(Roles="author, moder, admin")]
        public ActionResult Details(int id = 1)
        {
            var user = User.GetLoggedInUser(_userManager);
            var model = new AuthorDetailsViewModel()
            {
                UserId = user.Id,
                NickName = user.GetNameToShow(),
                Adult = user.IsAdult(
                    int.Parse(_configuration["Restrictions:NSFWAge"]),
                    bool.Parse(_configuration["Restrictions:UnauthorizedAdult"])),
                PaginatedArticles = new PaginatedItemsViewModel<Article>(
                    _db.Articles.AsQueryable().Where(x => x.AuthorId == user.Id), 6)
                {
                    Page = id,
                    StaticFilters = a =>
                        a.OrderByDescending(x => x.CreatedAt)
                            .ThenByDescending(x => x.Id)
                }
            };
            return View(model);
        }

        // GET: Author/Write
        public ActionResult Write()
        {
            var model = new ArticleViewModel()
            {
                AuthorId = User.GetLoggedInUserId<string>(),
                Heading = null,
                Nsfw = false,
                ShortDescription = null,
                LongDescription = null
            };
            return View(model);
        }

        // POST: Author/Write
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Write(ArticleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var article = model.Compile();
                    var res1 = await _db.Articles.AddAsync(article);
                    var res2 = await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Details));
                }
                return View(model);
            }
            catch (Exception e)
            {
                throw;
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/Delete/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}