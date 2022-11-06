using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using WebHash.CustomExceptions;
using WebHash.Interfaces;
using WebHash.IServices;

namespace WebHash.Services
{
    public class CsvService : ICsvService
    {
        private readonly ILoggerService _loggerService;

        public CsvService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public IEnumerable<CsvLine> ImportCsvFile(string filePath)
        {
            var readedHashes = ReadFromCsv(filePath);

            return readedHashes;
        }

        private IEnumerable<CsvLine> ReadFromCsv(string filePath)
        {
            var hashes = new List<CsvLine>();
            var badReaderLines = new List<string>();
            if (!string.IsNullOrEmpty(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    try
                    {
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            BadDataFound = arg => badReaderLines.Add(arg.Context.Parser.RawRecord)
                        };

                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            hashes = csv.GetRecords<CsvLine>().Select(x => x).ToList();
                            return hashes.Count() > 0 ? hashes.Where(x => x.Hash != null).ToList() : null;
                        }
                    }
                    catch (Exception ex)
                    {
                        _loggerService.Error("Some problem with csv file to import. Details: " + ex.Message);
                        throw new CsvErrorException(ex.Message);
                    }
                }
            }
            return null;

        }
    }
}
