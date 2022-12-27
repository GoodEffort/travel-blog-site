using travel_blog_api.Repos;
using System.ComponentModel.DataAnnotations;

namespace travel_blog_api.Models;

public class PagedPhotos {
    [Required]
    public int Page { get; set; }
    [Required]
    public int PageSize { get; set; }
    [Required]
    public int TotalPages { get; set; }
    [Required]
    public int TotalItems { get; set; }
    [Required]
    public List<Photo> Photos { get; set; } = new List<Photo>();
}
