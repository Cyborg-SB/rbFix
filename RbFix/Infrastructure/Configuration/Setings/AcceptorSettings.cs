using RbFix.Infrastructure.Configuration.Setings.Interfaces;

namespace RbFix.Infrastructure.Configuration.Setings
{
    public class AcceptorSettings : IAcceptorSettings
    {
        public AcceptorSettings(int socketAcceptorPort,
            string socketAcceptorHost)
        {
            SocketAcceptPort = socketAcceptorPort;
            SocketAcceptHost = socketAcceptorHost;
        }
        public int SocketAcceptPort { get; set; }

        public string SocketAcceptHost { get; set; }
    }
}
