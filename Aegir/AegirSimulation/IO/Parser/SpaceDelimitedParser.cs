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
    public class SpaceDelimitedParser : ILineParserReader
    {
        private bool isCommentAware;
        private string commentCharacter;

        public SpaceDelimitedParser()
        {
            isCommentAware = false;  
        }
        /// <summary>
        /// Creates a new SpaceDelimitedReader, ignoring any lines starting 
        /// with the given comment character (can also be a string/sequence of chars).
        /// </summary>
        /// <param name="CommentCharacter">The character(s) marking line as comment</param>
        public SpaceDelimitedParser(string CommentCharacter)
        {
            isCommentAware = true;
            commentCharacter = commentCharacter;
        }

        /// <summary>
        /// We assume the file is small enough to not get any problems with string split
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string[] ParseLine(string line)
        {
            if (isCommentAware && line.StartsWith(commentCharacter))
            {
                return null;
            }
            string[] lineElements = line.Split(' ');
            return lineElements;
        }
    }
}
