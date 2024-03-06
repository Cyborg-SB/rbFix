using QuickFix;

namespace RbFix.Application.UseCases.Interfaces
{
    public interface IGetSessionUseCase
    {
        Session GetSession(string sessionNaem);
        IReadOnlyCollection<Session> GetSessions();
    }
}
