using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.Validation.Models
{
    public class CategoryCreateValidationModel
    {
        [Required(ErrorMessage = "Поле названия обязательное для заполнения")]
        [MaxStringLength(100, ErrorMessage = "Название должно быть не более 100 символов.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле нормализованного названия обязательное для заполнения")]
        [MaxStringLength(100, ErrorMessage = "Нормализованное название должно быть не более 100 символов.")]
        [KebabNotation(ErrorMessage = "Строка должна быть в формате kebab-notation: " +
                                                "начинаться и заканчиваться на букву, содержать только латинские буквы и символ '-'.\"")]
        public string NormalizedName { get; set; }
    }
}
