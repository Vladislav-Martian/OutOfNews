using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfNews.Models;

namespace OutOfNews.Contexts
{
    public sealed class AuthDbContext: IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}