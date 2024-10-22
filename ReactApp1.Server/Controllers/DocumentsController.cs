using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {

        private IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpPost]
        public DocumentModel Create(DocumentModel model)
        {
            return _documentsService.Create(model);
        }

        [HttpGet("(id)")]
        public DocumentModel GetById(int id)
        {
            return _documentsService.GetById(id);
        }

        [HttpGet("(login)")]
        public List<DocumentModel> GetAll(string login)
        {
            return _documentsService.GetAll(login);
        }

        [HttpDelete("(id)")]
        public void DeleteById(int id)
        {
            _documentsService.DeleteById(id);
        }
    }
}
