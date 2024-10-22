using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services.Interfaces
{
    public interface ITagsService
    {
        List<TagModel> FindTags(int documentId);
    }
}
