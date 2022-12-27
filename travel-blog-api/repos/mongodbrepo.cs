using Newtonsoft.Json;
using MongoDB.Driver;
using travel_blog_api.Models;

namespace travel_blog_api.Repos;

public class MongoDBRepo : IMongoDBRepo
{
    private readonly IMongoCollection<Photo> _photos;

    public MongoDBRepo()
    {
        string username = "";
        string password = "";
        string connectionString = "";

        try {
            using StreamReader r = new StreamReader("credentials/mongodb.json");
            string json = r.ReadToEnd();
            dynamic o = JsonConvert.DeserializeObject(json) ?? throw new Exception();
            username = o["username"];
            password = o["password"];
            connectionString = o["connectionString"];
        }
        catch
        {
            throw new Exception("MongoDB credentials file is invalid. Please add your credentials to credentials/mongodb.json.");
        }

        MongoClient client = new MongoClient($"mongodb+srv://{username}:{password}@{connectionString}");
        IMongoDatabase database = client.GetDatabase("travel-blog");
        _photos = database.GetCollection<Photo>("photos");
    }

    public Photo Get(string id)
    {
        return _photos.Find<Photo>(photo => photo.Id == id).FirstOrDefault();
    }

    public PagedPhotos Get(string trip, int page, int pageSize = 10)
    {
        List<Photo> Photos = _photos
            .Find<Photo>(photo => photo.TripName == trip)
            .Skip(page * pageSize)
            .Limit(pageSize)
            .ToList();

        long TotalItems = _photos
            .Find<Photo>(photo => photo.TripName == trip)
            .CountDocuments();

        PagedPhotos pagedPhotos = new PagedPhotos()
        {
            Photos = Photos,
            TotalItems = (int)TotalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)TotalItems / pageSize)
        };

        return pagedPhotos;
    }

    public Photo Create(Photo photo)
    {
        _photos.InsertOne(photo);
        return photo;
    }

    public void Update(string id, Photo photoIn)
    {
        _photos.ReplaceOne(photo => photo.Id == id, photoIn);
    }

    public void Remove(Photo photoIn)
    {
        _photos.DeleteOne(photo => photo.Id == photoIn.Id);
    }

    public void Remove(string id)
    {
        _photos.DeleteOne(photo => photo.Id == id);
    }
}