using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami
{
    public static class Utils
    {
        public static string GiveMeStateFromEnum(Enum stateFromCore)
        {
            string res = "";
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
            string sRes = "";
            switch (smFromCore.ToString())
            {
                case "storage_mode_allocate":
                    sRes = "Allocate";
                    break;
                case "storage_mode_sparse":
                    sRes = "Sparse";
                    break;
                default:
                    sRes = "Sparse";
                    break;
            }
            return sRes;
        }

    }
}
