using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public class Starter
    {
        private ILogger _logger;
        private IFileService _fileService;
        private IConfigService _configService;
        private IBackupService _backupService;
        public Starter(ILogger logger, IFileService fileService, IConfigService configService, IBackupService backupService)
        {
            _logger = logger;
            _fileService = fileService;
            _configService = configService;
            _backupService = backupService;
        }

        public Config Config { get; set; }
        public void BackupCallback()
        {
            _backupService.Backup(Config);
        }

        public void Run()
        {
            Config = _configService.GetConfig();
            _logger.LoggerConfig = Config.Logger;
            _logger.LoggerStream = _fileService.CreateFile(Config.Logger.LogDirectory, Config.Logger.LogFileName);
            _logger.StartBackup += BackupCallback;
            Thread myThread = new Thread(new ThreadStart(() => _logger.Run()));
            myThread.IsBackground = true;
            myThread.Start();

            Action();
            Console.ReadLine();
            _fileService.CloseFile(_logger.LoggerStream);
        }

        public void Action()
        {
            var rand = new Random();
            LogType logType;
            for (int i = 0; i < 50; i++)
            {
                var k = i;
                logType = (LogType)rand.Next(0, 3);
                _logger.LogInfo(logType, $"This is log # {k}");
            }
        }
    }
}
