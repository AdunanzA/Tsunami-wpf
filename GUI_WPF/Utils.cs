using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Utils
{
    class Utils
    {
        public bool IsWindowsOs()
        {
            if (Environment.OSVersion.Platform == PlatformID.MacOSX && Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return false;
            }
            else return true;
        }

        public bool Is64BitOs()
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
    }
}
