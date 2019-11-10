using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RunPathPhotoServer.Model
{
    [ExcludeFromCodeCoverageAttribute]
    public class PhotoAlbum
    {
        public Album Album { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
