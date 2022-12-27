using System.ComponentModel.DataAnnotations;
namespace travel_blog_api.Models;

public class Photo
{
    [Required]
    public string Id { get; set; } = default!;
    [Required]
    public string TripName { get; set; } = default!;
    [Required]
    public string PhotoName { get; set; } = default!;
    [Required]
    public string PhotoDescription { get; set; } = default!;
}
