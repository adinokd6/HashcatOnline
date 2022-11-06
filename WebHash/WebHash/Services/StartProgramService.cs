using WebHash.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WebHash.CustomExceptions;

namespace WebHash.Services
{
    public class StartProgramService : IStartProgramService
    {
        private readonly ILoggerService _loggerService;
        private readonly string _workingDirectory;
        private readonly string _hashcatLocalization;
        private readonly string _filename;
        private readonly string _potfileLocalization;

        public StartProgramService(ILoggerService loggerService, string workingDirectory = null, string hashCatLocalization = null, string exeName = null, string potFileLocalization=null)
        {
            _workingDirectory = workingDirectory == null ? Environment.CurrentDirectory : workingDirectory;
            _hashcatLocalization = hashCatLocalization == null ? _workingDirectory + "\\bin\\hsc\\" : hashCatLocalization;
            _filename = exeName == null ? Path.Combine(_hashcatLocalization, "hashcat.exe") : exeName;
            _potfileLocalization = potFileLocalization == null ? "./bin/hsc/hashcat.potfile" : potFileLocalization;
            _loggerService = loggerService;
        }

        public Tuple<string, string> StartDecryptionProcess(string input, string hashCode)
        {
            CheckPotfileAndDelete();
            killProcess();
            var hashCatProcess = InitializeHashCatProcess(input);
            var decryptedInfo = DecryptHash(hashCatProcess, hashCode);

            hashCatProcess.Close();

            killProcess();

            //After every run (and before too) of hashcat delete hashcat.potfile to enable hashcat to decrypt the same hashes
            CheckPotfileAndDelete();
            return decryptedInfo;
        }

        private Process InitializeHashCatProcess(string cmdInput)
        {
            var proc = new ProcessStartInfo();
            proc.WorkingDirectory = _hashcatLocalization;
            proc.FileName = _filename;
            proc.Arguments = cmdInput;
            proc.RedirectStandardOutput = true;

            Process hashCat = new Process();
            hashCat.StartInfo = proc;

            try
            {
                hashCat.Start();
            }
            catch (Exception ex)
            {
                _loggerService.Error("Problem to start hashcat.exe. Details: " + ex.Message);
                throw new HashCatProblemException(ex.Message);
            }


            return hashCat;
        }

        private Tuple<string, string> DecryptHash(Process hashCatProcess, string hashCode)
        {
            StringBuilder consoleOutput = new StringBuilder();

            while (!hashCatProcess.HasExited)
            {
                consoleOutput.Append(hashCatProcess.StandardOutput.ReadToEnd());
            }

            var decryptedInfo = GetDecryptedHash(consoleOutput, hashCode);

            return decryptedInfo;

        }

        private Tuple<string, string> GetDecryptedHash(StringBuilder output, string hashCode)
        {
            string[] OutputFromHashCat = output.ToString().Split(' ').ToArray();

            string HashAndDecrypted = OutputFromHashCat.Where(x => x.Contains(hashCode + ":")).FirstOrDefault();
            if (HashAndDecrypted != null)
            {
                OutputFromHashCat = HashAndDecrypted.Split(":");
                _loggerService.Information("Successfully dehash. Details:" + OutputFromHashCat[0] + " " + OutputFromHashCat[1]);
                return Tuple.Create(OutputFromHashCat[0], OutputFromHashCat[1]);
            }
            else
            {
                bool isTokenError = output.ToString().Contains("Token length exception");
                if (isTokenError.Equals(true))
                {
                    _loggerService.Error("The hash you want to cracked is not the type you choose");
                    return Tuple.Create(string.Empty, "The hash you want to cracked is not the type you choose");
                }
                else
                {
                    _loggerService.Error("Some error occured.");
                    return Tuple.Create(string.Empty, "Some error occured");
                }

            }
        }

        private void killProcess()
        {
            var hashcatProcesses = Process.GetProcesses().Where(process => process.ProcessName == "hashcat");

            foreach (var process in hashcatProcesses)
            {
                if (process.ProcessName == "hashcat")
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void CheckPotfileAndDelete()
        {
            if (File.Exists(_potfileLocalization))
            {
                try
                {
                    File.Delete(_potfileLocalization);
                }
                catch (Exception ex)
                {
                    _loggerService.Error("Error with potfile path or configuration. Details: " + ex.Message);
                    throw new BadPotFilePathException(ex.Message);
                }

            }
        }
    }
}
