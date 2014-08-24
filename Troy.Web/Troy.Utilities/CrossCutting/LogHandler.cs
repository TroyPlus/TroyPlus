#region Namespaces

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
#endregion

namespace Troy.Utilities.CrossCutting
{
    /// <summary>
    ///   Provides mechanism to log the appliaction activities into a
    ///   log source (SQL Database or Flat file).
    /// </summary>
    public static class LogHandler
    {
        #region Constructor

        /// <summary>
        ///   Initializes the LogHandler.
        /// </summary>
        static LogHandler()
        {
            Logger.SetLogWriter(new LogWriterFactory().Create(), false);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Writes the log message into the target file source as specified in the
        ///   logging configuration.
        /// </summary>
        /// <param name="logMessage">Log message to write.</param>
        /// <param name="severity">Log severity.</param>
        public static void WriteLog(string logMessage,
                                    TraceEventType severity = TraceEventType.Information)
        {
            LogEntry logEntry = new LogEntry
                                {
                                    Message = logMessage,
                                    Severity = severity
                                };

            Logger.Write(logEntry);
        }

        #endregion
    }
}