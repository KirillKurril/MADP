using Microsoft.AspNetCore.Mvc;
using ALWD.UI.ViewModels;

namespace ALWD.UI.Components
{
    public class PaginationBar : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
