using QuickFix;
using RbFix.Infrastructure.Configuration;
using RbFix.Infrastructure.Helpers;

namespace RbFix.Infrastructure.Providers
{
    public class FixFileLogFactory : ILogFactory
    {
        private readonly SessionSettingsProvider settings_;
        public FixFileLogFactory(SessionSettingsProvider settings)
        {
            this.settings_ = settings;
        }

        public ILog Create(SessionID sessionID)
        {
            return new FixFileLogProvider(settings_.Get(sessionID).GetStringSafe(SessionSettings.FILE_LOG_PATH) ?? FixEngine.Instance.Settings.FileLogPath, sessionID);
        }
    }
}
