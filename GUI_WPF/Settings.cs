using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tsunami
{
    class Settings
    {
        /// <summary>Per ora metto sul desktop, 
        /// TODO: controllare se il settaggio è visto altrove nelle altre classi
        /// e va bene non statico</summary>
        public string PATH_DOWNLOAD = Environment.SpecialFolder.Desktop.ToString();

    }
}
