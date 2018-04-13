using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProjectOnlineMobile2.Models;
using Refit;

namespace ProjectOnlineMobile2.Services
{
    public class ProjectOnlineApiWrapper : BaseWrapper, IProjectOnlineApi
    {
        private static string _projectOnlineUrl = "https://sharepointevo.sharepoint.com/sites/mobility";
        private HttpClient _client;

        public ProjectOnlineApiWrapper()
        {
            if (_client == null)
            {
                _client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_projectOnlineUrl)
                };
                _client.DefaultRequestHeaders.Accept.Add(mediaType);
            }
        }

        public async Task<ProjectServerProject> GetProjectByGuid(string guid)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetProjectByGuid(guid);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<ProjectServerProject> GetProjectByName(string projectName)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetProjectByName(projectName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<ProjectServerProjectList> GetAllProjects()
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetAllProjects();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
