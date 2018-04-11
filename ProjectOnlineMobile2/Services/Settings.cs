

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ProjectOnlineMobile2.Services
{
    public class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string rtFa = "";
        private const string FedAuth = "";

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

        public static void ClearAll()
        {
            AppSettings.Clear();
        }

    }
}
