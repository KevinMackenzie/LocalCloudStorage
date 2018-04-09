using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LCS
{
    public class Logger : ILogger
    {
        #region Private Fields

        private class Entry : ILogEntry
        {
            public LogSeverity Severity { get; set; }
            public string Message { get; set; }
            public string FilePath { get; set; }
            public string FuncName { get; set; }

            public override string ToString()
            {
                return string.Format($"[{Severity}][{FilePath}:{FuncName}]: {Message}");
            }
        }
        private class LogViewer : ILogViewer
        {
            private readonly Logger _inst;
            private readonly ConcurrentQueue<ILogEntry> _entries = new ConcurrentQueue<ILogEntry>();

            private bool MatchesFilter(ILogEntry e)
            {
                return ((int) e.Severity & SeverityFileters) != 0;
            }

            public LogViewer(Logger inst)
            {
                _inst = inst;
            }

            public int SeverityFileters { get; set; } = 15;

            public IEnumerable<ILogEntry> Entries =>
                from ILogEntry e in _inst._entries where MatchesFilter(e) select e;
            
            public bool TryGetEntry(out ILogEntry entry)
            {
                while (_entries.TryDequeue(out var e))
                {
                    if (!MatchesFilter(e)) continue;

                    entry = e;
                    return true;
                }
                entry = null;
                return false;
            }

            public void SubmitEntry(ILogEntry entry)
            {
                _entries.Enqueue(entry);
            }
        }
        private readonly ConcurrentQueue<Entry> _entries = new ConcurrentQueue<Entry>();
        private readonly List<LogViewer> _logViewers = new List<LogViewer>();
        #endregion

        public void WriteLine(string message, LogSeverity severity = LogSeverity.Info,
            [CallerFilePath] string filePath = null, [CallerMemberName] string funcName = null)
        {
            var e = new Entry()
            {
                Severity = severity,
                Message = message,
                FilePath = filePath,
                FuncName = funcName
            };
            _entries.Enqueue(e);
            _logViewers.ForEach(v => v.SubmitEntry(e));
        }

        public ILogViewer CreateLogViewer()
        {
            _logViewers.Add(new LogViewer(this));
            return _logViewers.Last();
        }
    }
}
