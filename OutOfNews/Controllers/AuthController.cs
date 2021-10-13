using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration Config;
        public AuthController(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            Config = config;
        }
        #endregion
        #region Authentication center
        // GET
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
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
                    await _userManager.AddToRoleAsync(user, Config["Roles:DefaultRole"]);
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
        
        // Change password page
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View("DeleteAccount");
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAccountAction()
        {
            var user = User.GetLoggedInUser(_userManager);
            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index", "Home");
        }

        #region Edit-account

        [HttpGet]
        [Authorize]
        public IActionResult Me()
        {
            return View(User.GetLoggedInUser(_userManager));
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteNickname()
        {
            var user = User.GetLoggedInUser(_userManager);
            user.NickName = null;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Me");
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePhone()
        {
            var user = User.GetLoggedInUser(_userManager);
            user.PhoneNumber = null;
            user.PhoneNumberConfirmed = false;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Me");
        }
        
        
        [HttpPost] // partial change of server resource
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User.GetLoggedInUser(_userManager);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("ChangePassword", model);
                }
                return RedirectToAction("Index");
            }
            return View("ChangePassword", model);
        }
        
        [HttpPost] // partial change of server resource
        [Authorize]
        public async Task<IActionResult> ChangeName(ChangeNameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User.GetLoggedInUser(_userManager);
                user.NickName = model.NickName;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View("Me");
        }
        
        [HttpPost] // partial change of server resource
        [Authorize]
        public async Task<IActionResult> ChangeContacts(ChangeContactsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User.GetLoggedInUser(_userManager);

                if (user.Email != model.Email)
                {
                    var tokenMail = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    await _userManager.ChangeEmailAsync(user, model.Email, tokenMail);
                }
                
                if (user.PhoneNumber != model.PhoneNumber)
                {
                    var tokenPhone = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
                    await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, tokenPhone);
                }
                
                return RedirectToAction("Index");
            }
            return View("Me");
        }

        #endregion
        
    }
}