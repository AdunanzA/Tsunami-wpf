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
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}/Logs/${logger}.txt";
            fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            var rule = new LoggingRule("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }
    }
}
