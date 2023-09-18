using System.Data.Entity;
using travel_blog_api.Context;
using travel_blog_api.Models;

namespace travel_blog_api.Repos;

public class TravelBlogDepo : ITravelBlogRepo
{
    private readonly TravelblogContext Context;

    public TravelBlogDepo(TravelblogContext context)
    {
        Context = context;
    }

    public Photo Create(Photo photo)
    {
        Context.Photos.Add(photo);

        Context.SaveChanges();

        return photo;
    }

    public PagedPhotos Get(string trip, int page, int pageSize = 10)
    {
        List<Photo> photos = Context.Photos.Where(p => p.TripName == trip).Skip(page * pageSize).Take(pageSize).ToList();
        int count = Context.Photos.Where(p => p.TripName == trip).Count();

        return new PagedPhotos {
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)count / pageSize),
            TotalItems = count,
            Photos = photos
        };
    }

    public Photo? Get(int id)
    {
        return Context.Photos.Where(p => p.Id == id).FirstOrDefault();
    }

    public void Remove(Photo photoIn)
    {
        Context.Photos.Remove(photoIn);
        Context.SaveChanges();
    }

    public void Update(int id, string? Description, string? TripName, string? PhotoName)
    {
        var photo = Context.Photos.Where(p => p.Id == id).FirstOrDefault() ?? throw new Exception("Photo not found");

        if (Description != null)
            photo.Description = Description;
        if (TripName != null)
            photo.TripName = TripName;
        if (PhotoName != null)

        Context.SaveChanges();
    }
}