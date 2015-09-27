using System;
using System.IO;
namespace DCRF.Interface.Helper
{

    /// <summary>
    /// Provides access to local file system of the caller code.
    /// Mainly used in distributed DCRF which component is not on the same
    /// machine as the caller code
    /// </summary>
    public interface ILocalAccess
    {
        Stream CreateFileStream(string filePath, FileMode mode);
        void DeleteFile(string path);
        void DeleteFolder(string path, bool recursive);
        bool FileExists(string path);
        bool FolderExists(string path);
        string[] GetFiles(string folderPath);
    }
}
