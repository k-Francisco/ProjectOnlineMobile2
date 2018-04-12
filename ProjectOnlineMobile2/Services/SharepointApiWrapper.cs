using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public class SharepointApiWrapper : ISharepointApi
    {
        private static string _sharepointUrl = "https://sharepointevo.sharepoint.com";
        private HttpClient _client;

        public SharepointApiWrapper()
        {
            if(_client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Cookie("rtFa", Settings.rtFaToken, "/", "sharepointevo.sharepoint.com"));
                handler.CookieContainer.Add(new Cookie("FedAuth", Settings.FedAuthToken, "/", "sharepointevo.sharepoint.com"));

                _client = new HttpClient(handler) {
                    BaseAddress = new Uri(_sharepointUrl)
                };
            }
        }

        public async Task<string> GetCurrentUser()
        {
            Debug.WriteLine("rtFa", Settings.rtFaToken);
            Debug.WriteLine("FedAuthToken", Settings.FedAuthToken);
            try
            {
                return await RestService.For<ISharepointApi>(_client).GetCurrentUser();
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> GetFormDigest()
        {
            try
            {
                return await RestService.For<ISharepointApi>(_client).GetFormDigest();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
