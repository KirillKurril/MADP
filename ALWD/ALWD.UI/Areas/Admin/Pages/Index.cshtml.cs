using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ALWD.UI.Areas.Admin.Pages
{
	//[Authorize(Roles = "admin")]
	public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
