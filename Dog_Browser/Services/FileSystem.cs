using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Services
{
    public interface IFileSystem
    {
        Task AppendAllLinesAsync(string filePath, IEnumerable<string> contents);

        DirectoryInfo CreateDirectory(string path);

        Stream CreateFile(string filePath);

        bool FileExists(string filePath);
    }

    public class FileSystem : IFileSystem
    {
        public Task AppendAllLinesAsync(string filePath, IEnumerable<string> contents)
        {
            return File.AppendAllLinesAsync(filePath, contents);
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public Stream CreateFile(string filePath)
        {
            return File.Create(filePath);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
