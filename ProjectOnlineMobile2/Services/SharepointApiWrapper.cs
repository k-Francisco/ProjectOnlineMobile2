using Newtonsoft.Json;
using ProjectOnlineMobile2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public class SharepointApiWrapper : BaseWrapper,ISharepointApi
    {
        private static string _sharepointUrl = "https://sharepointevo.sharepoint.com";
        private HttpClient _client;

        public SharepointApiWrapper()
        {

            if(_client == null)
            {
                _client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_sharepointUrl)
                };
                _client.DefaultRequestHeaders.Accept.Add(mediaType);
            }

        }

        public async Task<UserModel> GetCurrentUser()
        {
            
            try
            {
                var response = await _client.GetStringAsync(_sharepointUrl + "/_api/web/currentUser?");
                return JsonConvert.DeserializeObject<UserModel>(response);
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetCurrentUser", e.Message);
                return null;
            }
        }

        public async Task<FormDigestModel> GetFormDigest()
        {
            try
            {
                var contents = new StringContent("", Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_sharepointUrl+"/sites/mobility/_api/contextinfo",contents);
                var postResponse = response.EnsureSuccessStatusCode();

                if (postResponse.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<FormDigestModel>(await postResponse.Content.ReadAsStringAsync());
                else
                    return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetFormDigest",e.Message);
                return null;
            }
        }
    }
}
