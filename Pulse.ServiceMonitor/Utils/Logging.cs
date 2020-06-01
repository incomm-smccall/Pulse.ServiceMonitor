using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using NLog;

namespace Pulse.ServiceMonitor.Utils
{
    public static class Logging
    {
        private static Logger ErrorLogger = LogManager.GetLogger("errors");
        private static Logger GenLogger = LogManager.GetLogger("general");

        public static void LogMessage(LoggingLevel loggerLevel, string message, [CallerMemberName] string functionName = "")
        {
            GenLogger.Log(GetLogLevel(loggerLevel), $"[{functionName}]:{DateTime.Now}:{message}");
        }

        public static void LogErrorMessage(string message, Exception excptionObject, [CallerMemberName] string functionName = "")
        {
            if (excptionObject != null)
                ErrorLogger.Log(LogLevel.Error, excptionObject, $"[{functionName}]:{DateTime.Now}:{message}");
            else
                ErrorLogger.Log(LogLevel.Error, $"[{functionName}]:{DateTime.Now}:{message}");
        }

        private static LogLevel GetLogLevel(LoggingLevel loggerLevel)
        {
            switch (loggerLevel)
            {
                case LoggingLevel.Trace:
                    return LogLevel.Trace;
                case LoggingLevel.Debug:
                    return LogLevel.Debug;
                case LoggingLevel.Info:
                    return LogLevel.Info;
                case LoggingLevel.Warn:
                    return LogLevel.Warn;
                case LoggingLevel.Error:
                    return LogLevel.Error;
                case LoggingLevel.Fatal:
                    return LogLevel.Fatal;
                case LoggingLevel.RunTime:
                    return LogLevel.Info;
                default: // should never come up
                    return LogLevel.Debug;
            }
        }
    }

    public enum LoggingLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
        RunTime
    }
}