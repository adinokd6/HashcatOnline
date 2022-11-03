using WebHash.IServices;
using WebHash.Models.ViewModels;
using WebHash.Interfaces;
using System;
using static WebHash.Models.Enums.Enums;
using Microsoft.Extensions.DependencyInjection;
using WebHash.DataModels;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using WebHash.Models.ViewModels.Analyse;
using System.Collections.Generic;

namespace WebHash.Services
{
    public class HashService : IHashService
    {
        private readonly IStartProgramService _startProgram;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IFileService _fileService;
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

        public HashService(IStartProgramService startProgramService, IServiceScopeFactory scopeFactory, IFileService fileService)
        {
            _startProgram = startProgramService;
            _scopeFactory = scopeFactory;
            _fileService = fileService;
        }

        public void Decode(CrackHashViewModel hash)
        {
            var hashCode = string.Empty;
            if (!hash.HashFromInput && hash.HashFromFileId != Guid.Empty)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Context>();
                    hashCode = db.Hashes.Where(x => x.Id == hash.HashFromFileId).Select(x => x.OriginalString).FirstOrDefault();
                }
            }
            else
            {
                hashCode = !string.IsNullOrEmpty(hash.InputValue) ? hash.InputValue : "";
            }



            if (!string.IsNullOrEmpty(hashCode))
            {
                Stopwatch stopwatch = new Stopwatch();

                var command = GetCommandForHashCat(hash.AttackMethod, hash.HashType, hashCode, hash.Dictionary1, hash.Dictionary2);

                stopwatch.Start();
                hash.OutputValue = _startProgram.StartDecryptionProcess(command, hashCode); //Zmiana jezeli na wejsciu bedzie slownik
                stopwatch.Stop();

                if(hash.OutputValue.Item1 != null)
                {
                    var outputValue = hash.OutputValue.Item1.Remove(0, 1); //remo /r becouse it is ouptu with new line from commandline
                    if (!hash.HashFromInput && outputValue.Equals(hashCode))
                    {
                        SaveResultsForHash(hash.HashFromFileId, hash.OutputValue.Item2, hash.HashType, stopwatch.Elapsed.TotalSeconds);
                    }
                }
            }

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
                foreach(var hash in groupedHash)
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

        private double CountAverageDehashTime(IGrouping<HashType,Hash> hashGroup)
        {
            var crackingTimeList = hashGroup.Select(x => x.CrackingTime);

            return Queryable.Average(crackingTimeList.AsQueryable());
        }

        private string GetCommandForHashCat(AttackMethod attackMethod, HashType hashType, string inputValue, IFormFile dictionary1, IFormFile dictionary2)
        {
            var dict1Path = _fileService.GetFullFilePath(dictionary1);
            var dict2Path = _fileService.GetFullFilePath(dictionary2);
            string commandForHashCat = string.Empty;
            switch (attackMethod)
            {
                case AttackMethod.Combination:
                    return commandForHashCat = GetAttackMode(attackMethod) + " " + GetHashType(hashType) + " " + inputValue + " " + dict1Path + " " + dict2Path;
                case AttackMethod.Straight:
                    return commandForHashCat = GetAttackMode(attackMethod) + " " + GetHashType(hashType) + " " + inputValue + " " + dict1Path;
                default:
                    return commandForHashCat = GetAttackMode(attackMethod) + " " + GetHashType(hashType) + " " + inputValue;

            }
        }

        private void SaveResultsForHash(Guid hashId, string result, HashType hashType, double time)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var hashToUpdate = db.Hashes.FirstOrDefault(x => x.Id == hashId);
                if(hashToUpdate != null)
                {
                    hashToUpdate.Result = result; // TODO: wywalic karotki z resulta od stringa z cmd
                    hashToUpdate.CrackingTime = Convert.ToInt32(time);
                    hashToUpdate.HashType = hashType;
                }
                db.SaveChanges();
            }
        }

        private string GetAttackMode(AttackMethod attackType)
        {

            return "-a " + ((int)attackType).ToString();
        }

        private string GetHashType(HashType hashType)
        {
            return "-m " + ((int)hashType).ToString();
        }
    }


}
