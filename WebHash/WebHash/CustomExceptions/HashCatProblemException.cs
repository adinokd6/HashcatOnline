using System;

namespace WebHash.CustomExceptions
{
    public class HashCatProblemException : Exception
    {
        public HashCatProblemException(string message)
    : base(message)
        {
        }
    }
}
