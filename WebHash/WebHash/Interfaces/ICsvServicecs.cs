using System.Collections.Generic;
using WebHash.Services;

namespace WebHash.IServices
{
    public interface ICsvService
    {
        public IEnumerable<CsvHash> ImportCsvFile(string fileName);
    }
}
