using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ProjectOnlineMobile2.Services
{
    public class ProjectOnlineApiWrapper : BaseWrapper, IProjectOnlineApi
    {
        private static string _sharepointUrl = "https://sharepointevo.sharepoint.com/sites/mobility";
        private HttpClient _client;

        public ProjectOnlineApiWrapper()
        {
            if (_client == null)
            {
                _client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_sharepointUrl)
                };
                _client.DefaultRequestHeaders.Accept.Add(mediaType);
            }
        }
    }
}
