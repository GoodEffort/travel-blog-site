using travel_blog_api.Models;

namespace travel_blog_api.Repos;

public interface ITravelBlogRepo
{
    PagedPhotos Get(string trip, int page, int pageSize = 10);
    Photo? Get(int id);
    Photo Create(Photo photo);
    void Update(int id, string? Description, string? TripName, string? PhotoName);
    void Remove(Photo photoIn);
}