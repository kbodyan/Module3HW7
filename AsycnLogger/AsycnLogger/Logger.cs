using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public class Logger : ILogger
    {
        private readonly object _lock = new object();
        private LoggerMessage _massage;
        private IFileService _fileService;
        public Logger(IFileService fileService)
        {
            _fileService = fileService;
        }

        public event Action StartBackup;
        public int RecordsForBackup { get; set; }
        public IDisposable LoggerStream { get; set; }
        public void Run()
        {
            LoggerMessage massage = null;
            var counter = 0;
            var limit = RecordsForBackup;
            while (true)
            {
                lock (_lock)
                {
                    if (_massage != null)
                    {
                        massage = _massage;
                        _massage = null;
                    }
                }

                if (massage != null)
                {
                    Task.Run(() => InternalLogInfo(_massage.Type, _massage.Message));
                    massage = null;
                    counter++;
                    if (counter >= limit)
                    {
                        counter = 0;
                        Task.Run(() => StartBackup?.Invoke());
                    }
                }
            }
        }

        public void LogInfo(LogType type, string message)
        {
            var newMessage = new LoggerMessage { Type = type, Message = message };
            Interlocked.Exchange<LoggerMessage>(ref _massage, newMessage);
        }

        private void InternalLogInfo(LogType type, string message)
        {
            string report = $"{DateTime.UtcNow.ToString()} : {type.ToString()} : {message}";
            Console.WriteLine(report);
            _fileService.WriteToFile(LoggerStream, report);
        }
    }
}
