using System;
using log4net;

namespace Octopus.Log
{
    /// <summary>
    /// 日志输出
    /// </summary>
    public class LogOut
    {
        #region Logger
        static readonly ILog Octopus_log = LogManager.GetLogger("Octopus_LogFileAppender");
        #endregion

        #region 全局5个函数
        public static void Debug(string message)
        {
            if (Octopus_log.IsDebugEnabled)
            {
                Octopus_log.Debug(message);
            }
        }
        public static void Error(string message)
        {
            if (Octopus_log.IsErrorEnabled)
            {
                Octopus_log.Error(message);
            }
        }
        public static void Fatal(string message)
        {
            if (Octopus_log.IsFatalEnabled)
            {
                Octopus_log.Fatal(message);
            }
        }
        public static void Info(string message)
        {
            if (Octopus_log.IsInfoEnabled)
            {
                Octopus_log.Info(message);
            }
        }
        public static void Warn(string message)
        {
            if (Octopus_log.IsWarnEnabled)
            {
                Octopus_log.Warn(message);
            }
        }

        public static void Debug(string message, Exception ex)
        {
            if (Octopus_log.IsDebugEnabled)
            {
                Octopus_log.Debug(message, ex);
            }
        }
        public static void Error(string message, Exception ex)
        {
            if (Octopus_log.IsErrorEnabled)
            {
                Octopus_log.Error(message, ex);
            }
        }
        public static void Fatal(string message, Exception ex)
        {
            if (Octopus_log.IsFatalEnabled)
            {
                Octopus_log.Fatal(message, ex);
            }
        }
        public static void Info(string message, Exception ex)
        {
            if (Octopus_log.IsInfoEnabled)
            {
                Octopus_log.Info(message, ex);
            }
        }
        public static void Warn(string message, Exception ex)
        {
            if (Octopus_log.IsWarnEnabled)
            {
                Octopus_log.Warn(message, ex);
            }
        }
        #endregion
    }
}