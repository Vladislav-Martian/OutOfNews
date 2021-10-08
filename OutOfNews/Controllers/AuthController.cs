using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfNews.Extensions;
using OutOfNews.Models;
using OutOfNews.ViewModels;

namespace OutOfNews.Controllers
{
    public class AuthController : Controller
    {
        #region Inject
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion
        #region Authentication center
        // GET
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Me");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }
        #endregion

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Username, Born=model.Born};
                // new user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // set cookie
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(
                        string.Empty, 
                        "This user does not exists. Check your login.");
                    return View(model);
                }
                
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName, 
                    model.Password, 
                    model.RememberMe, 
                    false);
                if (result.Succeeded)
                {
                    // is url external or not
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, but your login or password are wrong :<(");
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Remove cookies
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Me()
        {
            return View(User.GetLoggedInUser(_userManager));
        }
    }
}