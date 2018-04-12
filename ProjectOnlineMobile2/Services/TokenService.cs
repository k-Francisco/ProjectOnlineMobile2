using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace ProjectOnlineMobile2.Services
{
    //this class is dependent on the plugin Xam.Plugins.Settings by James Montemagno
    //It is required that you install the plugin on your project through nuget
    public class TokenService
    {
        public bool SaveCookies(string cookie) {

            bool doneSavingCookies = false;

            if (cookie != null)
            {
                if(cookie.Contains("rtFa") && cookie.Contains("FedAuth"))
                {
                    Settings.CookieString = JsonConvert.SerializeObject(cookie);

                    if (!String.IsNullOrWhiteSpace(Settings.CookieString))
                    {
                        doneSavingCookies = true;
                    }
                }
            }
            return doneSavingCookies;

        }

        public String ExtractRtFa()
        {
            string rtFa = string.Empty;

            string[] token = JsonConvert.DeserializeObject<string>(Settings.CookieString).Split(new char[] { ';' });

            for (int i = 0; i < token.Length; i++)
            {
                if (token[i].Contains("rtFa"))
                {
                    rtFa = token[i].Replace("rtFa=","");
                }
            }

            return rtFa;
        }

        public String ExtractFedAuth()
        {
            string FedAuth = string.Empty;

            string[] token = JsonConvert.DeserializeObject<string>(Settings.CookieString).Split(new char[] { ';' });

            for (int i = 0; i < token.Length; i++)
            {
                if (token[i].Contains("FedAuth"))
                {
                    FedAuth = token[i].Replace("FedAuth=", "");
                }
            }

            return FedAuth;
        }

        public bool IsAlreadyLoggedIn()
        {
            bool isLogged = false;

            if (!String.IsNullOrWhiteSpace(Settings.CookieString))
                isLogged = true;

            return isLogged;
        }

    }
}
