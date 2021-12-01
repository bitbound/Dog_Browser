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

        bool DirectoryExists(string dirPath);

        IEnumerable<string> EnumerateFiles(string dirPath);

        bool FileExists(string filePath);
        string[] ReadAllLines(string filePath);
        string ReadAllText(string filePath);
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

        public bool DirectoryExists(string dirPath)
        {
            return Directory.Exists(dirPath);
        }

        public IEnumerable<string> EnumerateFiles(string dirPath)
        {
            return Directory.EnumerateFiles(dirPath);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string[] ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
