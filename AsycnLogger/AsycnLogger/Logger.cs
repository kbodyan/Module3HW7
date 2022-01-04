using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public class Logger : ILogger
    {
        private readonly object _lock = new object();
        private Queue<LoggerMessage> _messages = new Queue<LoggerMessage>();
        private IFileService _fileService;
        public Logger(IFileService fileService)
        {
            _fileService = fileService;
        }

        public event Action<string> StartBackup;
        public LoggerConfig LoggerConfig { get; set; }
        public IDisposable LoggerStream { get; set; }
        public async Task Run()
        {
            LoggerMessage message = null;
            var counter = 0;
            var limit = LoggerConfig.RecordsForBackup;
            while (true)
            {
                lock (_messages)
                {
                    if (_messages.Count != 0)
                    {
                        message = _messages.Dequeue();
                    }
                }

                if (message != null)
                {
                    await InternalLogInfo(message.Type, message.Message);
                    message = null;
                    counter++;
                    if (counter >= limit)
                    {
                        await _fileService.CloseFile(LoggerStream);
                        var content = await _fileService.ReadAllFile(LoggerConfig.LogDirectory, LoggerConfig.LogFileName);
                        await Task.Run(() => StartBackup?.Invoke(content));
                        LoggerStream = _fileService.CreateFile(LoggerConfig.LogDirectory, LoggerConfig.LogFileName);
                        counter = 0;
                    }
                }
            }
        }

        public async Task LogInfo(LogType type, string message)
        {
            await Task.Run(() =>
            {
                var newMessage = new LoggerMessage { Type = type, Message = message };
                lock (_messages)
                {
                    _messages.Enqueue(newMessage);
                }
            });
        }

        private async Task InternalLogInfo(LogType type, string message)
        {
            string report = $"{DateTime.UtcNow.ToString()} : {type.ToString()} : {message}";
            Console.WriteLine(report);
            await _fileService.WriteToFile(LoggerStream, report);
        }
    }
}
