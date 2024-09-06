using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ALWD.UI.ViewModels
{
    public class CatalogViewModel
    {
        public CatalogViewModel(ListModel<Product> products, List<Category> categories)
        {
            Products = products;
            Categories = categories;
        }

        public ListModel<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
