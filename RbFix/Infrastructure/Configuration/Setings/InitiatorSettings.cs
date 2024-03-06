using RbFix.Infrastructure.Configuration.Setings.Interfaces;

namespace RbFix.Infrastructure.Configuration.Setings
{
    public class InitiatorSettings : IInitiatorSettings
    {
        public InitiatorSettings(int socketConnectPort,
            string socketConnectHost)
        {
            SocketConnectPort = socketConnectPort;
            SocketConnectHost = socketConnectHost;
        }
        public int ReconnectInterval { get; set; } = 30;

        public int HeartBeat { get; set; } = 20;

        public int LogonTimeout { get; set; } = 15;

        public int LogoutTimeout { get; set; } = 3;

        public int SocketConnectPort { get; set; }

        public string SocketConnectHost { get; set; }
    }
}
