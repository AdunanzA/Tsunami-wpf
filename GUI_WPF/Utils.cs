using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami
{
    /// <summary>
    /// The pourpouse of this Class is to Create a dll full with general porpouse functions 
    /// to use ad includes in vaious projects.
    /// </summary>
    public static class Utils
    {
        public static bool IsWindowsOs()
        {
            if (Environment.OSVersion.Platform == PlatformID.MacOSX && Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return false;
            }
            else return true;
        }

        public static bool Is64BitOs()
        {
            //if (IntPtr.Size == 8)       // 64 bit - impostare il vs. path
            //    return true;
            //else if (IntPtr.Size == 4)  // 32 bit - impostare il vs. path
            //    return false;
            if (Environment.Is64BitOperatingSystem)
            {
                return true;
            }
            else return false;
        }

        public static string GetWinVlcPath()
        {
            RegistryKey localMachine = Registry.LocalMachine;
            string InstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\VideoLAN\VLC", "InstallDir", null);
            if (InstallPath != null)
            {
                return InstallPath; ;
            }
            else throw new Exception("VLC non trovato nel registro"+InstallPath);
        }
    }
}
