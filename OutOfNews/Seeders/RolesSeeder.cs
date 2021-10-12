using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OutOfNews.Models;

namespace OutOfNews.Seeders
{
    public class RolesSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string anonymousWriterLogin = "anonas@nomail.com";
            string anonymousWriterPassword = "0anonas";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("moder") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moder"));
            }
            if (await roleManager.FindByNameAsync("author") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("author"));
            }
            if (await roleManager.FindByNameAsync("reader") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("reader"));
            }
            if (await userManager.FindByNameAsync(anonymousWriterLogin) == null)
            {
                User anonas = new User
                {
                    Email = anonymousWriterLogin, 
                    UserName = "Anonas",
                    Born = new DateTime(1998, 1, 1),
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(anonas, anonymousWriterPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(anonas, "author");
                    await userManager.AddToRoleAsync(anonas, "reader");
                }
            }
        }
    }
}