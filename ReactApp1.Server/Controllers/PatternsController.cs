using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatternsController : ControllerBase
    {
        private IPatternsService _patternsService;

        public PatternsController(IPatternsService patternsService)
        {
            _patternsService = patternsService;
        }

        [HttpPost]
        public DocumentModel Create(DocumentModel model)
        {
            return _patternsService.Create(model);
        }

        [HttpGet("(id)")]
        public DocumentModel GetById(int id)
        {
            return _patternsService.GetById(id);
        }

        [HttpGet("(login)")]
        public List<DocumentModel> GetAll(string login)
        {
            return _patternsService.GetAll(login);
        }

        [HttpDelete("(id)")]
        public void DeleteById(int id)
        {
            _patternsService.DeleteById(id);
        }
    }
}
