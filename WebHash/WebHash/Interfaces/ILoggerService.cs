
namespace WebHash.Interfaces
{
    public interface ILoggerService
    {
        void Error(string message);
        void Information(string message);
        string GetLogsInformation();

    }
}
