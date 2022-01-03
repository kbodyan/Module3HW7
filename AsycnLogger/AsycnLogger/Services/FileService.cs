﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLogger
{
    public class FileService : IFileService
    {
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        public IDisposable CreateFile(string dirPath, string fileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            var newFileName = $"{dirPath}/{fileName}";
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            return new StreamWriter(newFileName, true);
        }

        public async Task<string> ReadAllFile(string dirPath, string fileName)
        {
            var newFileName = $"{dirPath}/{fileName}";
            var result = string.Empty;
            using (var stream = new StreamReader(newFileName))
            {
                result = await stream.ReadToEndAsync();
            }

            return result;
        }

        public async Task WriteToFile(IDisposable stream, string text)
        {
            await _semaphoreSlim.WaitAsync();

            await ((StreamWriter)stream).WriteLineAsync(text);
            await ((StreamWriter)stream).FlushAsync();

            _semaphoreSlim.Release();
        }

        public void CloseFile(IDisposable stream)
        {
            ((StreamWriter)stream).Close();
        }
    }
}
