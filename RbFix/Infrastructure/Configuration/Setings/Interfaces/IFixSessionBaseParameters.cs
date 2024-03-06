using QuickFix.Fields;

namespace RbFix.Infrastructure.Configuration.Setings.Interfaces
{
    public interface IFixSessionBaseParameters
    {
        public string BeginString { get; set; }
        public string SenderCompID { get; set; }
        public string TargetCompID { get; set; }
        public string ApplVerID { get; set; }
        public string ConnectionType { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DayOfWeek StartDay { get; set; }
        public DayOfWeek EndDay { get; set; }
        public bool ResetOnLogon { get; set; }
        public bool ResetOnLogout { get; set; }
        public bool ResetOnDisconnect { get; set; }
        public bool RefreshOnLogon { get; set; }
        public bool UseDataDictionary { get; set; }
        public string DataDictionary { get; set; }
    }
}
