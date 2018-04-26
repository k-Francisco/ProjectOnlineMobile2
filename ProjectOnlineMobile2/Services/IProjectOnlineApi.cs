﻿using Newtonsoft.Json;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Models.PTA;
using ProjectOnlineMobile2.Models.PTL;
using ProjectOnlineMobile2.Models.ResourceAssignmentModel;
using ProjectOnlineMobile2.Models.TaskModel;
using ProjectOnlineMobile2.Models.TLL;
using ProjectOnlineMobile2.Models.TSPL;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public interface IProjectOnlineApi
    {
        [Get("/_api/ProjectData/Projects(guid'{guid}')")]
        Task<ProjectServerProject> GetProjectByGuid(string guid);

        [Get("/_api/ProjectData/Projects?$filter=ProjectName eq '{projectName}'")]
        Task<ProjectServerProject> GetProjectByName(string projectName);

        [Get("/_api/ProjectData/Projects?$filter=ProjectLastPublishedDate ne null")]
        Task<ProjectServerProjectList> GetAllProjects();

        [Get("/_api/ProjectServer/Projects('{projectUID}')/Tasks")]
        Task<TaskModel> GetTasksByProject(string projectUID);

        [Get("/_api/ProjectData/Projects(guid'{projectUID}')/Assignments?$filter=ResourceName eq '{resourceName}'")]
        Task<ResourceAssignmentModel> GetResourceAssignment(string projectUID, string resourceName);

        [Get("/_api/ProjectServer/timesheetperiods")]
        Task<TimeSheetPeriodList> GetAllTimesheetPeriods();

        [Get("/_api/ProjectServer/TimesheetPeriods('{periodId}')/Timesheet/Lines")]
        Task<TimesheetLinesList> GetTimesheetLinesByPeriod(string periodId);

        [Get("/_api/ProjectServer/TimesheetPeriods('{periodId}')/Timesheet/")]
        Task<String> GetTimesheet(string periodId);
    }
}
