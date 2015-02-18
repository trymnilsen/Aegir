﻿using Aegir.Message;
using Aegir.Rendering;
using AegirLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aegir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Environment.SetEnvironmentVariable("AppEnvironment", SimulationCase.APP_ENV_TOOL);
            SimulationCase simulation = new SimulationCase();
            RenderAssetStore assetStore = new RenderAssetStore();
            AegirIOC.Register(simulation);
            AegirIOC.Register(assetStore);
        }

    }
}
