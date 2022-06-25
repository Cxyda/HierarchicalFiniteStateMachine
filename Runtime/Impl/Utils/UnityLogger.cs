using Packages.HFSM.Runtime.Impl.Data;
using UnityEngine;

namespace Packages.HFSM.Runtime.Impl.Utils
{
    public interface ILogger
    {
        void Log(LogLevel logType, string message, params string[] args);
    }
    public class UnityLogger : ILogger
    {
        private readonly string _prefix;
        public LogLevel logLevel { get; set; }

        public UnityLogger(LogLevel logLevel, string logPrefix)
        {
            this.logLevel = logLevel;
            _prefix = logPrefix;
        }
        public void Log(LogLevel logType, string message, params string[] args)
        {
            if (logLevel < logType)
            {
                return;
            }

            var logMessage = $"{_prefix} :: {message}\n[{string.Join(";", args)}]";
            switch (logType)
            {
                case LogLevel.Error:
                    Debug.LogError(logMessage);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(logMessage);
                    break;
                default:
                    Debug.Log(logMessage);
                    break;
            }
        }
    }
}