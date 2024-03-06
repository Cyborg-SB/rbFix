using QuickFix;
using RbFix.Infrastructure.Configuration;
using RbFix.Infrastructure.Providers.Interfaces;

namespace RbFix.Infrastructure.Providers
{
    public class SessionSettingsProvider : SessionSettings, ISessionSettingsProvider
    {
        public SessionSettingsProvider(string sessionsConfig)
        {
            Instance = this;
            Load(sessionsConfig);
        }
        public SessionSettingsProvider Instance { get; private set; }

        internal void Load(string conf)
        {
            var mStream = new MemoryStream();
            var writer = new StreamWriter(mStream);
            writer.Write(conf);
            writer.Flush();
            mStream.Position = 0;
            Load(new StreamReader(mStream));
        }

    }
}
