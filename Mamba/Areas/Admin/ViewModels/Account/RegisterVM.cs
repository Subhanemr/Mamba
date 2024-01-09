using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Password be same")]
        public string ConfirmedPassword { get; set; }
    }
}
