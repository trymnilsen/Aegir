using System;
using System.Collections.Generic;
using System.IO;
using AegirLib.IO.Parser;
using AegirLib.Logging;

namespace AegirLib.IO
{
    public class FileReader
    {
        public LineParser Parser { get; private set; }

        public FileReader(LineParser parser)
        {
            this.Parser = parser;
        }
        /// <summary>
        /// Reads all of the file running it through the specified fileparser
        /// </summary>
        /// <param name="file">path to file to read</param>
        /// <returns>list of values on a line</returns>
        public List<string[]> ReadAllOfFile(string file)
        {
            var parsedLines = new List<string[]>();
/*            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] parsedElements = this.Parser.ParseLine(line);
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
            }*/
            return parsedLines;
        }
    }
}
