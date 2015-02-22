using Aegir.Rendering.Geometry.OBJ;
using AegirLib.Data.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Dictionary<Type, int> typeModels;
        /// <summary>
        /// Constructs a new store instance
        /// </summary>
        public RenderAssetStore()
        {
            models = new List<ObjMesh>();

            //Load our models
            typeModels = new Dictionary<Type, int>();
        }
        /// <summary>
        /// Loads the given model and assigns it to the given type
        /// </summary>
        /// <remarks>
        /// Replaces the model if it is already loaded
        /// </remarks>
        /// <param name="filepath">filepath to our model</param>
        /// <param name="targetType">type we want to associate this model with</param>
        public void CreateModelFromFile(string filepath, Type targetType)
        {
            int id = CreateModelFromFile(filepath);
            if (typeModels.ContainsKey(targetType))
            {
                //update it
                typeModels[targetType] = id;
            }
            else
            {
                typeModels.Add(targetType, id);
            }
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
            int newIndex = models.Count - 1;
            Debug.WriteLine("Loaded Model: " + fileName + " with index: " + newIndex);
            Debug.WriteLine("Model info:" + newModelMesh.ToString());
            return newIndex;
        }
        /// <summary>
        /// Looks up a mesh from an id/index
        /// </summary>
        /// <param name="index">The mesh index/id</param>
        /// <returns>Mesh data for this index/id</returns>
        public ObjMesh LookupModelMesh(int index)
        {
            //Check for valid index
            if(index>models.Count-1 || index<0)
            {
                return null;
            }
            return models[index];
        }
        /// <summary>
        /// Looks up a mesh based on a type
        /// </summary>
        /// <param name="type">The type to lookup</param>
        /// <returns>Mesh data</returns>
        public ObjMesh LookupModelMesh(Type type)
        {
            if(typeModels.ContainsKey(type))
            {
                return LookupModelMesh(typeModels[type]);
            }
            return null;
        }


    }
}
