using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services.Interfaces
{
    public interface IDocumentFiller
    {
        Task<byte[]> FillDocument(int fileId, Dictionary<string, string> tagValues);
    }
}
