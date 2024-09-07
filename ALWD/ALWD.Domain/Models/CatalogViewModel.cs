using ALWD.Domain.Entities;
using ALWD.Domain.Models;


namespace ALWD.Domain.ViewModels
{
    public class CatalogViewModel
    {
        public CatalogViewModel(ResponseData<ListModel<Product>> productResponse, List<Category> categories)
        {
            ProductResponse = productResponse;
            Categories = categories;
        }

        public ResponseData<ListModel<Product>> ProductResponse { get; set; }
        public List<Category> Categories { get; set; }
    }
}
