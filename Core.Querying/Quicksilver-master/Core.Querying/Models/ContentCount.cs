using EPiServer.Core;

namespace Core.Querying.Models
{
    public class ContentCount
    {
        public readonly string Name;
        public readonly ContentReference ContentLink;
        public readonly int Count;

        public ContentCount(IContent content, int count)
        {
            Name = content.Name;
            ContentLink = content.ContentLink;
            Count = count;
        }
    }
}