using System;

namespace WebHash.CustomExceptions
{
    public class BadPotFilePathException : Exception
    {
        public BadPotFilePathException(string message)
        : base(message)
                {
                }
    }
}
