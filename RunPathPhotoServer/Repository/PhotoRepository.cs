using System;
using System.Collections.Generic;
using RunPathPhotoServer.Repository.Data;
using RunPathPhotoServer.Client;
using System.Linq;

namespace RunPathPhotoServer.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IRunpathClient _client;

        public PhotoRepository(IRunpathClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IReadOnlyCollection<PhotoData> GetAll() => _client.GetPhotos().Result;

        public IReadOnlyCollection<PhotoData> GetByAlbum(int[] albumIds)
        {
            var photos = _client.GetPhotos().Result;
            return photos.Where(p => albumIds.Contains(p.AlbumId)).ToList();
        }
    }
}
