using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebHash.Interfaces
{
    public interface IFileService
    {
        public Task<bool> ImportFile(IFormFile uploadedFile, string fileName);
    }
}