using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.Patterns;
using StdPlatBS100;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Primavera.Log
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

        #endregion

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

        #endregion

        #region Public Methods

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

        #endregion
    }
}
