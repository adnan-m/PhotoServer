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
    public class AlbumRepositoryTests
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
            Assert.Throws<ArgumentNullException>(() => new AlbumRepository(null));
        }

        [Test]
        public void GetAll_Returns_NoData()
        {
            // Arrange
            _mockClient.Setup(a => a.GetAlbums()).Returns(Task.FromResult((IReadOnlyCollection<AlbumData>) new List<AlbumData>()));                        
            var sut = new AlbumRepository(_mockClient.Object);

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
            var albumData = new AlbumData
            {
                UserId = 1,
                Id = 1,
                Title = "saepe unde necessitatibus rem"
            };

            IReadOnlyCollection<AlbumData> data = new List<AlbumData> { albumData };
            _mockClient.Setup(a => a.GetAlbums()).Returns(Task.FromResult(data));
            var sut = new AlbumRepository(_mockClient.Object);

            // Act 
            var result = sut.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            CollectionAssert.AreEquivalent(data, result);
        }

        [Test]
        public void GetByUser_UserIdDoesNotMatch_NoData()
        {
            // Arrange
            int userId = 2;
            var albumData = new AlbumData
            {
                UserId = 1,
                Id = 1,
                Title = "saepe unde necessitatibus rem"
            };

            IReadOnlyCollection<AlbumData> data = new List<AlbumData> { albumData };
            _mockClient.Setup(a => a.GetAlbums()).Returns(Task.FromResult(data));
            var sut = new AlbumRepository(_mockClient.Object);

            // Act 
            var result = sut.GetByUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetByUser_UserIdMatch_Data()
        {
            // Arrange
            int userId = 1;
            var albumData = new AlbumData
            {
                UserId = userId,
                Id = 1,
                Title = "saepe unde necessitatibus rem"
            };

            IReadOnlyCollection<AlbumData> data = new List<AlbumData> { albumData };
            _mockClient.Setup(a => a.GetAlbums()).Returns(Task.FromResult(data));
            var sut = new AlbumRepository(_mockClient.Object);

            // Act 
            var result = sut.GetByUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            CollectionAssert.AreEquivalent(data, result);
        }
    }
}
