using Mamba.Models;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public decimal Price { get; set; }

        public string? Description { get; set; }


        [Required(ErrorMessage = "Is Reqiured")]
        public int CategoryId { get; set; }

        public ICollection<Category>? Categories { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public IFormFile Photo { get; set; }
    }
}
