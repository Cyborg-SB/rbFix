namespace RbFix.Infrastructure.Configuration.Setings.Interfaces
{
    public interface IFixEngineSettingsBase
    {
        public string FileStorePath { get; }
        public string FileLogPath { get; }
        public bool MillisecondsInTimeStamp { get; }
        public bool SendRedundantResendRequests { get; }
        public bool EnableLastMsgSeqNumProcessed { get; }
        public ulong MaxMessagesInResendRequest { get; }
        public bool RequiresOrigSendingTime { get; }
        public bool CheckFieldsHaveValues { get; }
        public bool CheckFieldsOutOfOrder { get; }
        public bool CheckUserDefinedFields { get; }

        public bool ValidateLengthAndChecksum { get; }
        public bool AllowUnknownMsgFields { get; }
        public bool CheckLatency { get; }
    }
}
