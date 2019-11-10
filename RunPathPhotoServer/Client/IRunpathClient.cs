using System.Collections.Generic;
using System.Threading.Tasks;
using RunPathPhotoServer.Repository.Data;

namespace RunPathPhotoServer.Client
{
    public interface IRunpathClient
    {
        Task<IReadOnlyCollection<AlbumData>> GetAlbums();
        Task<IReadOnlyCollection<PhotoData>> GetPhotos();
    }
}
