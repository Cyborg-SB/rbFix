using QuickFix.DataDictionary;
using RbFix.Infrastructure.Configuration.Setings;

namespace RbFix.Infrastructure.Configuration
{
     public class FixEngine 
    {
        private static readonly object _locker = new ();
        private static bool _isEngineInitialized = false;
        public FixEngineSettings Settings { get; private set; }
        public DataDictionary DataDictionary { get; private set; }
        private FixEngine(FixEngineSettings fixEngineSettings
            )
        {
            Settings = fixEngineSettings;
        }

        public static FixEngine Instance { get; private set; } = null;
        public static bool IsInitialized { get { return _isEngineInitialized; } }


        public static void Initialize(FixEngineSettings fixEngine)
        {
            lock (_locker)
            {
                if (_isEngineInitialized)
                    throw new InvalidOperationException("Engine is already initialized");


                Instance = new FixEngine(fixEngine);
                _isEngineInitialized = true;
            }
        }



    }

}
