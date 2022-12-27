using travel_blog_api.Models;

namespace travel_blog_api.Repos;

public interface IMongoDBRepo
{
    PagedPhotos Get(string trip, int page, int pageSize = 10);
    Photo Get(string id);
    Photo Create(Photo photo);
    void Update(string id, Photo photoIn);
    void Remove(Photo photoIn);
    void Remove(string id);
}