using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using WebHash.IServices;

namespace WebHash.Services
{
    public class CsvService : ICsvService
    {
        public IEnumerable<CsvLine> ImportCsvFile(string filePath)
        {
            var readedHashes = ReadFromCsv(filePath);

            return readedHashes;
        }

        private IEnumerable<CsvLine> ReadFromCsv(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    try
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var hashes = csv.GetRecords<CsvLine>();
                            return hashes.Where(x => x.Hash != null).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message); // TODO: dorobic prawidlowego exceptiona. Ale to juz zrobic globalnie te eventy
                    }
                }
            }
            return null;

        }
    }
}
