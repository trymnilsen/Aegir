using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirLib.Logging;

namespace AegirLib.IO.Parser
{
    public abstract class LineParser
    {
        /// <summary>
        /// The IO Reader for getting our text, defualts to Stream reader if not defined
        /// </summary>
        public TextReader Reader { get; set; }
        public string FilePath { get; set; }

        protected LineParser(string filepath)
        {
            this.FilePath = filepath;
        }

        /// <summary>
        /// Reads of the the file with the given reader 
        /// </summary>
        /// <returns>List of parsed elements</returns>
        /// <remarks>Disposes the reader after read</remarks>
        public List<string[]> ReadAllOfFile()
        {
            var parsedLines = new List<string[]>();
            if (Reader == null)
            {
                Reader = new StreamReader(FilePath);
            }
            try
            {
                using (Reader)
                {
                    while (!Reader.EndOfStream)
                    {
                        string line = Reader.ReadLine();
                        string[] parsedElements = ParseLine(line);
                        if (parsedElements != null)
                        {
                            parsedLines.Add(parsedElements);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString(), ELogLevel.Error);
            }
            return parsedLines;
        }

        protected abstract string[] ParseLine(string line);
    }
}
