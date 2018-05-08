using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
using Xamarin.Forms;

namespace ProjectOnlineMobile2.Services
{
    public class ProjectOnlineApiWrapper : BaseWrapper
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

        //public async Task<ProjectServerProject> GetProjectByGuid(string guid)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetProjectByGuid(guid);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("GetProjectByGuid", e.Message);
        //        return null;
        //    }
        //}

        //public async Task<ProjectServerProject> GetProjectByName(string projectName)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetProjectByName(projectName);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("GetProjectByName", e.Message);
        //        return null;
        //    }
        //}

        public async Task<ProjectServerProjectList> GetAllProjects()
        {
            try
            {
                var response = await _client.GetStringAsync(_projectOnlineUrl + "/_api/ProjectData/Projects?$filter=ProjectLastPublishedDate ne null");
                return JsonConvert.DeserializeObject<ProjectServerProjectList>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetAllProjects", e.Message);
                return null;
            }
        }

        //public async Task<TaskModel> GetTasksByProject(string projectUID)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetTasksByProject(projectUID);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("GetTasksByProject", e.Message);
        //        return null;
        //    }
        //}

        public async Task<ResourceAssignmentModel> GetResourceAssignment(string projectUID, string resourceName)
        {
            try
            {
                var response = await _client.GetStringAsync(_projectOnlineUrl +
                    "/_api/ProjectData/Projects(guid'"+ projectUID +"')/Assignments?$filter=ResourceName eq '"+ resourceName +"'");

                return JsonConvert.DeserializeObject<ResourceAssignmentModel>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetResourceAssignment", e.Message);
                return null;
            }
        }

        public async Task<TimeSheetPeriodList> GetAllTimesheetPeriods()
        {
            try
            {
                var response = await _client.GetStringAsync(_projectOnlineUrl + "/_api/ProjectServer/timesheetperiods");
                return JsonConvert.DeserializeObject<TimeSheetPeriodList>(response);
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
                var response = await _client.GetStringAsync(_projectOnlineUrl + "/_api/ProjectServer/TimesheetPeriods('"+ periodId +"')/Timesheet/Lines");

                return JsonConvert.DeserializeObject<TimesheetLinesList>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTimesheetLinesByPeriod", e.Message);
                if (e.Message.Equals("404 (Not Found)"))
                {
                    MessagingCenter.Instance.Send<String>(periodId, "DoCreateTimesheet");
                }
                return null;
            }
        }

        //public async Task<TimesheetModel> GetTimesheet(string periodId)
        //{
        //    try
        //    {
        //        return await RestService.For<IProjectOnlineApi>(_client).GetTimesheet(periodId);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("GetTimesheet", e.Message);
        //        return null;
        //    }
        //}

        public async Task<bool> CreateTimesheet(string periodId, string formDigest)
        {
            try
            {
                var contents = new StringContent("", Encoding.UTF8, "application/json");

                if(!_client.DefaultRequestHeaders.Contains("X-RequestDigest"))
                    _client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);

                var response = await _client.PostAsync(_projectOnlineUrl + "/_api/ProjectServer/TimesheetPeriods('" + periodId + "')/createTimesheet()", contents);
                var postResponse = response.EnsureSuccessStatusCode();
                if (postResponse.IsSuccessStatusCode)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTimesheet", e.Message);
                return false;
            }
        }

        public async Task<TimesheetLineWorkModel> GetTimesheetLineWork(string periodId, string lineId)
        {
            try
            {
                var response = await _client.GetStringAsync(_projectOnlineUrl +
                    "/_api/ProjectServer/TimesheetPeriods('"+periodId+"')/Timesheet/Lines('"+lineId+"')/Work");

                return JsonConvert.DeserializeObject<TimesheetLineWorkModel>(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTimesheetLineWork", e.Message);
                return null;
            }
        }

        public async Task<bool> AddTimesheetLineWork(string periodId, string lineId, string body, string formDigestValue)
        {
            bool isSuccess = false;

            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
            _client.DefaultRequestHeaders.Add("X-RequestDigest", formDigestValue);
            try
            {
                var result = await _client.PostAsync(_projectOnlineUrl + "/_api/ProjectServer/TimesheetPeriods('" + periodId + "')" +
                    "/Timesheet/Lines('" + lineId + "')/Work/add", contents);

                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                _client.DefaultRequestHeaders.Remove("X-RequestDigest");

                return isSuccess;
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddTimesheetLineWork", e.Message);
                return isSuccess;
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
