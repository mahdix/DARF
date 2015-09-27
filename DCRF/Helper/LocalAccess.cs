using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Interface.Helper
{
    public class LocalAccess : ILocalAccess
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string folderPath)
        {
            return Directory.GetFiles(folderPath);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void DeleteFolder(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        public Stream CreateFileStream(string filePath,FileMode mode)
        {
            FileStream fs = new FileStream(filePath,mode);

            return fs;
        }
    }
}
