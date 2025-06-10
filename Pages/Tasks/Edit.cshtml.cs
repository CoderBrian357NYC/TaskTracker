using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(AppDbContext context, UserManager<ApplicationUser> userManager)
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
            TaskItem = taskitem;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            //Fix: manually set userId
            TaskItem.UserId = userId;

            //Fix: Remove the validation error or USerId
            ModelState.Remove("TaskItem.UserId");

            if (!ModelState.IsValid)
            {
                return Page();
            }


            var existingTask = await _context.TaskItem.FirstOrDefaultAsync(t => t.Id == TaskItem.Id && t.UserId == userId);

            if (existingTask == null)
            {
                return NotFound();  // Or return Forbid() if you prefer
            }

            // Ensure the TaskItem's UserId is set to the current user to avoid changes
            existingTask.Title = TaskItem.Title;
            existingTask.IsComplete = TaskItem.IsComplete;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(TaskItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItem.Any(e => e.Id == id);
        }
    }
}
