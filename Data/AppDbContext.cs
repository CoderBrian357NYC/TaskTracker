using System.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;
using TaskTracker.Data;

namespace TaskTracker.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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
