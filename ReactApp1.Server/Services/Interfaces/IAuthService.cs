using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services.Interfaces
{
    public interface IAuthService
    {
        UserModel Login(UserModel model);
    }
}
