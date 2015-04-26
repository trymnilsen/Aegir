using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO.File
{
    /// <summary>
    /// This class loads and reads a file, and returns an array 
    /// </summary>
    public class SpaceDelimitedReader : IDisposable
    {
       
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public List<string> ReadAllOfFile(string file)
        {
            if(file == null)
            {
                throw new ArgumentNullException("File to read was null");
            }
            try
            {
                using(StreamReader sr = new StreamReader(file))
                {
                    string line = sr.ReadLine();
                    
                }
            }
        }
        
    }
}
