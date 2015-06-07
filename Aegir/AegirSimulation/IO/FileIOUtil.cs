using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO
{
    public class FileIOUtil
    {
        public static FileInfo GetFileInfoAndCheckExistance(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                throw new ArgumentException("File:["+filePath+"] did not exist");
            }
            return file;
        }
    }
}
