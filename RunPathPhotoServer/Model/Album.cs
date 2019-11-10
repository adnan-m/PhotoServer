using System.Diagnostics.CodeAnalysis;

namespace RunPathPhotoServer.Model
{
    [ExcludeFromCodeCoverageAttribute]
    public class Album
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
    }
}
