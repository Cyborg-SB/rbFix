using QuickFix;
using QuickFix.Fields.Converters;
using System.Text;

namespace RbFix.Infrastructure.Providers
{
    public class FixFileLogProvider : ILog, IDisposable
    {
        private readonly object sync_ = new();

        private StreamWriter messageLog_;

        private StreamWriter eventLog_;

        private string messageLogFileName_;

        private string eventLogFileName_;

        private bool _disposed;

        public FixFileLogProvider(string fileLogPath)
        {
            Init(fileLogPath, "GLOBAL");
        }

        public FixFileLogProvider(string fileLogPath, SessionID sessionID)
        {
            Init(fileLogPath, Prefix(sessionID));
        }

        private void Init(string fileLogPath, string prefix)
        {
            if (!Directory.Exists(fileLogPath))
            {
                Directory.CreateDirectory(fileLogPath);
            }
            string sufix = $".{DateTime.Now:yyyyMMdd}";

            messageLogFileName_ = Path.Combine(fileLogPath, prefix + sufix + ".messages.current.log");
            eventLogFileName_ = Path.Combine(fileLogPath, prefix + sufix + ".event.current.log");
            messageLog_ = new StreamWriter(messageLogFileName_, append: true);
            eventLog_ = new StreamWriter(eventLogFileName_, append: true);
            messageLog_.AutoFlush = true;
            eventLog_.AutoFlush = true;
        }

        public static string Prefix(SessionID sessionID)
        {
            StringBuilder stringBuilder = new StringBuilder(sessionID.BeginString).Append('-').Append(sessionID.SenderCompID);
            if (SessionID.IsSet(sessionID.SenderSubID))
            {
                stringBuilder.Append('_').Append(sessionID.SenderSubID);
            }

            if (SessionID.IsSet(sessionID.SenderLocationID))
            {
                stringBuilder.Append('_').Append(sessionID.SenderLocationID);
            }

            stringBuilder.Append('-').Append(sessionID.TargetCompID);
            if (SessionID.IsSet(sessionID.TargetSubID))
            {
                stringBuilder.Append('_').Append(sessionID.TargetSubID);
            }

            if (SessionID.IsSet(sessionID.TargetLocationID))
            {
                stringBuilder.Append('_').Append(sessionID.TargetLocationID);
            }

            if (SessionID.IsSet(sessionID.SessionQualifier))
            {
                stringBuilder.Append('-').Append(sessionID.SessionQualifier);
            }

            return stringBuilder.ToString();
        }

        private void DisposedCheck()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Clear()
        {
            DisposedCheck();
            lock (sync_)
            {
                messageLog_.Close();
                eventLog_.Close();
                messageLog_ = new StreamWriter(messageLogFileName_, append: false);
                eventLog_ = new StreamWriter(eventLogFileName_, append: false);
                messageLog_.AutoFlush = true;
                eventLog_.AutoFlush = true;
            }
        }

        public void OnIncoming(string msg)
        {
            DisposedCheck();
            lock (sync_)
            {
                messageLog_.WriteLine(DateTimeConverter.Convert(DateTime.UtcNow) + " : " + msg);
            }
        }

        public void OnOutgoing(string msg)
        {
            DisposedCheck();
            lock (sync_)
            {
                messageLog_.WriteLine(DateTimeConverter.Convert(DateTime.UtcNow) + " : " + msg);
            }
        }

        public void OnEvent(string s)
        {
            DisposedCheck();
            lock (sync_)
            {
                eventLog_.WriteLine(DateTimeConverter.Convert(DateTime.UtcNow) + " : " + s);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                messageLog_?.Dispose();

                eventLog_?.Dispose();

                messageLog_ = null;
                eventLog_ = null;
            }

            _disposed = true;
        }

        ~FixFileLogProvider()
        {
            Dispose(disposing: false);
        }
    }
}
