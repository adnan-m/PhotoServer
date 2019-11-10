using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using RunPathPhotoServer.Client;

namespace RunPathPhotoServer.Test.Client
{
    public class RunpathClientTests
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private Mock<IRunpathClient> _mockClient;

        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRunpathClient>();

            var inMemorySettings = new Dictionary<string, string>
            {                
                { "AppSettings:ApiPhoto" , "http://jsonplaceholder.typicode.com/photos"},
                { "AppSettings:ApiAlbum", "http://jsonplaceholder.typicode.com/albums"}                
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();            
        }

        [Test]
        public void Initiate_WithoutHttpClient_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RunpathClient(null, _configuration));
        }

        [Test]
        public void Initiate_WithoutConfiguration_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RunpathClient(_httpClient, null));
        }

        [Test]
        public void GetAlbums_Returns_NoData()
        {
            // Arrange                       
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent($"[]"),
               })
               .Verifiable();

            _httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var sut = new RunpathClient(_httpClient, _configuration);

            // Act 
            var result = sut.GetAlbums();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Result.Count);
        }

        [Test]
        public void GetAlbums_Returns_Data()
        {
            // Arrange            
            int userId = 1;
            int Id = 6;
            string title = "natus impedit quibusdam illo est";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent($"[{{'userId': {userId}, 'id': {Id}, 'title': '{title}'}}]"),
               })
               .Verifiable();

            _httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var sut = new RunpathClient(_httpClient, _configuration);

            // Act 
            var result = sut.GetAlbums();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Result.Count);
            Assert.AreEqual(Id, result.Result.First().Id);
            Assert.AreEqual(title, result.Result.First().Title);
            Assert.AreEqual(userId, result.Result.First().UserId);
        }

        [Test]
        public void GetPhotos_Returns_NoData()
        {
            // Arrange                                    
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent($"[]"),
               })
               .Verifiable();

            _httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var sut = new RunpathClient(_httpClient, _configuration);

            // Act 
            var result = sut.GetPhotos();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Result.Count);            
        }

        [Test]
        public void GetPhotos_Returns_Data()
        {
            // Arrange                        
            int Id = 6;
            int albumId = 2;
            string url = "https://via.placeholder.com/600/9e59da";
            string thumnailUrl = "https://via.placeholder.com/150/9e59da";
            string title = "natus impedit quibusdam illo est";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent($"[{{'albumId': {albumId}, 'id': {Id},  'title': '{title}',  'url': '{url}', 'thumbnailUrl': '{thumnailUrl}' }}]"),
               })
               .Verifiable();            

            _httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var sut = new RunpathClient(_httpClient, _configuration);

            // Act 
            var result = sut.GetPhotos();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Result.Count);
            Assert.AreEqual(Id, result.Result.First().Id);
            Assert.AreEqual(title, result.Result.First().Title);
            Assert.AreEqual(url, result.Result.First().Url.ToString());
            Assert.AreEqual(thumnailUrl, result.Result.First().ThumbnailUrl.ToString());
        }
    }
}
