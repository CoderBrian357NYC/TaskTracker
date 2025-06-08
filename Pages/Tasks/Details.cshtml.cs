using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;

namespace TaskTracker.Pages.Tasks
{
    public class DetailsModel : PageModel
    {
        private readonly TaskTracker.Data.AppDbContext _context;

        public DetailsModel(TaskTracker.Data.AppDbContext context)
        {
            _context = context;
        }

        public TaskItem TaskItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskitem = await _context.TaskItem.FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
