using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHash.Models.ViewModels;

namespace WebHash.Interfaces
{
    public interface IFileService
    {
        public Task<bool> ImportFile(IFormFile uploadedFile, string fileName);

        public IEnumerable<FileViewModel> GetFiles();

        public IEnumerable<HashViewModel> GetHashesFromFile(Guid fileId);

        public string GetFullFilePath(IFormFile file);
    }
}