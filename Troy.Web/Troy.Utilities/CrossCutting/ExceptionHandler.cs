#region Namespaces

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

#endregion

namespace Troy.Utilities.CrossCutting
{
    /// <summary>
    ///   Provides mechanism to log the exception details into
    ///   a log source (SQL DB or flat files).
    /// </summary>
    public static class ExceptionHandler
    {
        #region Fields

        /// <summary>
        ///   Exception manager instance to log the exception.
        /// </summary>
        private static readonly ExceptionManager ExceptionManager;

        #endregion

        #region Constructor

        /// <summary>
        ///   Initializes the ExceptionHandler
        /// </summary>
        static ExceptionHandler()
        {
            //  Create LogWriter.
            Logger.SetLogWriter(new LogWriterFactory().Create(), false);

            // Initialize and set the ExceptionManager for the ExceptionPolicy.
            IConfigurationSource config = ConfigurationSourceFactory.Create();
            ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);
            ExceptionManager = factory.CreateManager();
            ExceptionPolicy.SetExceptionManager(ExceptionManager, false);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Logs the exception details into the target file source as specified in the
        ///   exception and logging policy configuration.
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public static void LogException(Exception exception)
        {
            Exception exceptionToReThrow;
            ExceptionManager.HandleException(exception,
                                             Constants.EXCEPTION_POLICY_NAME,
                                             out exceptionToReThrow);            
        }

        /// <summary>
        ///   Logs the exception details into the target file source as specified in the
        ///   exception and logging policy configuration.
        /// </summary>
        /// <param name="exception">Exception to log</param>
        /// <param name="rethrowException">Rethrow the logged exception.</param>
        public static Exception LogException(Exception exception, bool rethrowException)
        {
            Exception exceptionToReThrow;
            ExceptionManager.HandleException(exception,
                                             Constants.EXCEPTION_POLICY_NAME,
                                             out exceptionToReThrow);

            if (exceptionToReThrow == null)
            {
                exceptionToReThrow = exception;
            }
            if (rethrowException)
            {
                throw exceptionToReThrow;
            }
            return exceptionToReThrow;
        }


        #endregion
    }
}