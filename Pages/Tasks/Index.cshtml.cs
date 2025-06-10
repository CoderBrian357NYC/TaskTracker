using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly TaskTracker.Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser>? _userManager;

        public IndexModel(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<TaskItem> TaskItems { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = _userManager!.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                TaskItems = new List<TaskItem>();
                return;
            }

            TaskItems = await _context.TaskItem
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }
}
