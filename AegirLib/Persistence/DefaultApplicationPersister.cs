using AegirPresets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AegirLib.Persistence
{
    /// <summary>
    /// Base class for all persisters that has a default preset in the AegirPresets Assembly
    /// simplifies loading of the xml for that assembly by providing file name, loading it and
    /// calling Load with the deserialized XElement Data
    /// </summary>
    public abstract class DefaultApplicationPersister : IApplicationPersister
    {
        /// <summary>
        /// The name of the file containing the default data for this persister
        /// </summary>
        private readonly string defaultPresetPath;

        /// <summary>
        /// Creates a new default perister with the given path to the preset xml file
        /// </summary>
        /// <param name="defaultPath">Name (and path) of file to load or default data from</param>
        public DefaultApplicationPersister(string defaultPath)
        {
            this.defaultPresetPath = defaultPath;
        }

        /// <summary>
        /// Load data of XElement nodes into the application
        /// </summary>
        /// <param name="data">Deserialized Xml nodes of savedata</param>
        public abstract void Load(IEnumerable<XElement> data);

        /// <summary>
        /// Save the current state of the application to XElement nodes
        /// </summary>
        /// <returns></returns>
        public abstract XElement Save();

        /// <summary>
        /// Loads the default preset for this persister
        /// </summary>
        public void LoadDefault()
        {
            Assembly assembly = typeof(PresetsDummyClass).Assembly;

            //Get stream of embedded assembly files
            //resourceStream should be disposed when wrapping reader is disposed
            Stream resourceStream = assembly.GetManifestResourceStream("AegirPresets." + defaultPresetPath);
            using (StreamReader reader = new StreamReader(resourceStream))
            {
                string sceneXml = reader.ReadToEnd();
                string persisterName = GetType().Name;
                XElement deserializedXml = XElement.Parse(sceneXml);
                //our xml is wrapped in a <root> element.. We need to get child of this
                //conviniently its named the same as name of the class persisting it (by convention).
                XElement SceneElement = deserializedXml.Element(persisterName);
                if (SceneElement != null)
                {
                    Load(SceneElement.Elements());
                }
                else
                {
                    //Seems like we didn't find a element with the same name as the perister class
                    //we cannot continue without it
                    throw new PersistanceException($"Could not load {persisterName}, default document was invalid");
                }
            }
        }
    }
}