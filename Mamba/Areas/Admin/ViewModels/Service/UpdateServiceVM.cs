using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class UpdateServiceVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public string Icon { get; set; }
    }
}
