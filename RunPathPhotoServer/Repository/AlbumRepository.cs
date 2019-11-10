using System;
using System.Collections.Generic;
using System.Linq;
using RunPathPhotoServer.Repository.Data;
using RunPathPhotoServer.Client;


namespace RunPathPhotoServer.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly IRunpathClient _client;

        public AlbumRepository(IRunpathClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IReadOnlyCollection<AlbumData> GetAll() => _client.GetAlbums().Result;                    

        public IReadOnlyCollection<AlbumData> GetByUser(int userId)
        {
            var albums = _client.GetAlbums().Result;
            return albums.Where(a => a.UserId == userId).ToList();
        }
    }
}
