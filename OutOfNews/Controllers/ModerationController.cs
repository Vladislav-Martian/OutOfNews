using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class ModerationController : Controller
    {
        #region Injects

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public ModerationController(UserManager<User> userManager, IConfiguration configuration, AppDbContext db)
        {
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
        }

        #endregion
        
        public ActionResult Edit([FromRoute]int id)
        {
            User user = User.GetLoggedInUser(_userManager);
            var article = _db.Articles.AsQueryable()
                .FirstOrDefault(a => a.Id == id);
            
            if (article == null)
            {
                ModelState.AddModelError("", "Article don`t exists, or does not belong to you");
                return BadRequest();
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
                return RedirectToAction("Index", "Home");
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
            var article = await _db.Articles.AsQueryable().Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            
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
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }
    }
}