using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using WebHash.CustomExceptions;
using WebHash.DataModels;
using WebHash.Interfaces;
using WebHash.Models.ViewModels;
using WebHash.Models.ViewModels.Analyse;
using static WebHash.Models.Enums.Enums;

namespace WebHash.Services
{
    public class AnalyseService : IAnalyseService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILoggerService _loggerService;

        private readonly List<string> _backgroundColors = new List<string>()
        {
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)",
                "rgba(255, 159, 64, 0.2)"
        };

        private readonly List<string> _borderColors = new List<string>()
        {
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 159, 64, 1)"
        };

        public AnalyseService(IServiceScopeFactory serviceScopeFactory, ILoggerService loggerService)
        {
            _scopeFactory = serviceScopeFactory;
            _loggerService = loggerService;
        }


        public AnalyseViewModel GetAnalyseData()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var model = new AnalyseViewModel()
                {
                    HashTypes = new List<string>(),
                    HashNumbers = new List<int>(),
                    AverageDehashTime = new List<double>(),
                    BackgroundColor = new List<string>(),
                    BorderColor = new List<string>(),
                };

                var db = scope.ServiceProvider.GetRequiredService<Context>();

                var groupedHash = db.Hashes.AsEnumerable().GroupBy(x => x.HashType).OrderByDescending(g => g.Count()).ToList().Take(4);

                int i = 0;
                foreach (var hash in groupedHash)
                {
                    model.HashTypes.Add(hash.Key.ToString());
                    model.HashNumbers.Add(hash.Count());
                    model.AverageDehashTime.Add(CountAverageDehashTime(hash));
                    model.BackgroundColor.Add(_backgroundColors.ElementAt(i));
                    model.BorderColor.Add(_borderColors.ElementAt(i));
                    i++;
                }

                return model;
            }
        }

        public IEnumerable<FileViewModel> GetAllFiles()
        {
            var result = new List<FileViewModel>();
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                foreach(var file in db.Files)
                {
                    result.Add(new FileViewModel()
                    {
                        Id = file.Id,
                        Name = file.Name
                    });
                }

                
            }
            return result;
        }

        public FileDetailsViewModel GetDetailsForFile(Guid id)
        {
            if (id != Guid.Empty)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Context>();
                    var file = db.Files.Where(x => x.Id == id).Include(y => y.Hashes).FirstOrDefault();

                    if (file != null)
                    {
                        var typesOfHashesInFile = GetHashesTypesInfoForFile(file.Hashes.AsEnumerable());
                        var hashesDetails = GetHashesDetailsForFile(file.Hashes.AsEnumerable());

                        var fileDetails = new FileDetailsViewModel()
                        {
                            Name = file.Name,
                            HashesCount = file.Hashes.Count(),
                            SumOfCrackingTime = file.Hashes.Any() ? file.Hashes.Select(x => x.CrackingTime).Sum() : 0,
                            UnCrackedPasswords = file.Hashes.Any() ? file.Hashes.Where(x => x.Result == null).Count() : 0,
                            CrackedPasswords = file.Hashes.Any() ? file.Hashes.Where(x => x.Result != null).Count() : 0,
                            HashInFileNumberList = typesOfHashesInFile,
                            HashesDetails = hashesDetails
                        };
                        return fileDetails;
                    }
                    else
                    {
                        _loggerService.Error("Request to search for file which wasn't found. Requested File Id: " + id.ToString());
                        throw new FileNotFoundException("Request to search for file which wasn't found. Requested File Id: " + id.ToString());
                    }
                }
            }
            return null;

        }

        private IEnumerable<HashForFileNumberViewModel> GetHashesTypesInfoForFile(IEnumerable<Hash> hashList)
        {
            if (hashList != null)
            {
                var typesOfHashesInFile = new List<HashForFileNumberViewModel>();
                var groupedHash = hashList.GroupBy(x => x.HashType).OrderByDescending(g => g.Count()).ToList().Take(4);

                foreach (var hash in groupedHash)
                {
                    typesOfHashesInFile.Add(new HashForFileNumberViewModel()
                    {
                        HashCount = hash.Count(),
                        HashTypeName = Enum.GetName(typeof(HashType), hash.Select(x => x.HashType).First()),
                    });
                }
                return typesOfHashesInFile;
            }
            return null;

        }

        private IEnumerable<HashInfoViewModel> GetHashesDetailsForFile(IEnumerable<Hash> hashList)
        {
            if (hashList != null)
            {
                var hashDetails = new List<HashInfoViewModel>();

                foreach (var hash in hashList)
                {
                    hashDetails.Add(new HashInfoViewModel()
                    {
                        OriginalString = hash.OriginalString,
                        IsDecrypted = hash.Result != null,
                        Result = hash.Result,
                        CrackingTime = hash.CrackingTime,
                        HashType = Enum.GetName(typeof(HashType), hash.HashType),
                    });
                }
                return hashDetails;
            }
            return null;

        }

        private double CountAverageDehashTime(IGrouping<HashType, Hash> hashGroup)
        {
            var crackingTimeList = hashGroup.Select(x => x.CrackingTime);

            return Queryable.Average(crackingTimeList.AsQueryable());
        }
    }
}
