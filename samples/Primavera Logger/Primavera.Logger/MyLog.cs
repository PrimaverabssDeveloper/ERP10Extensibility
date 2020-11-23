using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService;
using Primavera.Extensibility.Patterns;
using StdPlatBS100;

namespace Primavera.Logger
{
    /// <summary>
    /// Class that enable debug to the events.
    /// </summary>
    /// <seealso cref="Primavera.Extensibility.Patterns.IExtensibilityLogger" />
    public class MyLog : IExtensibilityLogger
    {
        #region Members

        private readonly string logFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ExtensibilityLogger.txt";
        private StdBSDialogos Dialogos { get; }
        public bool Enabled { get; set; }

        #endregion Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MyLog"/> class.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="dialogos">The dialogos.</param>
        public MyLog(bool enabled, StdBSDialogos dialogos)
        {
            Enabled = enabled;
            Dialogos = dialogos;
        }

        #endregion Constructors

        #region Extensibility Engine Logging

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public void LogError(string message, ErrorSeverity errorSeverity = ErrorSeverity.Normal)
        {
            if (!Enabled)
                return;

            File.AppendAllText(logFile, $"\n[{DateTime.Now.ToShortTimeString()}] - {message}\nStackTrace: {Environment.StackTrace}");

            if (errorSeverity == ErrorSeverity.Critical)
                Dialogos.MostraAviso(message, StdBSTipos.IconId.PRI_Critico, Environment.StackTrace);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public void LogError(string message, Exception exception, ErrorSeverity errorSeverity = ErrorSeverity.Normal)
        {
            if (!Enabled)
                return;

            File.AppendAllText(logFile, $"\n[{DateTime.Now.ToShortTimeString()}] - {message}\nException: {exception}");

            if (errorSeverity == ErrorSeverity.Critical)
                Dialogos.MostraAviso(message, StdBSTipos.IconId.PRI_Critico, exception.ToString());
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (!Enabled)
                return;

            File.AppendAllText(logFile,
                $"\n[{DateTime.Now.ToShortTimeString()}] - {message}\nStackTrace: {Environment.StackTrace}");
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void LogMessage(string message, Exception exception)
        {
            if (!Enabled)
                return;

            File.AppendAllText(logFile, $"\n[{DateTime.Now.ToShortTimeString()}] - {message}\nException: {exception}");
        }

        /// <summary>
        /// Logs the errors.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exceptions">The exceptions.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public void LogErrors(string message, List<Exception> exceptions, ErrorSeverity errorSeverity = ErrorSeverity.Normal)
        {
            if (!Enabled)
                return;

            string detailedMessage = "[Detailed Errors]";

            foreach (var exception in exceptions)
                detailedMessage += $"\nSource:{exception.Source} Message: {exception.Message} Exception: {exception}";

            File.AppendAllText(logFile,
                $"\n[{DateTime.Now.ToShortTimeString()}] - {message}\nMultipleErrorsLog: {detailedMessage}");

            if (errorSeverity == ErrorSeverity.Critical)
                if (exceptions.Any())
                    Dialogos.MostraAviso(message, StdBSTipos.IconId.PRI_Critico, detailedMessage);
        }

        #endregion Public Methods

        #region #region Implementations Logging

        /// <summary>
        /// Logs only the errors caught directly in user implementations, and with access the method that triggered the exception (extensibilityException.Method)
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="extensibilityException">The exception holding the method being executed as an object</param>
        /// <param name="errorSeverity">The error severity.</param>
        public void LogError(string message, ExtensibilityException extensibilityException, ErrorSeverity errorSeverity = ErrorSeverity.Normal)
        {
            LogError(message, extensibilityException.Exception, errorSeverity);
        }


        /// <summary>
        /// Logs only the messages caught directly in user implementations, and with access the method that triggered the exception (extensibilityException.Method)
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="extensibilityException">The exception holding the method being executed as an object</param>
        public void LogMessage(string message, ExtensibilityException extensibilityException)
        {
            LogMessage(message, extensibilityException.Exception);
        }

        /// <summary>
        /// Logs only the errors caught directly in user implementations, and with access the method that triggered the exception (extensibilityException.Method)
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="extensibilityExceptions">The exceptions holding the methods being executed as an object</param>
        /// <param name="errorSeverity">The error severity.</param>
        public void LogErrors(string message, List<ExtensibilityException> extensibilityExceptions, ErrorSeverity errorSeverity = ErrorSeverity.Normal)
        {
            LogErrors(message, extensibilityExceptions.Select(p => p.Exception).ToList(), errorSeverity);
        }

        #endregion
    }
}
