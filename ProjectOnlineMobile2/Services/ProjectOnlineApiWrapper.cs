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
using ProjectOnlineMobile2.Models.TLL;
using ProjectOnlineMobile2.Models.TLWM;
using ProjectOnlineMobile2.Models.TM;
using ProjectOnlineMobile2.Models.TSPL;
using Refit;
using Xamarin.Forms;

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

        public async Task<TimeSheetPeriodList> GetAllTimesheetPeriods()
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetAllTimesheetPeriods();
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetAllTimesheetPeriods", e.Message);
                return null;
            }
        }

        public async Task<TimesheetLinesList> GetTimesheetLinesByPeriod(string periodId)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetTimesheetLinesByPeriod(periodId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTimesheetLinesByPeriod", e.Message);
                if(e.Message.Equals("Response status code does not indicate success: 404 (Not Found)."))
                {
                    MessagingCenter.Instance.Send<String>(periodId, "DoCreateTimesheet");
                }
                return null;
            }
        }

        public async Task<TimesheetModel> GetTimesheet(string periodId)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetTimesheet(periodId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTimesheet", e.Message);
                return null;
            }
        }

        public async Task<string> CreateTimesheet(string periodId, string formDigest)
        {
            try
            {
                _client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
                var response =  await RestService.For<IProjectOnlineApi>(_client).CreateTimesheet(periodId, formDigest);
                //_client.DefaultRequestHeaders.Remove("X-RequestDigest");
                return response;
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTimesheet", e.Message);
                return null;
            }
        }

        public async Task<TimesheetLineWorkModel> GetTimesheetLineWork(string periodId, string lineId)
        {
            try
            {
                return await RestService.For<IProjectOnlineApi>(_client).GetTimesheetLineWork(periodId, lineId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTimesheetLineWork", e.Message);
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
