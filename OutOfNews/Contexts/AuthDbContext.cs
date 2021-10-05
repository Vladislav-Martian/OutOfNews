using Microsoft.EntityFrameworkCore;
using OutOfNews.Models;

namespace OutOfNews.Contexts
{
    public sealed class AuthDbContext: DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserConfig> UserConfigs { get; set; }
    }
}