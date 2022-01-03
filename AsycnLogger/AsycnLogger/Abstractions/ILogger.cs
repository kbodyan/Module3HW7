using System;

namespace AsyncLogger
{
    public interface ILogger
    {
        event Action StartBackup;
        IDisposable LoggerStream { get; set; }
        int RecordsForBackup { get; set; }

        void LogInfo(LogType type, string message);
        void Run();
    }
}