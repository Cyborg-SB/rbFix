using QuickFix;
using RbFix.Application.Services.Interfaces;
using RbFix.Application.UseCases.Interfaces;

namespace RbFix.Application.UseCases
{
    public class GetSessionUseCase : IGetSessionUseCase
    {
        private readonly ILogger<GetSessionUseCase> logger;
        private readonly IFixSessionService fixSessionService;

        public GetSessionUseCase(ILogger<GetSessionUseCase> logger, IFixSessionService fixSessionService)
        {
            this.logger = logger;
            this.fixSessionService = fixSessionService;
        }

        public Session GetSession(string sessionNaem)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Session> GetSessions()
        {
            return fixSessionService.GetSessions();
        }
    }
}
