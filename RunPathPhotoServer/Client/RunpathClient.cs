using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RunPathPhotoServer.Repository.Data;

namespace RunPathPhotoServer.Client
{
    public class RunpathClient : IRunpathClient
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;

        public RunpathClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); ;
        }

        public async Task<IReadOnlyCollection<AlbumData>> GetAlbums() 
        {            
            var uri = _configuration.GetSection("AppSettings").GetValue<string>("ApiAlbum");            
            var response = await _httpClient.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<IReadOnlyCollection<AlbumData>>(response);            
        }

        public async Task<IReadOnlyCollection<PhotoData>> GetPhotos()
        {
            var uri = _configuration.GetSection("AppSettings").GetValue<string>("ApiPhoto");
            var response = await _httpClient.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<IReadOnlyCollection<PhotoData>>(response);            
        }
    }
}
