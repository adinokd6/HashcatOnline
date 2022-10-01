using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using WebHash.IServices;

namespace WebHash.Services
{
    public class CsvService : ICsvService
    {
        #region Attributes
        private List<string> hash = new List<string>();
        #endregion




        public IEnumerable<string> ImportCsvFile(string fileName)
        {
            var elo = ReadFromCsv(fileName);

            return elo;
        }

        public IEnumerable<string> ReadFromCsv(string fileName)
        {
            if(!string.IsNullOrEmpty(fileName))
            {
                using (var reader = new StreamReader(fileName))
                {
                    try
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var elo = csv.GetRecords<CsvLine>();
                            var elo2 = elo.Where(x => x.Hash != null).Select(x => x.Hash).ToList();
                            return elo2;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return null;
            
        }
    }
}
