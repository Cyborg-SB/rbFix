#region assembly QuickFix, Version=1.11.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\CyborgSB\.nuget\packages\quickfixn.core\1.11.0\lib\net6.0\QuickFix.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using QuickFix.Util;
using System.IO;
using System.Text;

namespace QuickFix
{
    //
    // Resumo:
    //     File store implementation
    public class FixFileStore : IMessageStore, IDisposable
    {
        private class MsgDef
        {
            public long Index { get; private set; }

            public int Size { get; private set; }

            public MsgDef(long index, int size)
            {
                this.Index = index;
                this.Size = size;
            }
        }

        private readonly string seqNumsFileName_;

        private readonly string msgFileName_;

        private readonly string headerFileName_;

        private readonly string sessionFileName_;

        private FileStream seqNumsFile_;

        private FileStream msgFile_;

        private StreamWriter headerFile_;

        private readonly MemoryStore cache_ = new ();

        private readonly Dictionary<ulong, MsgDef> offsets_ = new();

        private bool _disposed;

        public ulong NextSenderMsgSeqNum
        {
            get
            {
                return cache_.NextSenderMsgSeqNum;
            }
            set
            {
                cache_.NextSenderMsgSeqNum = value;
                SetSeqNum();
            }
        }

        public ulong NextTargetMsgSeqNum
        {
            get
            {
                return cache_.NextTargetMsgSeqNum;
            }
            set
            {
                cache_.NextTargetMsgSeqNum = value;
                SetSeqNum();
            }
        }

        public DateTime? CreationTime => cache_.CreationTime;

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

        public FixFileStore(string path, SessionID sessionID)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string text = Prefix(sessionID);
            string sufix = $".{DateTime.Now:yyyyMMdd}";
            seqNumsFileName_ = Path.Combine(path, text + sufix + ".seqnums");
            msgFileName_ = Path.Combine(path, text + sufix + ".body");
            headerFileName_ = Path.Combine(path, text + sufix + ".header");
            sessionFileName_ = Path.Combine(path, text + sufix + ".session");
            Open();
        }

        private void Open()
        {
            Close();
            ConstructFromFileCache();
            InitializeSessionCreateTime(GetCreationTime());
            seqNumsFile_ = new FileStream(seqNumsFileName_, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            msgFile_ = new FileStream(msgFileName_, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            headerFile_ = new StreamWriter(headerFileName_, append: true);
        }

        private void Close()
        {
            seqNumsFile_?.Dispose();
            msgFile_?.Dispose();
            headerFile_?.Dispose();
        }

        private static void PurgeSingleFile(Stream stream, string filename)
        {
            stream?.Close();
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private static void PurgeSingleFile(StreamWriter stream, string filename)
        {
            stream?.Close();
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private static void PurgeSingleFile(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private void PurgeFileCache()
        {
            PurgeSingleFile(seqNumsFile_, seqNumsFileName_);
            PurgeSingleFile(msgFile_, msgFileName_);
            PurgeSingleFile(headerFile_, headerFileName_);
            PurgeSingleFile(sessionFileName_);
        }

        private void ConstructFromFileCache()
        {
            offsets_.Clear();
            if (File.Exists(headerFileName_))
            {
                using StreamReader streamReader = new (headerFileName_);
                string text;
                while ((text = streamReader.ReadLine()) != null)
                {
                    string[] array = text.Split(',');
                    if (array.Length == 3)
                    {
                        offsets_[Convert.ToUInt64(array[0])] = new MsgDef(Convert.ToInt64(array[1]), Convert.ToInt32(array[2]));
                    }
                }
            }

            if (!File.Exists(seqNumsFileName_))
            {
                return;
            }

            using StreamReader streamReader2 = new (seqNumsFileName_);
            string[] array2 = streamReader2.ReadToEnd().Split(':');
            if (array2.Length == 2)
            {
                cache_.NextSenderMsgSeqNum = Convert.ToUInt64(array2[0]);
                cache_.NextTargetMsgSeqNum = Convert.ToUInt64(array2[1]);
            }
        }

        private DateTime? GetCreationTime()
        {
            return cache_.CreationTime;
        }

        private void InitializeSessionCreateTime(DateTime? creationTime)
        {
            if (File.Exists(sessionFileName_) && new FileInfo(sessionFileName_).Length > 0)
            {
                using StreamReader streamReader = new (sessionFileName_);
                string s = streamReader.ReadToEnd();
                creationTime = UtcDateTimeSerializer.FromString(s);
            }
            else
            {
                using StreamWriter streamWriter = new (sessionFileName_, append: false);
                streamWriter.Write(UtcDateTimeSerializer.ToString(cache_.CreationTime.Value));
            }
        }
        public void Get(ulong startSeqNum, ulong endSeqNum, List<string> messages)
        {
            for (ulong num = startSeqNum; num <= endSeqNum; num++)
            {
                if (offsets_.ContainsKey(num))
                {
                    msgFile_.Seek(offsets_[num].Index, SeekOrigin.Begin);
                    byte[] array = new byte[offsets_[num].Size];
                    msgFile_.Read(array, 0, array.Length);
                    messages.Add(CharEncoding.DefaultEncoding.GetString(array));
                }
            }
        }

        public bool Set(ulong msgSeqNum, string msg)
        {
            msgFile_.Seek(0L, SeekOrigin.End);
            long position = msgFile_.Position;
            byte[] bytes = CharEncoding.DefaultEncoding.GetBytes(msg);
            int num = bytes.Length;
            StringBuilder stringBuilder = new ();
            stringBuilder.Append(msgSeqNum).Append(',').Append(position)
                .Append(',')
                .Append(num);
            headerFile_.WriteLine(stringBuilder.ToString());
            headerFile_.Flush();
            offsets_[msgSeqNum] = new MsgDef(position, num);
            msgFile_.Write(bytes, 0, num);
            msgFile_.Flush();
            return true;
        }

        public void IncrNextSenderMsgSeqNum()
        {
            cache_.IncrNextSenderMsgSeqNum();
            SetSeqNum();
        }

        public void IncrNextTargetMsgSeqNum()
        {
            cache_.IncrNextTargetMsgSeqNum();
            SetSeqNum();
        }

        private void SetSeqNum()
        {
            seqNumsFile_.Seek(0L, SeekOrigin.Begin);
            StreamWriter streamWriter = new (seqNumsFile_);
            streamWriter.Write(NextSenderMsgSeqNum.ToString("D20") + " : " + NextTargetMsgSeqNum.ToString("D20") + "  ");
            streamWriter.Flush();
        }

        public void Reset()
        {
            cache_.Reset();
            PurgeFileCache();
            Open();
        }

        public void Refresh()
        {
            cache_.Reset();
            Open();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }

                _disposed = true;
            }
        }

        ~FixFileStore()
        {
            Dispose(disposing: false);
        }
    }
}