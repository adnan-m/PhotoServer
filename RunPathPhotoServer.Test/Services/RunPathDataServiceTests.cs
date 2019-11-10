using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using RunPathPhotoServer.Model;
using RunPathPhotoServer.Repository;
using RunPathPhotoServer.Repository.Data;
using RunPathPhotoServer.Services;


namespace RunPathPhotoServer.Test.Services
{
    public class RunPathDataServiceTests
    {
        private Mock<IPhotoRepository> _mockPhotoRepository;
        private Mock<IAlbumRepository> _mockAlbumRepository;

        [SetUp]
        public void Initialise()
        {
            _mockPhotoRepository = new Mock<IPhotoRepository>();
            _mockAlbumRepository = new Mock<IAlbumRepository>();
        }

        [Test]
        public void Initiate_WithoutPhotoRepository_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RunPathDataService(null, _mockAlbumRepository.Object));
        }

        [Test]
        public void Initiate_WithoutAlbumRepository_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RunPathDataService(_mockPhotoRepository.Object, null));
        }

        [Test]
        public void GetPhotoAlbums_Returns_NoData()
        {
            // Arrange
            _mockPhotoRepository.Setup(a => a.GetAll()).Returns(new List<PhotoData>());
            _mockAlbumRepository.Setup(a => a.GetAll()).Returns(new List<AlbumData>());
            var sut = new RunPathDataService(_mockPhotoRepository.Object, _mockAlbumRepository.Object);

            // Act 
            var result = sut.GetPhotoAlbums();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetPhotoAlbums_AlbumIdMatchWithPhoto_ReturnsAlbumAndPhotos()
        {
            // Arrange
            var photoData = new PhotoData
            {
                AlbumId = 1,
                Id = 50,
                Title = "et inventore quae ut tempore eius voluptatum",
                Url = new Uri("https://via.placeholder.com/600/9e59da"),
                ThumbnailUrl = new Uri("https://via.placeholder.com/150/9e59da")
            };

            var albumData = new AlbumData
            {
                UserId = 1,
                Id = 1,
                Title = "saepe unde necessitatibus rem"
            };
            _mockPhotoRepository.Setup(a => a.GetAll()).Returns(new List<PhotoData> { photoData });
            _mockAlbumRepository.Setup(a => a.GetAll()).Returns(new List<AlbumData> { albumData });
            var sut = new RunPathDataService(_mockPhotoRepository.Object, _mockAlbumRepository.Object);

            // Act 
            var result = sut.GetPhotoAlbums();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            AssertAlubm(result.First().Album, albumData);
            AssertPhoto(result.First().Photos.First<Photo>(), photoData);
        }     

        [TestCase()]
        public void GetPhotoAlbums_NoAlbumIdMatchReturns_AlbumWithoutPhotos()
        {
            // Arrange  
            var photoData = new PhotoData
            {
                AlbumId = 1,
                Id = 50,
                Title = "et inventore quae ut tempore eius voluptatum",
                Url = new Uri("https://via.placeholder.com/600/9e59da"),
                ThumbnailUrl = new Uri("https://via.placeholder.com/150/9e59da")
            };

            var albumData = new AlbumData
            {
                UserId = 1,
                Id = 9,
                Title = "saepe unde necessitatibus rem"
            };
            _mockPhotoRepository.Setup(a => a.GetAll()).Returns(new List<PhotoData> ());
            _mockAlbumRepository.Setup(a => a.GetAll()).Returns(new List<AlbumData> { albumData });
            var sut = new RunPathDataService(_mockPhotoRepository.Object, _mockAlbumRepository.Object);

            // Act 
            var result = sut.GetPhotoAlbums();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            AssertAlubm(result.First().Album, albumData);
            Assert.AreEqual(0, result.First().Photos.Count);
        }

        [TestCase()]
        public void GetPhotoAlbumsByUser_UsrIdMatchReturns_AlbumAndPhotos()
        {
            // Arrange  
            int userId = 1;
            int albumId = 2;
            var photoData = new PhotoData
            {
                AlbumId = albumId,
                Id = 50,
                Title = "et inventore quae ut tempore eius voluptatum",
                Url = new Uri("https://via.placeholder.com/600/9e59da"),
                ThumbnailUrl = new Uri("https://via.placeholder.com/150/9e59da")
            };

            var albumData = new AlbumData
            {
                UserId = userId,
                Id = albumId,
                Title = "saepe unde necessitatibus rem"
            };

            _mockPhotoRepository.Setup(a => a.GetByAlbum(new int[] { albumId })).Returns(new List<PhotoData> { photoData });
            _mockAlbumRepository.Setup(a => a.GetByUser(userId)).Returns(new List<AlbumData> { albumData });
            var sut = new RunPathDataService(_mockPhotoRepository.Object, _mockAlbumRepository.Object);

            // Act 
            var result = sut.GetPhotoAlbumsByUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            AssertAlubm(result.First().Album, albumData);
            AssertPhoto(result.First().Photos.First<Photo>(), photoData);
        }

        [TestCase()]
        public void GetPhotoAlbumsByUser_UsrIdDoesNotMatchReturns_MatchingAlbumAndNoPhotos()
        {
            // Arrange  
            int albumUserId = 2;
            int desireUserId = 1;
            int albumId = 2;
            var photoData = new PhotoData
            {
                AlbumId = 3,
                Id = 50,
                Title = "et inventore quae ut tempore eius voluptatum",
                Url = new Uri("https://via.placeholder.com/600/9e59da"),
                ThumbnailUrl = new Uri("https://via.placeholder.com/150/9e59da")
            };

            var albumData = new AlbumData
            {
                UserId = albumUserId,
                Id = 3,
                Title = "saepe unde necessitatibus rem"
            };

            var albumDataDesire = new AlbumData
            {
                UserId = desireUserId,
                Id = albumId,
                Title = "distinctio laborum qui"
            };
            
            _mockPhotoRepository.Setup(a => a.GetByAlbum(new int[] { albumId })).Returns(new List<PhotoData> { photoData } );
            _mockAlbumRepository.Setup(a => a.GetByUser(desireUserId)).Returns(new List<AlbumData> { albumDataDesire } );
            var sut = new RunPathDataService(_mockPhotoRepository.Object, _mockAlbumRepository.Object);

            // Act 
            var result = sut.GetPhotoAlbumsByUser(desireUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            AssertAlubm(result.First().Album, albumDataDesire);
            Assert.AreEqual(0, result.First().Photos.Count);
        }

        private void AssertAlubm(Album album, AlbumData albumData)
        {
            Assert.AreEqual(album.Id, albumData.Id);
            Assert.AreEqual(album.Title, albumData.Title);
            Assert.AreEqual(album.UserId, albumData.UserId);
        }

        private void AssertPhoto(Photo photo, PhotoData photoData)
        {
            Assert.AreEqual(photo.Id, photoData.Id);
            Assert.AreEqual(photo.Title, photoData.Title);
            Assert.AreEqual(photo.AlbumId, photoData.AlbumId);
            Assert.AreEqual(photo.ThumbnailUrl.ToString(), photoData.ThumbnailUrl.ToString());
            Assert.AreEqual(photo.Url.ToString(), photoData.Url.ToString());
        }
    }
}
