using System;

namespace WebHash.CustomExceptions
{
    public class CsvErrorException : Exception
    {
        public CsvErrorException(string message)
    : base(message)
        {
        }
    }
}
