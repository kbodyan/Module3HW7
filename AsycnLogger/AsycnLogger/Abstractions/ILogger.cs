using System;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public interface ILogger
    {
        event Action StartBackup;
        LoggerConfig LoggerConfig { get; set; }
        IDisposable LoggerStream { get; set; }
        void LogInfo(LogType type, string message);
        Task Run();
    }
}