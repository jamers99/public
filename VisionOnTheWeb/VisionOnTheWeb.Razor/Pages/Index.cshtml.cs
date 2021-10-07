using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VisionOnTheWeb.Razor.Pages
{
    public class IndexModel : PageModel
    {
        public VisionWeb Vision { get; private set; }

        public async Task OnGet()
        {
            Vision = new VisionWeb();
            await Vision.Login();
            await Vision.GetCustomer();
        }
    }
}