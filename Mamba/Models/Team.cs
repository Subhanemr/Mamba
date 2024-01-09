namespace Mamba.Models
{
    public class Team : BaseNameEntity
    {
        public string Img { get; set; } = null!;
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;
        public string? InstaLink { get; set; }
        public string? FaceLink { get; set; }
        public string? TwitLink { get; set; }
        public string? LinkedLink { get; set; }
    }
}
