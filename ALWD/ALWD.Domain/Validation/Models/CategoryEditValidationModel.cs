using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.Validation.Models
{
    public class CategoryEditValidationModel : CategoryCreateValidationModel
    {
        [Required(ErrorMessage = "Id property is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be positive integer")]
        public int Id { get; set; }
    }
}
