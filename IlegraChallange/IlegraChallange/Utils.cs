using System;
using System.Runtime.InteropServices;

namespace IlegraChallange
{
    public class Utils
    {
        public static string GetHomePath()
        {
            var envHome = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "HOMEPATH" : "HOME";
            return Environment.GetEnvironmentVariable(envHome);
        }
    }
}