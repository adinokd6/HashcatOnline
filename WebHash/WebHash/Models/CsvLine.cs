using CsvHelper.Configuration.Attributes;

namespace WebHash.Services
{
    public class CsvLine
    {
        public string Hash { get; set; }
        public string HashType { get; set; }
        public string HashName { get; set; }
    }
}
