using a = Mamba.Models;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.Admin.ViewModels
{
    public class CreateTeamVM
    {
        [Required(ErrorMessage = "Is Reqiured")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is Reqiured")]
        public int PositionId { get; set; }


        [Required(ErrorMessage = "Is Reqiured")]
        public IFormFile Photo { get; set; }

        public string? InstaLink { get; set; }
        public string? FaceLink { get; set; }
        public string? TwitLink { get; set; }
        public string? LinkedLink { get; set; }

        public ICollection<a.Position>? Positions { get; set; }
    }
}
