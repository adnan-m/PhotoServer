using System.Collections.Generic;
using RunPathPhotoServer.Repository.Data;

namespace RunPathPhotoServer.Repository
{
    public interface IPhotoRepository
    {
        IReadOnlyCollection<PhotoData> GetAll();

        IReadOnlyCollection<PhotoData> GetByAlbum(int[] albumIds);
    }
}