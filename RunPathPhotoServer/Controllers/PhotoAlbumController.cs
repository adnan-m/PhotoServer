using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RunPathPhotoServer.Model;
using RunPathPhotoServer.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoAlbumController : ControllerBase
    {
        private readonly IRunPathDataService _runPathDataService;

        public PhotoAlbumController(IRunPathDataService runPathDataService)
        {
            _runPathDataService = runPathDataService;
        }

        // GET api/
        [HttpGet]
        public IEnumerable<PhotoAlbum> Get()
        {
            return _runPathDataService.GetPhotoAlbums();            
        }

        // GET api/user/{userId}
        [HttpGet("user/{userId}")]
        public IEnumerable<PhotoAlbum> Get(int userId)
        {
            return _runPathDataService.GetPhotoAlbumsByUser(userId);
        }      
        
    }
}
