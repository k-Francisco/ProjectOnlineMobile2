using ProjectOnlineMobile2.Services;

namespace ProjectOnlineMobile2.Android
{
    public sealed class Singleton
    {
        private static volatile Singleton _instance;
        private static readonly object _syncLock = new object();

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_syncLock)
                {
                    if(_instance == null)
                    {
                        _instance = new Singleton();
                    }
                }

                return _instance;
            }
        }

        public TokenService TokenServices = new TokenService();
        public SharepointApiWrapper sharepointApi = new SharepointApiWrapper();
    }
}