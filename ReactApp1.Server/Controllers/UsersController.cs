using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public UserModel Create(UserModel model)
        {
            return _usersService.Create(model);
        }

        [HttpPatch("(newPassword)")]
        public UserModel Update(UserModel model, string newPass)
        {
            return _usersService.Update(model, newPass);
        }
        
        [HttpGet("(login)")]
        public UserModel GetByLogin(string login)
        {
            return _usersService.GetByLogin(login);
        }

        [HttpDelete("(login)")]
        public void DeleteByLogin(string login)
        {
            _usersService.DeleteByLogin(login);
        }
    }
}
