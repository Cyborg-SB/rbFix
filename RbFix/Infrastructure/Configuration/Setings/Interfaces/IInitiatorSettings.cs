namespace RbFix.Infrastructure.Configuration.Setings.Interfaces
{
    public interface IInitiatorSettings
    {
        public int ReconnectInterval { get; }
        public int HeartBeat { get; }
        public int LogonTimeout { get; }
        public int LogoutTimeout { get; }
        public int SocketConnectPort { get; }
        public string SocketConnectHost { get; }
    }
}
