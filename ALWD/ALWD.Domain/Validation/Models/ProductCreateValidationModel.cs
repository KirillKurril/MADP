using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace ALWD.Domain.Validation.Models
{
    public class ProductCreateValidationModel
    {
        [Required(ErrorMessage = "Поле названия обязательное для заполнения")]
        [MaxStringLength(100, ErrorMessage = "Название должно быть не более 100 символов.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле названия обязательное для заполнения")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле стоимости обязательное для заполнения")]
        [Range(0.01, 100000, ErrorMessage = "Цена должна быть в диапазоне от 0.01 до 1000.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Поле количества обязательное для заполнения")]
        [Range(1, 100000, ErrorMessage = "Количество должно быть от 1 до 100 000.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Категория товара должна быть указана")]
        public int CategoryId { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(15 * 1024 * 1024, ErrorMessage = "Размер файла не должен превышать 5MB.")]
        [AllowedExtensions(new string[]{ ".jpg", ".png", ".jpeg" }, ErrorMessage = "Разрешены только файлы с расширениями .jpg, .jpeg и .png.")]
        [AllowedMimeType(new string[] { "image/jpeg", "image/png" }, ErrorMessage = "Разрешены только файлы с медиатипом image/jpeg и image/png")]
        public IFormFile? Image { get; set; }


    }
}
