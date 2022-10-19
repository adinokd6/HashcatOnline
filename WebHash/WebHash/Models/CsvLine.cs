using CsvHelper.Configuration.Attributes;

namespace WebHash.Services
{
    public class CsvLine
    {
        [Index(0)]
        public string Hash { get; set; }
        [Index(1)]
        public string HashType { get; set; }
    }
}
