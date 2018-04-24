using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Models.PTA;
using ProjectOnlineMobile2.Models.PTL;
using ProjectOnlineMobile2.Models.ResourceAssignmentModel;
using ProjectOnlineMobile2.Models.TaskModel;
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
                Debug.WriteLine("GetProjectByGuid", e.Message);
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
                Debug.WriteLine("GetProjectByName", e.Message);
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
                Debug.WriteLine("GetAllProjects", e.Message);
                return null;
            }
        }

        public async Task<TaskModel> GetTasksByProject(string projectUID)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetTasksByProject(projectUID);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTasksByProject", e.Message);
                return null;
            }
        }

        public async Task<ResourceAssignmentModel> GetResourceAssignment(string projectUID, string resourceName)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetResourceAssignment(projectUID, resourceName);
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetResourceAssignment", e.Message);
                return null;
            }
        }

        //public async Task<ProjectTaskList> GetProjectTask(string projectUID, string taskId)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetProjectTask(projectUID, taskId);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        return null;
        //    }
        //}

        //public async Task<ProjectTaskAssignment> GetProjectTaskAssignment(string projectUID, string taskId)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetProjectTaskAssignment(projectUID, taskId);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        return null;
        //    }
        //}

        //public async Task<ProjectTaskAssignment> GetProjectAssignments(string projectUID)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetProjectAssignments(projectUID);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        return null;
        //    }
        //}
    }
}
