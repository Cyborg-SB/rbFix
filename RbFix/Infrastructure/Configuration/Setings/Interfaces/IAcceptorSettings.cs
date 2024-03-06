namespace RbFix.Infrastructure.Configuration.Setings.Interfaces
{
    public interface IAcceptorSettings
    {
        public int SocketAcceptPort { get; }
        public string SocketAcceptHost { get; }
    }
}
