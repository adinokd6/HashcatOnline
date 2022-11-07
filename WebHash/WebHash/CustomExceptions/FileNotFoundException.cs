using System;

namespace WebHash.CustomExceptions
{
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message)
                : base(message)
        {
        }
    }
}
