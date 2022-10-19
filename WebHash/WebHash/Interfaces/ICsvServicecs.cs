using System.Collections.Generic;
using WebHash.Services;

namespace WebHash.IServices
{
    public interface ICsvService
    {
        public IEnumerable<CsvLine> ImportCsvFile(string fileName);
    }
}
