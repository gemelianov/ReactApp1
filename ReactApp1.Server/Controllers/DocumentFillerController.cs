using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class DocumentFillerController : ControllerBase
    {
        private readonly IDocumentFiller _documentFiller;

        public DocumentFillerController(IDocumentFiller documentFiller)
        {
            _documentFiller = documentFiller;
        }

        [HttpPost("{fileId}/fill")]
        public async Task<IActionResult> FillDocument(int fileId, [FromBody] Dictionary<string, string> tagValues)
        {
            try
            {
                byte[] filledDocument = await _documentFiller.FillDocument(fileId, tagValues);
                return File(filledDocument, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{fileId}_filled.docx");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
