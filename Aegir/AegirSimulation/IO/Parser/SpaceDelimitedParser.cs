using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegirLib.Logging;

namespace AegirLib.IO.Parser
{
    /// <summary>
    /// This class loads and reads a file, and returns an array 
    /// </summary>
    public class SpaceDelimitedParser : LineParser
    {
        private string commentCharacter;

        /// <summary>
        /// Create a space delimited file parser
        /// </summary>
        /// <param name="file">file to load/read</param>
        public SpaceDelimitedParser(string file)
            : base(file) {}

        /// <summary>
        /// Creates a new SpaceDelimitedReader, ignoring any lines starting 
        /// with the given comment character (can also be a string/sequence of chars).
        /// </summary>
        /// <param name="file">the file to load</param>
        /// <param name="commentCharacter">The character(s) marking line as comment</param>
        public SpaceDelimitedParser(string file, string commentCharacter) 
            :base(file)
        {
            this.commentCharacter = commentCharacter;
        }

        /// <summary>
        /// Implements the parse function and returns elements for given line
        /// </summary>
        /// <param name="line">line to parse</param>
        /// <returns>Parsed line elements</returns>
        /// <remarks>We assume the file is small enough to not get any problems with string split</remarks>
        protected override string[] ParseLine(string line)
        {
            if (commentCharacter!=null && line.StartsWith(commentCharacter))
            {
                return null;
            }
            string[] lineElements = line.Split(' ');
            return lineElements;
        }
    }
}
