using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Targets;
using NLog.Config;

namespace Tsunami.Settings
{
    public static class Logger
    {
        public static void Inizialize()
        {
            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}/Logs/${logger}.txt";
            fileTarget.Layout = @"${date:format=HH\:mm\:ss.fff} ${pad:padding=5:inner=${level:uppercase=true}} [${callsite:className=false:includeSourcePath=false}] - ${message}";
            //LoggingRule rule = new LoggingRule("*", LogLevel.Trace, LogLevel.Fatal, fileTarget);
            LoggingRule rule = new LoggingRule("*", fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }
    }
}
