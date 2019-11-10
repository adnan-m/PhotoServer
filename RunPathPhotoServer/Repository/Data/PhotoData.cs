using System;
using System.Diagnostics.CodeAnalysis;

namespace RunPathPhotoServer.Repository.Data
{
    [ExcludeFromCodeCoverageAttribute]
    public class PhotoData
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public Uri ThumbnailUrl { get; set; }        
    }
}
