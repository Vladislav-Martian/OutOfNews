using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult> Index()
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
                    await _db.Articles.AddAsync(article);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Details));
                }
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            User user = User.GetLoggedInUser(_userManager);
            var article = _db.Articles.AsQueryable()
                .Where(a => a.AuthorId == user.Id)
                .FirstOrDefault(a => a.Id == id);
            
            if (article == null)
            {
                ModelState.AddModelError("", "Article don`t exists, or does not belong to you");
                return View(null);
            }
            
            var model = new EditArticleViewModel()
            {
                ArticleId = article.Id,
                Location = article.Location,
                Tags = string.Join(", ", article.Tags),
                Nsfw = article.Nsfw,
                Heading = article.Heading,
                ShortDescription = article.ShortDescription,
                LongDescription = article.LongDescription
            };
            return View(model);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditArticleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Article article = await _db.Articles.Where(a => a.Id == model.ArticleId).FirstAsync();
                    article.Heading = model.Heading;
                    article.ShortDescription = model.ShortDescription;
                    article.LongDescription = model.LongDescription;
                    article.CreatedAt = DateTime.Now;
                    article.Nsfw = model.Nsfw;
                    article.Location = model.Location;
                    
                    _db.Update(article);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Details));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        // GET: Author/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var article = await _db.Articles.Where(a => a.Id == id)
                .FirstOrDefaultAsync(a => a.AuthorId == User.GetLoggedInUserId<string>());
            
            return View("DeleteArticle", article);
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteArticle(int id)
        {
            try
            {
                _db.Articles.Remove(
                    await _db.Articles.Where(a => a.Id == id).FirstOrDefaultAsync());
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            catch
            {
                return View();
            }
        }
    }
}