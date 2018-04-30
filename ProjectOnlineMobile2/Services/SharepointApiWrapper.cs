using ProjectOnlineMobile2.Models;
using Refit;
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

            //if(webInfo == null)
            //{
            //    GetWebInfo();
            //}
        }

        //private async void GetWebInfo()
        //{
        //    webInfo = await GetFormDigest();
        //}

        public async Task<UserModel> GetCurrentUser()
        {
            
            try
            {
                return await RestService.For<ISharepointApi>(_client).GetCurrentUser();
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
                return await RestService.For<ISharepointApi>(_client).GetFormDigest();
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetFormDigest",e.Message);
                return null;
            }
        }
    }
}
