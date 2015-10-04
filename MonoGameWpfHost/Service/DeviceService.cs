using System;
using System.Windows;
using System.Windows.Interop;
using SharpDX.Direct3D9;
using MonoGameWpfHost.Util;

namespace MonoGameWpfHost.Service
{
    internal static class DeviceService
    {
        private static int activeClients;
        private static Direct3DEx d3DContext;
        private static DeviceEx d3DDevice;

        public static DeviceEx D3DDevice
        {
            get { return d3DDevice; }
        }

        public static void StartD3D(Window parentWindow)
        {
            activeClients++;

            if (activeClients > 1)
                return;

            d3DContext = new Direct3DEx();

            var presentParameters = new PresentParameters
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard,
                DeviceWindowHandle = new WindowInteropHelper(parentWindow).Handle,
                PresentationInterval = PresentInterval.Default
            };

            d3DDevice = new DeviceEx(d3DContext, 0, DeviceType.Hardware, IntPtr.Zero,
                CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve,
                presentParameters);
        }

        public static void EndD3D()
        {
            activeClients--;
            if (activeClients < 0)
                throw new InvalidOperationException();

            if (activeClients != 0)
                return;

            Disposer.RemoveAndDispose(ref d3DDevice);
            Disposer.RemoveAndDispose(ref d3DContext);
        }
    }
}
