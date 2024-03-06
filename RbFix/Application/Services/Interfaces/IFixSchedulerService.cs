using QuickFix;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace RbFix.Application.Services.Interfaces
{
    public interface IFixSchedulerService
    {
        void LoadConfiguration(string content);
        void RegisterSessions(IFixApplication app);
        IReadOnlyCollection<Session> Sessions { get; }
    }
}
