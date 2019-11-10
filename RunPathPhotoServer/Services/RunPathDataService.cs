using System;
using System.Collections.Generic;
using System.Linq;
using RunPathPhotoServer.Model;
using RunPathPhotoServer.Repository;

namespace RunPathPhotoServer.Services
{
    public class RunPathDataService : IRunPathDataService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IAlbumRepository _albumRepository;

        public RunPathDataService(IPhotoRepository photoRepository, IAlbumRepository albumRepository)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        }

        public IEnumerable<PhotoAlbum> GetPhotoAlbums() 
        {
            var photos = _photoRepository.GetAll().ToList().Select(data => new Photo { Id = data.Id, AlbumId = data.AlbumId, Title = data.Title, ThumbnailUrl = data.ThumbnailUrl, Url = data.Url });
            var albums = _albumRepository.GetAll().ToList().Select(data => new Album { Id = data.Id, UserId = data.UserId, Title = data.Title });
            return ProcessPhotoAndAlbums(photos, albums);
        }

        public IEnumerable<PhotoAlbum> GetPhotoAlbumsByUser(int userId)
        {
            var albums = _albumRepository.GetByUser(userId).ToList().Select(data =>  new Album{ Id = data.Id, UserId = data.UserId, Title= data.Title });
            var albumIds = albums.Select(a => a.Id).ToArray();
            var photos = _photoRepository.GetByAlbum(albumIds).ToList().Select(data => new Photo { Id = data.Id, AlbumId = data.AlbumId, Title = data.Title, ThumbnailUrl = data.ThumbnailUrl, Url = data.Url });           
            return ProcessPhotoAndAlbums(photos, albums);
        }

        private IEnumerable<PhotoAlbum> ProcessPhotoAndAlbums(IEnumerable<Photo> photos, IEnumerable<Album> albums) 
        {
            var photosAlbumDictionary = GetPhotoAlbum(photos);
            return GetPhotoAlbums(albums, photosAlbumDictionary);            
        }

        private Dictionary<int, List<Photo>> GetPhotoAlbum(IEnumerable<Photo> photos) => photos
                .GroupBy(photo => photo.AlbumId)
                .ToDictionary(p => p.Key, p => p.ToList());

        private IEnumerable<PhotoAlbum> GetPhotoAlbums(IEnumerable<Album> albums, Dictionary<int, List<Photo>> photosAlbumDictionary) => albums.Select(album => new PhotoAlbum
            {
                Album = album,
                Photos = photosAlbumDictionary.GetValueOrDefault(album.Id) ?? new List<Photo>()
        });

        private IEnumerable<PhotoAlbum> GetPhotoAlbums(IEnumerable<Album> albums, Dictionary<int, List<Photo>> photosAlbumDictionary,int userId) => albums.Select(album => new PhotoAlbum
        {
            Album = album,
            Photos = photosAlbumDictionary.GetValueOrDefault(album.Id) ?? new List<Photo>()
        });
    }
}
