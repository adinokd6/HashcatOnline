using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHash.DataModels;
using WebHash.Interfaces;
using WebHash.IServices;
using WebHash.Models.ViewModels;

namespace WebHash.Services
{
    public class FileService : IFileService
    {
        public ICsvService _csvService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostingEnvironment _environment;
        public FileService(ICsvService csvService, IServiceScopeFactory scopeFactory, IHostingEnvironment environment)
        {
            _csvService = csvService;
            _scopeFactory = scopeFactory;
            _environment = environment;
        }

        public async Task<bool> ImportFile(IFormFile uploadedFile, string fileName)
        {

            var filePath = await GetFilePath(uploadedFile);

            if (System.IO.File.Exists(filePath))
            {
                var newFile = new File()
                {
                    Name = fileName,
                    Date = DateTime.Now,
                    IsDeleted = false
                };


                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Context>();

                    if (db.Files.Any(x => x.Name == fileName))
                    {
                        return false;
                    }

                    var readedHashes = _csvService.ImportCsvFile(filePath);

                    var listOfHashes = GetListOfHashes(readedHashes);
                    if(!listOfHashes.Any())
                    {
                        return false;
                    }

                    newFile.Hashes = listOfHashes;
                    db.Add(newFile);
                    db.SaveChanges();
                }



            }
            return false;
        }

        public IEnumerable<FileViewModel> GetFiles()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var files = db.Files;

                var filesList = new List<FileViewModel>();

                foreach (var file in files)
                {
                    filesList.Add(new FileViewModel()
                    {
                        Id = file.Id,
                        Name = file.Name,
                    });
                }
                return filesList;
            }
        }

        public IEnumerable<HashViewModel> GetHashesFromFile(Guid fileId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var file = db.Files.Include(x => x.Hashes).Where(x => x.Id == fileId).FirstOrDefault();
                    

                var hashesList = new List<HashViewModel>();

                foreach (var hash in file.Hashes.Where(x => string.IsNullOrEmpty(x.Result)))
                {
                    hashesList.Add(new HashViewModel()
                    {
                        Id = hash.Id,
                        Hash = hash.OriginalString,
                    });
                }
                return hashesList;
            }
        }

        private async Task<string> GetFilePath(IFormFile file)
        {
            var filePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot\\tmp", file.FileName);
            using var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            await file.CopyToAsync(fileStream);

            return filePath;
        }

        private List<Hash> GetListOfHashes(IEnumerable<string> readedHashes)
        {
            if (readedHashes == null)
            {
                return null;
            }

            var hashes = new List<Hash>();
            foreach (var hash in readedHashes)
            {
                hashes.Add(new Hash()
                {
                    OriginalString = hash,
                    IsDeleted = false
                });
            }

            return hashes;
        }
    }
}
