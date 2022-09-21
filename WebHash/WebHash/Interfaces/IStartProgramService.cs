using System;

namespace WebHash.Interfaces
{
    public interface IStartProgramService
    {
        public Tuple<string, string> StartDecryptionProcess(string input, string hashCode);
    }
}
