using System;
using System.Diagnostics;

namespace ProjectOnlineMobile2.Services
{
    //this class is dependent on the plugin Xam.Plugins.Settings by James Montemagno
    //It is required that you install the plugin on your project through nuget
    public class TokenService
    {
        public bool ExtractAuthorizationTokens(string cookie) {

            bool doneSavingTokens = false;

            if (cookie != null)
            {
                if(cookie.Contains("rtFa") && cookie.Contains("FedAuth"))
                {
                    Debug.WriteLine("cookie", cookie);
                    string[] token = cookie.Split(new char[] { ';' });
                    for (int i = 0; i < token.Length; i++)
                    {
                        if (token[i].Contains("rtFa"))
                        {
                            Settings.rtFaToken = token[i].Replace("rtFa=", "");
                        }

                        if (token[i].Contains("FedAuth"))
                        {
                            Settings.FedAuthToken = token[i].Replace("FedAuth=", "");
                        }
                    }
                    
                    if (!String.IsNullOrEmpty(Settings.rtFaToken) && (!String.IsNullOrEmpty(Settings.FedAuthToken)))
                        doneSavingTokens = true;
                }
            }
            return doneSavingTokens;

        }

        public bool IsAlreadyLoggedIn()
        {
            bool isLogged = false;

            if (!String.IsNullOrEmpty(Settings.rtFaToken) && !String.IsNullOrEmpty(Settings.FedAuthToken))
                isLogged = true;

            return isLogged;
        }

    }
}
