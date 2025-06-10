using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Pages.Tasks
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(TaskTracker.Data.AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var taskitem = await _context.TaskItem.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (taskitem == null)
            {
                return NotFound();
            }
            else
            {
                TaskItem = taskitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var taskitem = await _context.TaskItem.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskitem == null) return NotFound();

            TaskItem = taskitem;
            _context.TaskItem.Remove(TaskItem);
            await _context.SaveChangesAsync();
            

            return RedirectToPage("./Index");
        }
    }
}
