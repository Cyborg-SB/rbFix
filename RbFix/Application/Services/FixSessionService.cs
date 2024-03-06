
using QuickFix;
using RbFix.Application.Services.Interfaces;
using RbFix.Infrastructure.Providers.Interfaces;

namespace RbFix.Application.Services
{
    internal class FixSessionService : IFixSessionService
    {
        private readonly ISessionSettingsProvider sessionSettingsProvider;
        private readonly IFixSchedulerService fixSchedulerService;
        public FixSessionService(
                    ISessionSettingsProvider sessionSettingsProvider,
                    IFixSchedulerService fixSchedulerService)
        {
            this.sessionSettingsProvider = sessionSettingsProvider;
            this.fixSchedulerService = fixSchedulerService;
        }


        public Session? GetSession(string senderCompId)
        {
            return fixSchedulerService.Sessions.FirstOrDefault(x => x.SessionID.SenderCompID == senderCompId);
        }
        public IReadOnlyCollection<Session> GetSessions()
        {
            return fixSchedulerService.Sessions;
        }
        public bool IsSessionRegistered(string senderCompId)
        {
            return sessionSettingsProvider.Instance.Has(GetSession(senderCompId)?.SessionID);
        }

        public bool UnRegisterSession(string senderCompId)
        {
            return sessionSettingsProvider.Instance.Remove(GetSession(senderCompId)?.SessionID);
        }

        public void RegisterSession(string senderCompId, Dictionary dictionary)
        {
           sessionSettingsProvider.Instance.Set(GetSession(senderCompId)?.SessionID, dictionary);
        }
    }
 }
