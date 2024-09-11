using ALWD.Domain.Entities;
using ALWD.Domain.Models;


namespace ALWD.Domain.ViewModels
{
    public class CatalogViewModel
    {
        public CatalogViewModel(
            ResponseData<ListModel<Product>> productResponse,
            ResponseData<IReadOnlyList<Category>> categories)
        {
            ProductResponse = productResponse;
            Categories = categories;
        }

        public ResponseData<ListModel<Product>> ProductResponse { get; set; }
        public ResponseData<IReadOnlyList<Category>> Categories { get; set; }
    }
}
