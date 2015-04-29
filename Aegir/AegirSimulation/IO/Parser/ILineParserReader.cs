using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO.Parser
{
    public interface ILineParserReader
    {
        /// <summary>
        /// Parses the given string as a line from a file
        /// </summary>
        /// <param name="line">the file line to parse</param>
        /// <returns>Array of parsed line elements</returns>
        /// <remarks>Returns null if line is not valid/should not be included</remarks>
        string[] ParseLine(string line);
    }
}
