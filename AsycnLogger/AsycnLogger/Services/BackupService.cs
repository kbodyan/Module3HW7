using System;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public class BackupService : IBackupService
    {
        private IFileService _fileService;

        public BackupService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string BackupDirectory { get; set; }
        public async Task Backup(Config config)
        {
            var time = DateTime.UtcNow;
            var filename = $"{config.Backup.BackupDirectory}/{time.Hour}.{time.Minute}.{time.Second} {time.ToShortDateString()}.txt";
            var content = await _fileService.ReadAllFile(config.Logger.LogDirectory, config.Logger.LogFileName);
            var backupFile = _fileService.CreateFile(config.Backup.BackupDirectory, filename);
            await _fileService.WriteToFile(backupFile, content);
        }
    }
}
