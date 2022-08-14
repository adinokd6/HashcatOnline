using System;

namespace MainApp.Interfaces
{
    public interface IStartProgramService
    {
        public Tuple<string, string> StartDecryptionProcess(string input, string hashCode);
    }
}
