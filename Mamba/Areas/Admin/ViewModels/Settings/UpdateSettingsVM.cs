using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class UpdateSettingsVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public string Value { get; set; }
    }
}
