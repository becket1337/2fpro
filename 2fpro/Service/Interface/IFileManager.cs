using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Service.Interface
{
    public interface IFileManager
    {
        byte[] ConvertToBytes(IFormFile image);
        void ClearDir(string path);
        void WriteFiles(IEnumerable<IFormFile> files, string path);
        void WriteFile(IFormFile file, string path);
        Task WriteImage(IFormFile file, string path, bool isWM = false, int width = 0, int height = 0);
        void CheckDirectory(string path);
        string GetRandomName();
        void RemoveDir(string path);
        void RemoveFile(string path);
        void CreateDir(string path);
        IEnumerable<DirectoryInfo> GetDirs();
        DirectoryInfo GetDir(string folder);

    }
}
