using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegir.Rendering.Geometry.Buffer;
using AegirLib.IO.Parser;

namespace Aegir.IO
{
    public class ObjFxFile
    {
        private SpaceDelimitedParser reader;

        public ObjFxFile(string fileName)
        {
            reader = new SpaceDelimitedParser(fileName);
        }
        /// <summary>
        /// Reads the given Obj model file into memory
        /// </summary>
        /// <returns>returns self for fluency</returns>
        public ObjFxFile Read()
        {
            return this;
        }

        public GeometryVolume GetBuffers()
        {
            
        }
    }
}
