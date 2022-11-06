using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHash.Models.ViewModels;

namespace WebHash.Interfaces
{
    public interface IFileService
    {
        Task<bool> ImportFile(IFormFile uploadedFile, string fileName);

        IEnumerable<FileViewModel> GetFiles();
        IEnumerable<FileViewModel> GetAllFiles();
        bool IsFileExists(Guid id);

        IEnumerable<HashViewModel> GetHashesFromFile(Guid fileId);

        string GetFullFilePath(IFormFile file);

        void DeleteFile(Guid id);
    }
}