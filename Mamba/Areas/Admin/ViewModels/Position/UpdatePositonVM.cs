using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class UpdatePositonVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string Name { get; set; }
    }
}
