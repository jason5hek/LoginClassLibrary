using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoginClassLib
{
    public interface IFileWrap
    {
        bool Exists(string path);
    }

    public class FileWrapper : IFileWrap
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }


}
