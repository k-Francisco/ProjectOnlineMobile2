using Newtonsoft.Json;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Models.PTA;
using ProjectOnlineMobile2.Models.PTL;
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

        //[Get("/_api/ProjectData/Projects(guid'{projectUID}')/Tasks()?$Select=*")]
        //Task<ProjectTaskList> GetTasksByProject(string projectUID);

        //[Get("/_api/ProjectServer/Projects('{projUID}')/Draft/Tasks/getById({taskId})")]
        //Task<ProjectTaskList> GetProjectTask(string projectUID, string taskId);

        //[Get("/_api/ProjectData/Tasks(ProjectId=guid'{projectUID}',TaskId=guid'{taskId}')/Assignments")]
        //Task<ProjectTaskAssignment> GetProjectTaskAssignment(string projectUID, string taskId);

        //[Get("/_api/ProjectData/Projects(guid'{projectUID}')/Assignments")]
        //Task<ProjectTaskAssignment> GetProjectAssignments(string projectUID);
    }
}
