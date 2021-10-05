using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfNews.Contexts;
using OutOfNews.FormModels;
using OutOfNews.Models;

namespace OutOfNews.Controllers
{
    public class AuthController: Controller
    {
        #region Injection
        private AuthDbContext db;

        public AuthController(AuthDbContext context)
        {
            db = context;
        }
        #endregion
        
        #region GET
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        #endregion
        
        #region POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .FirstOrDefaultAsync(u => u.Mail == model.Email 
                                              && u.Pass == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);
 
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Wrong auth data!");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Mail == model.Email);
                if (user == null)
                {
                    var newUserConfig = new UserConfig
                    {
                        Nickname = model.Nickname,
                        UseNickname = !string.IsNullOrEmpty(model.Nickname)
                    };

                    var newUser = new User
                    {
                        Mail = model.Email,
                        Pass = model.Password,
                        Name = model.Name,
                        BornDate = model.BornDate,
                        UserConfig = newUserConfig
                    };

                    db.Users.Add(newUser);
                    db.UserConfigs.Add(newUserConfig);
                    
                    await db.SaveChangesAsync();
 
                    await Authenticate(model.Email);
 
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Wrong auth data!");
            }
            return View(model);
        }
        
        #endregion

        #region Authentification

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(
                claims, 
                "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        #endregion
        
    }
}