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
    public class IndexModel : PageModel
    {
        private readonly TaskTracker.Data.AppDbContext _context;

        public IndexModel(TaskTracker.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<TaskItem> TaskItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            TaskItem = await _context.TaskItem.ToListAsync();
        }
    }
}
