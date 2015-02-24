using AegirLib.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.IO.Config
{
    public class ConfigFile
    {
        private string filePath;
        private Dictionary<Type, IConfigProvider> configObjects;
        public ConfigFile(string filePath)
        {
            this.configObjects = new Dictionary<Type, IConfigProvider>();
            this.filePath = filePath;
        }
        public void RegisterConfig(IConfigProvider configProvider)
        {
            Type configType = configProvider.GetType();
            if(configObjects.ContainsKey(configType))
            {
                configObjects[configType] = configProvider;
            }
            else
            {
                configObjects.Add(configType, configProvider);
            }
        }
        public void LoadFile()
        {
            string fileContents = "";
            try
            {
                fileContents = File.ReadAllText(filePath);
            }
            catch (Exception e) { Logger.Log(e.ToString(), ELogLevel.Error); }
            
        }
        public void SaveFile()
        {
            JObject persistRoot = new JObject();
            foreach(KeyValuePair<Type,IConfigProvider> cp in configObjects)
            {
                persistRoot.Add(cp.Key.ToString(), JToken.FromObject(cp.Value));
            }
            Logger.Log(persistRoot.ToString(),ELogLevel.Debug);
        }
        public IConfigProvider GetConfig<T>()
        {
            if(configObjects.ContainsKey(typeof(T)))
            {
                return configObjects[typeof(T)];
            }
            return null;
        } 
        public struct AssetDefinition
        {
            public string type;
            public string filePath;
        }
    }
}
