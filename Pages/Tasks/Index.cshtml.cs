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
using Microsoft.AspNetCore.Authorization;

namespace TaskTracker.Pages.Tasks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TaskTracker.Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser>? _userManager;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? ShowCompleted { get; set; }

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

            var query = _context.TaskItem.Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(SearchString))
            {
                var lowerSearch = SearchString.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(lowerSearch));
            }

            if (ShowCompleted.HasValue)
            {
                query = query.Where(t => t.IsComplete == ShowCompleted.Value);
            }

            TaskItems = await query.ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleCompleteAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _context.TaskItem.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            task.IsComplete = !task.IsComplete; // Toggle the value
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}