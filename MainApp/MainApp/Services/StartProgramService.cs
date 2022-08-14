using MainApp.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MainApp.Services
{
    public class StartProgramService : IStartProgramService
    {
        private readonly string _workingDirectory;
        private readonly string _hashcatLocalization;
        private readonly string _filename;
        private readonly string _potfileLocalization;

        public StartProgramService()
        {
            _workingDirectory = Environment.CurrentDirectory;
            _hashcatLocalization = _workingDirectory + "\\bin\\hsc\\";
            _filename = Path.Combine(_hashcatLocalization, "hashcat.exe");
            _potfileLocalization = "./bin/hsc/hashcat.potfile";
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
                var exception = ex;
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

            string HashAndDecrypted = OutputFromHashCat.FirstOrDefault(x => x.Contains(hashCode + ":"));
            if (HashAndDecrypted != null)
            {
                OutputFromHashCat = HashAndDecrypted.Split(":");

                return Tuple.Create(OutputFromHashCat[0], OutputFromHashCat[1]);
            }
            else
            {
                bool isTokenError = output.ToString().Contains("Token length exception");
                if (isTokenError.Equals(true))
                {
                    return Tuple.Create(string.Empty, "The hash you want to cracked is not the type you choose");
                }
                else
                {
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
                    var exception = ex;
                }

            }
        }
    }
}
