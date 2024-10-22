using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Services.Interfaces;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatternsUploadController : ControllerBase
    {
        IPatternsUploader _patternsUploader;

        public PatternsUploadController (IPatternsUploader patternsUploader)
        {
            _patternsUploader = patternsUploader;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] PatternUploadModel model)
        {
            // Получение имени пользователя из токена
            var login = model.Login;

            // Проверка наличия файла
            if (model.File==null)
            {
                return BadRequest("Файл не найден.");
            }

            // Получение файла
            var file = model.File;

            // Проверка типа файла (Word или Excel)
            if (!file.ContentType.StartsWith("application/vnd.openxmlformats-officedocument.wordprocessingml."))
            {
                return BadRequest("Неверный тип файла. Допускается только файл Word.");
            }

            // Загрузка файла через сервис
            await _patternsUploader.UploadFileAsync(model);

            // Возвращение ответа
            return Ok("Файл успешно загружен.");
        }
    }
}
