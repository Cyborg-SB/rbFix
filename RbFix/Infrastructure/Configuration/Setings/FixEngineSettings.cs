using QuickFix;
using RbFix.Infrastructure.Configuration.Setings.Interfaces;

namespace RbFix.Infrastructure.Configuration.Setings
{

    public class FixEngineSettings : IFixEngineSettingsBase
    {
        private const string defaultLogAndStorePath = "/";
        public string FileStorePath { get; set; }
        public string FileLogPath { get; set; }
        public bool MillisecondsInTimeStamp { get; set; } = true;
        public bool SendRedundantResendRequests { get; set; } = false;
        public bool EnableLastMsgSeqNumProcessed { get; set; } = false;
        public ulong MaxMessagesInResendRequest { get; set; } = 10000;
        public bool RequiresOrigSendingTime { get; set; } = true;
        public bool ValidateLengthAndChecksum { get; set; } = false;
        public bool CheckFieldsHaveValues { get; set; } = true;
        public bool CheckFieldsOutOfOrder { get; set; } = false;
        public bool CheckUserDefinedFields { get; set; } = false;
        public bool AllowUnknownMsgFields { get; set; } = true;
        public bool CheckLatency { get; set; } = true;
        public int MaxLatency { get; set; } = 120;
        public FixEngineSettings()
        {
            FileStorePath = defaultLogAndStorePath;
            FileLogPath = defaultLogAndStorePath;
        }

        public FixEngineSettings(string fileStorePath,
            string fileLogPath)
        {
            FileStorePath = fileStorePath;
            FileLogPath = fileLogPath;
        }

    }
}
