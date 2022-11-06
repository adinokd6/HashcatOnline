using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using WebHash.Interfaces;

namespace WebHash.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string logsPathName;

        public LoggerService(string logPathName = null)
        {
            logsPathName = logPathName == null ? AppDomain.CurrentDomain.BaseDirectory + "logs.txt" : logPathName;
        }

        public string GetLogsInformation()
        {
            var allLogs = File.ReadAllText(logsPathName);
            return allLogs;
        }

        public void Error(string message)
        {
            using (var log = new LoggerConfiguration()
                .WriteTo.File(logsPathName)
                .CreateLogger())
            {
                log.Error(message);
            }
        }

        public void Information(string message)
        {
            using (var log = new LoggerConfiguration()
                .WriteTo.File(logsPathName)
                .CreateLogger())
            {
                log.Information(message);
            }
        }
    }
}
