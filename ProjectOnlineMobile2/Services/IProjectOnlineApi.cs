using Newtonsoft.Json;
using ProjectOnlineMobile2.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public interface IProjectOnlineApi
    {
        [Post("/_api/ProjectData/Projects(guid'{guid}')")]
        Task<ProjectServerProject> GetProjectByGuid(string guid);

        [Post("/_api/ProjectData/Projects?$filter=ProjectName eq '{projectName}'")]
        Task<ProjectServerProject> GetProjectByName(string projectName);

        [Get("/_api/ProjectData/Projects?$filter=ProjectLastPublishedDate ne null")]
        Task<ProjectServerProjectList> GetAllProjects();
    }
}
