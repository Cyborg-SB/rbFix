using Microsoft.Extensions.Configuration;
using QuickFix;
using QuickFix.Fields;
using QuickFix.Transport;
using RbFix.Application.Services;
using RbFix.Application.Services.Interfaces;
using RbFix.Infrastructure.Helpers;
using RbFix.Infrastructure.Providers;
using RbFix.Infrastructure.Providers.Interfaces;

namespace RbFix.Services
{
    public class App : IFixApplication
    {

        public App()
        {
            
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
          
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            message.GetJson();
            message.SetField(new StringField(12000, "12000"));
            message.ToJSON(false);
        }

        public void OnCreate(SessionID sessionID)
        {
           
        }

        public void OnLogon(SessionID sessionID)
        {
            
        }

        public void OnLogout(SessionID sessionID)
        {
            
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
          
        }

        public void ToApp(Message message, SessionID sessionID)
        {
            
        }
    }
    internal class FixService: IHostedService
    {
        private readonly IFixSchedulerService scheduler;
        private readonly IFixSessionService fixSessionService;
        public FixService( IFixSchedulerService scheduler, IFixSessionService fixSessionService)
        {
            this.scheduler = scheduler;
            this.fixSessionService = fixSessionService;
        }
        public void InitiateFix()
        {
            scheduler.RegisterSessions(new App());

            fixSessionService.GetSession("ARCAA");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            InitiateFix();
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
