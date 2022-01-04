using System.Threading.Tasks;

namespace AsyncLogger
{
    public interface IBackupService
    {
        BackupConfig BackupConfig { get; set; }
        string BackupDirectory { get; set; }

        Task Backup(string content);
    }
}