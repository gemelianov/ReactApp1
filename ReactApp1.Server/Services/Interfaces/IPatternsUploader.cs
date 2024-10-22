using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services.Interfaces
{
    public interface IPatternsUploader
    {
        Task UploadFileAsync([FromForm] PatternUploadModel model);
    }
}
