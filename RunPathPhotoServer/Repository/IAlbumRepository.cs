using System.Collections.Generic;
using RunPathPhotoServer.Repository.Data;

namespace RunPathPhotoServer.Repository
{
    public interface IAlbumRepository
    {
        IReadOnlyCollection<AlbumData> GetAll();
        IReadOnlyCollection<AlbumData> GetByUser(int userId);
    }
}