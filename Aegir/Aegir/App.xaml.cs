using Aegir.Message;
using Aegir.Rendering;
using Aegir.Config;
using AegirLib;
using AegirLib.IO.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AegirLib.Simulation;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            #if DEBUG
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            #endif
            //Load Config
            LoadConfig();
            //Environment.SetEnvironmentVariable("AppEnvironment", SimulationCase.APP_ENV_TOOL);
            SimulationCase simulation = new SimulationCase();
            //RenderAssetStore assetStore = new RenderAssetStore();
            AegirIOC.Register(simulation);
        }
        private void LoadConfig()
        {
            #if DEBUG
                string configFilepath = "../../config.json";
            #else
                string configFilepath = "config.json";
            #endif
            //ConfigFile mainConfig = new ConfigFile(configFilepath);
            //mainConfig.RegisterConfig(new BaseConfig());
            //mainConfig.RegisterConfig(new AssetConfig());
            //mainConfig.SaveFile();
            //AegirIOC.Register(mainConfig);
        }

    }
}
