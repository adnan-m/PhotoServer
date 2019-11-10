using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RunPathPhotoServer.Client;
using RunPathPhotoServer.Repository;
using RunPathPhotoServer.Repository.Data;


namespace RunPathPhotoServer.Test.Repository
{   
    public class PhotoRepositoryTests
    {
        private Mock<IRunpathClient> _mockClient;
        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRunpathClient>();
        }

        [Test]
        public void Initiate_WithoutHttpClient_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PhotoRepository(null));
        }

        [Test]
        public void GetAll_Returns_NoData()
        {
            // Arrange
            _mockClient.Setup(a => a.GetPhotos()).Returns(Task.FromResult((IReadOnlyCollection<PhotoData>)new List<PhotoData>()));
            var sut = new PhotoRepository(_mockClient.Object);

            // Act 
            var result = sut.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetAll_Returns_Data()
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

            IReadOnlyCollection<PhotoData> data = new List<PhotoData> { photoData };
            _mockClient.Setup(a => a.GetPhotos()).Returns(Task.FromResult(data));
            var sut = new PhotoRepository(_mockClient.Object);

            // Act 
            var result = sut.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            CollectionAssert.AreEquivalent(data, result);
        }

        [Test]
        public void GetByUser_AlbumIdDoesNotMatch_NoPhotos()
        {
            // Arrange
            int albumId = 1;
            int[] albumIds = new int[] { albumId };
            var photoData = new PhotoData
            {
                AlbumId = 2,
                Id = 50,
                Title = "et inventore quae ut tempore eius voluptatum",
                Url = new Uri("https://via.placeholder.com/600/9e59da"),
                ThumbnailUrl = new Uri("https://via.placeholder.com/150/9e59da")
            };

            IReadOnlyCollection<PhotoData> data = new List<PhotoData> { photoData };
            _mockClient.Setup(a => a.GetPhotos()).Returns(Task.FromResult(data));
            var sut = new PhotoRepository(_mockClient.Object);

            // Act 
            var result = sut.GetByAlbum(albumIds);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetByUser_AlbumIdMatch_Photos()
        {
            // Arrange
            int albumId = 1;
            int[] albumIds = new int[] { albumId };
            var photoData = new PhotoData
            {
                AlbumId = albumId,
                Id = 50,
                Title = "et inventore quae ut tempore eius voluptatum",
                Url = new Uri("https://via.placeholder.com/600/9e59da"),
                ThumbnailUrl = new Uri("https://via.placeholder.com/150/9e59da")
            };

            IReadOnlyCollection<PhotoData> data = new List<PhotoData> { photoData };
            _mockClient.Setup(a => a.GetPhotos()).Returns(Task.FromResult(data));
            var sut = new PhotoRepository(_mockClient.Object);

            // Act 
            var result = sut.GetByAlbum(albumIds);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            CollectionAssert.AreEquivalent(data, result);
        }
    }
}
