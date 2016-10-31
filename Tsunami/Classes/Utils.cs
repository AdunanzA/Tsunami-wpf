using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Tsunami.Classes
{
    /// <summary>
    /// The pourpouse of this Class is to Create a dll full with general porpouse functions 
    /// to use ad includes in vaious projects.
    /// </summary>
    public static class Utils
    {
        //[DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        //public static extern long StrFormatByteSize(long fileSize, System.Text.StringBuilder buffer, int bufferSize);

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
           if (Environment.Is64BitProcess)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
        /// Source: http://www.pinvoke.net/default.aspx/shlwapi.strformatbytesize
        /// </summary>
        /// <param name="filelength">The numeric value to be converted.</param>
        /// <returns>the converted string</returns>
        //public static string StrFormatByteSize(long filesize)
        //{
        //    StringBuilder sb = new StringBuilder(11);
        //    StrFormatByteSize(filesize, sb, sb.Capacity);
        //    return sb.ToString();
        //}

        public static Hashtable GetSettings(string path)
        {
            Hashtable _ret = new Hashtable();
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader
                (
                    new FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read)
                );
                XmlDocument doc = new XmlDocument();
                string xmlIn = reader.ReadToEnd();
                reader.Close();
                doc.LoadXml(xmlIn);
                foreach (XmlNode child in doc.ChildNodes)
                    if (child.Name.Equals("Settings"))
                        foreach (XmlNode node in child.ChildNodes)
                            if (node.Name.Equals("add"))
                                _ret.Add
                                (
                                    node.Attributes["key"].Value,
                                    node.Attributes["value"].Value
                                );
            }
            return (_ret);
        }

        public static string GetWinVlcPath()
        {
            return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\VideoLAN\VLC", "InstallDir", null);
        }

        public static string GiveMeStateFromEnum(Enum stateFromCore)
        {
            string res;
            switch (stateFromCore.ToString())
            {
                case "queued_for_checking":
                    res = "Queued For Checking";
                    break;
                case "checking_files":
                    res = "Checking Files";
                    break;
                case "downloading_metadata":
                    res = "Downloading Metadata";
                    break;
                case "downloading":
                    res = "Downloading";
                    break;
                case "finished":
                    res = "Finished";
                    break;
                case "seeding":
                    res = "Seeding";
                    break;
                case "allocating":
                    res = "Allocating";
                    break;
                case "checking_resume_data":
                    res = "Checking Resume Data";
                    break;
                default:
                    res = "Error";
                    break;
            }
            return res;
        }

        public static string GiveMeStorageModeFromEnum(Enum smFromCore)
        {
            string sRes;
            switch (smFromCore.ToString())
            {
                case "storage_mode_allocate":
                    sRes = "Allocate";
                    break;
                default:
                    sRes = "Sparse";
                    break;
            }
            return sRes;
        }

        public static string RetrieveDirectory()
        {
            string ret;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.ShowDialog();
            ret = fbd.SelectedPath;
            return ret;
        }

    }
}
