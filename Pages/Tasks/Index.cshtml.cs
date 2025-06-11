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

        private const int PageSize = 5; // fixed page size

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? ShowCompleted { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;  // current page number, default to 1
        public IList<TaskItem> TaskItems { get; set; } = default!;

        public int TotalPages { get; set; }  // total number of pages

        public IndexModel(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task OnGetAsync()
        {
            var userId = _userManager!.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                TaskItems = new List<TaskItem>();
                TotalPages = 0;
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

            // Calculate total count for pagination
            var totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            // Clamp PageNumber between 1 and TotalPages
            if (PageNumber < 1) PageNumber = 1;
            if (PageNumber > TotalPages) PageNumber = TotalPages;

            // Fetch only the current page of results
            TaskItems = await query
                .OrderBy(t => t.CreatedAt)  // order consistently
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
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