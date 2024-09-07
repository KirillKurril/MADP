using ALWD.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;


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
