

using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.DTOs
{
    public class UpdateProductDTO : CreateProductDTO
    {

        [Required(ErrorMessage = "Id property is required")]
        [Range(1, int.MaxValue, ErrorMessage ="Id must be positive integer")]
        public int ProductId { get; set; }
    }
}
