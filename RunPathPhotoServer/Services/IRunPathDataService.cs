using System.Collections.Generic;
using RunPathPhotoServer.Model;

namespace RunPathPhotoServer.Services
{
    public interface IRunPathDataService
    {
        IEnumerable<PhotoAlbum> GetPhotoAlbums();
        IEnumerable<PhotoAlbum> GetPhotoAlbumsByUser(int userId);
    }
}
