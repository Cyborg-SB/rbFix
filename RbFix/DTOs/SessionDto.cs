namespace RbFix.DTOs
{
    public class SessionDto
    {
        public string SessionName { get; set; }
        public string SessionTarget { get; set; }
        public bool IsSessionLoggedOn { get; set; }
        public bool IsSessionEnabled { get; set; }
        public bool IsSessionInitiator { get; set; }
        public string SessionFixVersion { get; set; }
        public bool IsSessionTime { get; set; }
        public int HeartBeatInterval { get; set; }
        public bool ResetOnLogon { get; set; }
        public bool ResetOnLogout { get; set; }
    }
}
