using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;

namespace TaskTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options) { }
        public DbSet<TaskItem> TaskItem { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<TaskItem>()
             .Property(t => t.Title)
             .HasColumnType("TEXT");
        }



    }
}
