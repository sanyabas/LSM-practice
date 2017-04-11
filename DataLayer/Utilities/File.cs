using System.IO;

namespace DataLayer.Utilities
{
    public class File : IFile
    {
        private readonly string path;

        public File(string path)
        {
            this.path = path;
        }

        public Stream GetStream()
        {
            return System.IO.File.Open(path,FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
    }
}