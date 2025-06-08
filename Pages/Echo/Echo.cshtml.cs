
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskTracker.Pages.Echo
{
    public class EchoModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; } = string.Empty;

        public string? EchoedText { get; set; }

        public void OnGet()
        {
            //Initialize or reset state
            EchoedText = null;
        }

        public void OnPost()
        {
            EchoedText = InputText;
        }
    }
}
