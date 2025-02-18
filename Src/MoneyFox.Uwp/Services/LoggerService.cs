﻿using MoneyFox.Application.Common.Constants;
using NLog;
using NLog.Config;
using NLog.Targets;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public static class LoggerService
    {
        public static string LogFileName => "moneyfox.log";

        public static void Initialize()
        {
            var config = new LoggingConfiguration();
            var logfile = new FileTarget("logfile")
            {
                FileName = AppConstants.LogFileName, AutoFlush = true, ArchiveEvery = FileArchivePeriod.Month
            };

            // Configure console
            var debugTarget = new DebugTarget("console");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }
    }
}