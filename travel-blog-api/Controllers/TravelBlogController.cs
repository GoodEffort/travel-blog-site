using Microsoft.AspNetCore.Mvc;
using System.IO;
using travel_blog_api.Models;
using travel_blog_api.Repos;

namespace travel_blog_api.Controllers;

[ApiController]
[Route("[controller]")]
public class TravelBlogController : ControllerBase
{

    private readonly ILogger<TravelBlogController> _logger;
    private readonly IMongoDBRepo _repo;

    public TravelBlogController(ILogger<TravelBlogController> logger, IMongoDBRepo repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [HttpGet("FindPhotos", Name = "GetPhotos")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedPhotos))]
    public IActionResult Get(string TripName, int page = 0, int pageSize = 10)
    {
        var photos = _repo.Get(TripName, page, pageSize);
        return Ok(photos);
    }

    [HttpGet("Photo", Name = "GetPhoto")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Photo))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPhoto(string photoId)
    {
        var photo = _repo.Get(photoId);
        
        if (photo == null)
            return NotFound();
        else
            return Ok(photo);
    }

    [HttpPost("Photo", Name = "CreatePhoto")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Photo))]
    public IActionResult Create(Photo photo)
    {
        _repo.Create(photo);
        return CreatedAtRoute("GetPhotos", new { id = photo.Id.ToString() }, photo);
    }
}
