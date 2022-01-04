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

        public BackupConfig BackupConfig { get; set; }
        public string BackupDirectory { get; set; }
        public async Task Backup(string content)
        {
            var time = DateTime.UtcNow;
            var filename = $"{BackupConfig.BackupDirectory}/{time.Hour}.{time.Minute}.{time.Second} {time.ToShortDateString()}.txt";
            var backupFile = _fileService.CreateFile(BackupConfig.BackupDirectory, filename);
            await _fileService.WriteToFile(backupFile, content);
            await _fileService.CloseFile(backupFile);
            await Task.Delay(1000);
        }
    }
}
