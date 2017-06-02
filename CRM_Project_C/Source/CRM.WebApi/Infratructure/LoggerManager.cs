﻿using NLog;
using NLog.Targets;
using System;
using System.Net.Http;

namespace CRM.WebApi.Infratructure
{
    public class LoggerManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public LoggerManager()
        {
            FileTarget loggerTarget = (FileTarget)LogManager.Configuration.FindTargetByName("file");
            loggerTarget.DeleteOldFileOnStartup = false;
        }
        public void LogInfo(HttpMethod request, Uri uri)
        {
            Logger.Info($"Request: [ {request} ] | URL [ {uri} ]");
        }
        public void LogError(Exception ex, HttpMethod request, Uri uri)
        {
            Logger.Error($"\nRequest: [ {request} ] | URL [ {uri} ]\nErr: [ {ex.Message} ] Inner: [ {ex.InnerException?.Message} ]\n" + new string('-', 120));
        }
        public void LogException(Exception ex)
        {
            Logger.Log(LogLevel.Fatal, ex, $"\nErr: {ex.Message}\nInner: {ex.InnerException?.Message}\n");
        }
    }


}