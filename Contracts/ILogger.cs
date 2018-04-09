using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LCS
{
    public enum LogSeverity
    {
        Info = 1,
        Warning = 2,
        Error = 4,
        Critical = 8
    }
    public interface ILogEntry
    {
        LogSeverity Severity { get; }
        string Message { get; }
        string FilePath { get; }
        string FuncName { get; }
    }

    public interface ILogViewer
    {
        /// <summary>
        /// Which severities to include in <see cref="Entries"/>.  Defaults to all.
        /// </summary>
        int SeverityFileters { get; set; }
        /// <summary>
        /// A copy of all of the entries
        /// </summary>
        IEnumerable<ILogEntry> Entries { get; }
        /// <summary>
        /// Attempts to get the next entry of the log
        /// </summary>
        /// <param name="entry">the log entry</param>
        /// <returns>whether there is a new entry waiting</returns>
        bool TryGetEntry(out ILogEntry entry);
    }
    public interface ILogger
    {
        void WriteLine(string message, LogSeverity severity = LogSeverity.Info,
            [CallerFilePath] string filePath = null, [CallerMemberName] string funcName = null);


        ILogViewer CreateLogViewer();
    }
}
