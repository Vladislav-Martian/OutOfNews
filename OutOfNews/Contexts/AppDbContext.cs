using Microsoft.EntityFrameworkCore;
using OutOfNews.Models;

namespace OutOfNews.Contexts
{
    public sealed class AppDbContext: DbContext
    {
        #region Tables
        public DbSet<ModerationRequest> ModerationRequests { get; set; }

        #endregion
        
        
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}