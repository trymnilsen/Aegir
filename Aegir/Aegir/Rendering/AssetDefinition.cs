using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    /// <summary>
    /// Definition for the different assets that can be rendered during
    /// a simulation 
    /// </summary>
    public struct AssetDefinition
    {
        public string Type;
        public string Filepath;

        /// <summary>
        /// Creates a new AssetDefintion with for the given type (as string name)
        /// pointing with its filepath to a obj_gfx file
        /// </summary>
        /// <param name="type">type of actor this definition is valid for</param>
        /// <param name="filepath">file path to obj_gfx file</param>
        public AssetDefinition(string type, string filepath)
        {
            this.Type = type;
            this.Filepath = filepath;
        }
    }
}
