using System.Threading.Tasks;

namespace AsyncLogger
{
    public interface IBackupService
    {
        string BackupDirectory { get; set; }

        Task Backup(Config config);
    }
}