using System;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public interface IFileService
    {
        public Task CloseFile(IDisposable stream);
        IDisposable CreateFile(string dirPath, string fileName);
        Task<string> ReadAllFile(string dirPath, string fileName);
        Task WriteToFile(IDisposable stream, string text);
    }
}