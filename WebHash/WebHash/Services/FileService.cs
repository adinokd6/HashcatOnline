using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebHash.DataModels;
using WebHash.Interfaces;
using WebHash.IServices;
using WebHash.Models.ViewModels;
using static WebHash.Models.Enums.Enums;

namespace WebHash.Services
{
    public class FileService : IFileService
    {
        public ICsvService _csvService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWebHostEnvironment _environment;
        private readonly ILoggerService _loggerService;
        private readonly string _rootPath = "wwwroot\\tmp";

        public FileService(ICsvService csvService, IServiceScopeFactory scopeFactory, IWebHostEnvironment environment, ILoggerService logger)
        {
            _csvService = csvService;
            _scopeFactory = scopeFactory;
            _environment = environment;
            _loggerService = logger;
        }

        public async Task<bool> ImportFile(IFormFile uploadedFile, string fileName)
        {
            DeleteAllTemporaryFiles();
            var filePath = await GetFilePath(uploadedFile);

            if (System.IO.File.Exists(filePath))
            {
                var newFile = new WebHash.DataModels.File()
                {
                    Name = fileName,
                    Date = DateTime.Now,
                };


                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Context>();

                    if (db.Files.Any(x => x.Name == fileName))
                    {
                        _loggerService.Information("Attempt to import file without providing File name");
                        return false;
                    }

                    var readedHashes = _csvService.ImportCsvFile(filePath);

                    var listOfHashes = GetListOfHashes(readedHashes);
                    if (!listOfHashes.Any())
                    {
                        _loggerService.Information("Attempt to import empty file. File name: " + fileName);
                        return false;
                    }

                    newFile.Hashes = listOfHashes;
                    db.Add(newFile);
                    db.SaveChanges();
                    _loggerService.Information("New file has been added. File name: " + fileName);
                    return true;
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

                foreach (var file in files.Where(x => x.Hashes.Count > 0))
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

                if(file == null || file.Hashes.Count == 0)
                {
                    return null;
                }

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

        public string GetFullFilePath(IFormFile file)
        {
            if (file != null)
            {
                DeleteAllTemporaryFiles();
                var getFilePathTask = GetFilePath(file);
                getFilePathTask.Wait();
                var filePath = getFilePathTask.Result;
                filePath.Replace(@"\\", @"\");
                return "\"" + filePath + "\"";
            }
            return String.Empty;
        }

        public IEnumerable<FileViewModel> GetAllFiles()
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

        public bool IsFileExists(Guid id)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var files = db.Files;
                var fileTodelete = files.Where(x => x.Id == id).FirstOrDefault();
                if (fileTodelete != null)
                    return true;
                return false;
            }
        }

        public void DeleteFile(Guid id)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var files = db.Files;
                var fileTodelete = files.Where(x => x.Id == id).Include(y => y.Hashes).FirstOrDefault();

                if (fileTodelete != null)
                {
                    db.Files.Remove(fileTodelete);
                }

                db.SaveChanges();
            }
        }

        private async Task<string> GetFilePath(IFormFile file)
        {
            var filePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot\\tmp", file.FileName);
            using var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            await file.CopyToAsync(fileStream);

            return filePath;
        }

        private void DeleteAllTemporaryFiles()
        {
            string[] files = Directory.GetFiles(_rootPath);
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
        }

        private List<Hash> GetListOfHashes(IEnumerable<CsvHash> readedHashes)
        {
            if (readedHashes == null)
            {
                return null;
            }

            var hashes = new List<Hash>();
            foreach (var hash in readedHashes)
            {
                HashType hashType;
                if (Enum.TryParse(hash.HashType, out hashType))
                {
                    hashes.Add(new Hash()
                    {
                        OriginalString = hash.Hash,
                        HashType = hashType,
                        Name = hash.HashName
                    });
                }
            }

            return hashes;
        }
    }
}
