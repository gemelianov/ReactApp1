using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services.Interfaces
{
    public interface IPatternsService
    {
        DocumentModel Create(DocumentModel model);

        DocumentModel GetById(int id);

        List<DocumentModel> GetAll(string login);

        void DeleteById(int id);
    }
}
