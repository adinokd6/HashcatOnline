using System.Collections.Generic;

namespace WebHash.Models
{
    public class CsvFile
    {
        public string FileName { get; set; }

        public IEnumerable<string> Hashes { get; set; }

    }
}
