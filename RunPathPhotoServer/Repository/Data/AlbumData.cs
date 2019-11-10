using System.Diagnostics.CodeAnalysis;

namespace RunPathPhotoServer.Repository.Data
{
    [ExcludeFromCodeCoverageAttribute]
    public class AlbumData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
    }
}
