using System;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public interface ILogger
    {
        event Action<string> StartBackup;
        LoggerConfig LoggerConfig { get; set; }
        IDisposable LoggerStream { get; set; }
        Task LogInfo(LogType type, string message);
        Task Run();
    }
}