using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services.Interfaces
{
    public interface IUsersService
    {
        UserModel Create(UserModel model);

        UserModel Update(UserModel model, string newPass);

        UserModel GetByLogin(string login);

        void DeleteByLogin(string login);
    }
}
