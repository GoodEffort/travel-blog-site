using System.ComponentModel.DataAnnotations;

namespace travel_blog_api.Models;

public partial class Photo
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string TripName { get; set; } = null!;
    [Required]
    public string PhotoName { get; set; } = null!;
    public string? Description { get; set; }
}
