using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Waterbot.Common
{
    /// <summary>
    /// Provides a set of methods and properties that help log messages and 
    /// events.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Adds the specified exception to the log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void AddException(Exception exception)
        {
            WriteInternal(exception.ToString());
        }

        /// <summary>
        /// Adds the specified exception to the log, along with a message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        public static void AddException(Exception exception, string message)
        {
            var builder = new StringBuilder(message);

            builder.AppendLine();
            builder.Append("Details: ");
            builder.Append(exception.ToString());

            WriteInternal(builder);
        }

        /// <summary>
        /// Adds the specified event to the log.
        /// </summary>
        /// <param name="logEvent">An instance of the event to log.</param>
        /// <param name="caller">
        /// The name of the caller to the method. This value is optional and can
        /// be provided automatically when invoked from compilers that support
        /// CallerMemberName.
        /// </param>
        /// <param name="file">
        /// The full path of the source file that contains the caller. This
        /// value is optional and can be provided automatically when invoked
        /// from compilers that support CallerFilePath.
        /// </param>
        /// <param name="lineNo">
        /// The line number in the source file at which the method is called.
        /// This value is optional and can be provided automatically when
        /// invoked from compilers that support CallerLineNumber.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static void Add(ILogEvent logEvent,
            [CallerMemberName] string caller = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int lineNo = 0)
        {
            Add(logEvent.Message, caller, file, lineNo);
        }

        /// <summary>
        /// Adds the specified message to the log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="caller">
        /// The name of the caller to the method. This value is optional and can
        /// be provided automatically when invoked from compilers that support
        /// CallerMemberName.
        /// </param>
        /// <param name="file">
        /// The full path of the source file that contains the caller. This
        /// value is optional and can be provided automatically when invoked
        /// from compilers that support CallerFilePath.
        /// </param>
        /// <param name="lineNo">
        /// The line number in the source file at which the method is called.
        /// This value is optional and can be provided automatically when
        /// invoked from compilers that support CallerLineNumber.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static void Add(string message,
            [CallerMemberName] string caller = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int lineNo = 0)
        {
            var builder = new StringBuilder(message);

            if (!string.IsNullOrEmpty(caller) ||
                !string.IsNullOrEmpty(file) ||
                lineNo > 0)
            {
                var line = lineNo > 0 ? $"line {lineNo}" : null;
                var info = StringUtils.Join(", ", caller, file, line);

                builder.AppendLine();
                builder.Append("\tat ");
                builder.Append(info);
            }

            WriteInternal(builder);
        }

        private static void WriteInternal(StringBuilder message)
        {
            WriteInternal(message.ToString());
        }

        private static void WriteInternal(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
