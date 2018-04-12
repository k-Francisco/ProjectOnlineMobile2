using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ProjectOnlineMobile2.Services
{
    //This class is dependent on the package Xam.Plugins.Settings by James Montemagno
    //In order to use the functions available in this class, you need to install the plugin on your project through nuget
    public class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string rtFa = "";
        private const string FedAuth = "";
        private const string cookie = "";

        #endregion

        public static string rtFaToken
        {
            get => AppSettings.GetValueOrDefault(rtFa, string.Empty);
            set => AppSettings.AddOrUpdateValue(rtFa, value);
        }

        public static string FedAuthToken
        {
            get => AppSettings.GetValueOrDefault(FedAuth, string.Empty);
            set => AppSettings.AddOrUpdateValue(FedAuth, value);
        }

        public static string CookieString
        {
            get => AppSettings.GetValueOrDefault(cookie, string.Empty);
            set => AppSettings.AddOrUpdateValue(cookie, value);
        }

        public static void ClearAll()
        {
            AppSettings.Clear();
        }

    }
}
