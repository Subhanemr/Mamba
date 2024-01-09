using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
