using AegirLib.Logging;
using Newtonsoft.Json;
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
        public ConfigFile(string filePath)
        {
            this.filePath = filePath;
        }
        public void LoadFile()
        {
            string fileContents = "";
            try
            {
                fileContents = File.ReadAllText(filePath);
            }
            catch (Exception e) { Logger.Log(e.ToString(), ELogLevel.Error); }
            
            //ConfigData = JsonConvert.DeserializeObject<ConfigData>(fileContents);
        }
        public void SaveFile()
        {

        }

        public struct AssetDefinition
        {
            public string type;
            public string filePath;
        }
    }
}
