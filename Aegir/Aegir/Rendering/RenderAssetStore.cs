using Aegir.Rendering.Geometry.OBJ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Rendering
{
    /// <summary>
    /// Contains methods for loading a model and retreiving it based on id
    /// </summary>
    public class RenderAssetStore
    {
        /// <summary>
        /// Our list holding the different mesh data objects
        /// </summary>
        private List<ObjMesh> models;

        /// <summary>
        /// Constructs a new store instance
        /// </summary>
        public RenderAssetStore()
        {
            models = new List<ObjMesh>();
        }
        /// <summary>
        /// Loads a mesh from a file
        /// </summary>
        /// <param name="fileName">Filename of model/mesh to be loaded</param>
        /// <returns>index/id of newly created mesh for later lookup</returns>
        public int CreateModelFromFile(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            //Check that it exists
            if(!file.Exists)
            {
                throw new ArgumentException("File does not exist: " + fileName);
            }
            //Check that its not already loaded
            int indexOfExisting = models.FindIndex(x => x.FileName == file.FullName);
            if(indexOfExisting != -1) { return indexOfExisting; }

            //if not load it
            ObjMesh newModelMesh = new ObjMesh(file);
            bool isLoaded = newModelMesh.LoadFile();
            if(!isLoaded)
            {
                throw new InvalidDataException("Could not load geometry data from" + fileName);
            }
            //Add to collection
            models.Add(newModelMesh);
            //Return index
            return models.Count - 1;
        }
        /// <summary>
        /// Looks up a mesh from an id/index
        /// </summary>
        /// <param name="index">The mesh index/id</param>
        /// <returns>Mesh data for this index/id</returns>
        public ObjMesh LookupModelMesh(int index)
        {
            if(index>models.Count-1 || index<0)
            {
                return null;
            }
            return models[index];
        }

    }
}
