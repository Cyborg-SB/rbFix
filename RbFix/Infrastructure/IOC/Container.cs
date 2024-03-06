using RbFix.Application.Services;
using RbFix.Application.Services.Interfaces;
using RbFix.Application.UseCases;
using RbFix.Application.UseCases.Interfaces;
using RbFix.Infrastructure.Configuration;
using RbFix.Infrastructure.Configuration.Setings;
using RbFix.Infrastructure.Providers;
using RbFix.Infrastructure.Providers.Interfaces;
using RbFix.Services;

namespace RbFix.Infrastructure.IOC
{
    public static class Container    {

        public static void FixEngineInit(
            this IServiceCollection services,
            FixEngineSettings fixEngineSettings,
            string sessionsConfig)
        {
            services.AddSingleton<ISessionSettingsProvider>(_ = new SessionSettingsProvider(sessionsConfig));
            FixEngine.Initialize(fixEngineSettings);
        }
        public static void RegisterFixManagerComponent(this IServiceCollection services)
        {
            services.AddSingleton<IFixSchedulerService, FixSchedulerService>();
            services.AddSingleton<IFixSessionService, FixSessionService>();
            services.AddHostedService<FixService>();
        }

        public static void RegisterUseCases(this IServiceCollection services)
        {
            services.AddSingleton<IRegisterSessionUseCase, RegisterSessionUseCase>();
            services.AddSingleton<IUnRegisterSessionUseCase, UnRegisterSessionUseCase>();
            services.AddSingleton<IGetSessionUseCase, GetSessionUseCase>();
        }
    }
}

