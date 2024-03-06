using QuickFix;
using RbFix.Infrastructure.Configuration;
using RbFix.Infrastructure.Configuration.Setings;
using RbFix.Infrastructure.Helpers;

namespace RbFix.Infrastructure.Providers
{
    public class FixFileStoreFactory : IMessageStoreFactory
    {
        private readonly SessionSettingsProvider settings_;
        public FixFileStoreFactory(SessionSettingsProvider settings)
        {
            this.settings_ = settings;
        }

        public IMessageStore Create(SessionID sessionID)
        {
            return new FixFileStore(settings_.Get(sessionID).GetStringSafe(SessionSettings.FILE_STORE_PATH) ?? FixEngine.Instance.Settings.FileStorePath, sessionID);
        }
    }
}
