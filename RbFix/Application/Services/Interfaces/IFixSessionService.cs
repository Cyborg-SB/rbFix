using QuickFix;

namespace RbFix.Application.Services.Interfaces
{
    public interface IFixSessionService
    {
        IReadOnlyCollection<Session> GetSessions();
        Session? GetSession(string senderCompId);
        bool IsSessionRegistered(string senderCompId);
        bool UnRegisterSession(string senderCompId);
        void RegisterSession(string senderCompId, Dictionary dictionary);
    }
}
