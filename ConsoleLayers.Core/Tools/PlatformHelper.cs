using System.Runtime.InteropServices;

namespace ConsoleLayers.Core.Tools
{
    public static class PlatformHelper
    {
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
}