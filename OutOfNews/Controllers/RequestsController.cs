using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfNews.Contexts;
using OutOfNews.Extensions;
using OutOfNews.Models;
using OutOfNews.ViewModels;

namespace OutOfNews.Controllers
{
    public class RequestsController: Controller
    {
        #region Inject

        private AppDbContext _db;
        private UserManager<User> _userManager;

        public RequestsController(AppDbContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        #endregion

        [HttpGet]
        [Authorize]
        public IActionResult Index(RequestViewModel model = null)
        {
            if (model == null)
            {
                model = new RequestViewModel()
                {
                    Heading = "",
                    Message = "",
                    RequestVariant = AdminRequestVariant.Other
                };
            }
            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = "moder, admin")]
        public IActionResult List()
        {
            return View(_db.ModerationRequests
                .AsQueryable()
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .ToList());
        }

        #region Posts

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SendRequest(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var full = new ModerationRequest()
                {
                    Heading = model.Heading,
                    Message = model.Message,
                    RequestVariant = model.RequestVariant,
                    UserId = User.GetLoggedInUserId<string>()
                };
                await _db.ModerationRequests.AddAsync(full);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RemoveRequest(int requestId)
        {
            if (!(User.IsInRole("moder") || User.IsInRole("admin")))
            {
                return RedirectToAction("Index", "Home");
            }

            var request = await _db.ModerationRequests.FindAsync(requestId as object);
            _db.Remove(request);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }
        
        #endregion
        
    }
}