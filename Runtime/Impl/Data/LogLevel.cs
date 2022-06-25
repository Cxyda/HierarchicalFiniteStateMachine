namespace Packages.HFSM.Runtime.Impl.Data
{
    public enum LogLevel
    {
        /// <summary>
        /// Only logs errors and exceptions
        /// </summary>
        Error = 0,
        /// <summary>
        /// Logs warning and all logs of lower levels
        /// </summary>
        Warning = 1,
        /// <summary>
        /// Logs standard log messages and all logs of lower levels
        /// </summary>
        Log = 2,
        /// <summary>
        /// Logs informative log messages and all logs of lower levels
        /// </summary>
        Info = 3,
        /// <summary>
        /// Enables full message logging
        /// </summary>
        Verbose = 4
    }
}