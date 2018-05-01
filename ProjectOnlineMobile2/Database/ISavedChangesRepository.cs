using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using TasksResult = ProjectOnlineMobile2.Models.ResourceAssignmentModel.Result;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.Result;

namespace ProjectOnlineMobile2.Database
{
    public interface ISavedChangesRepository
    {
        //used for the changes that were made on the hours of the timesheet lines
        Task<List<LineWorkChangesModel>> GetChangesAsync();
        Task<bool> AddEntryAsync(LineWorkChangesModel savedChanges);
        Task<bool> RemoveEntryAsync(DateTime startDate);

        //used for offline syncing for the projects
        Task<List<ProjectResult>> GetProjects();
        Task<bool> AddProjects(ProjectResult projects);
        Task<bool> RemoveProjects(ProjectResult projects);

        //used for offline syncing for the tasks
        Task<List<TasksResult>> GetUserTasks();
        Task<bool> AddUserTask(TasksResult task);
        Task<bool> RemoveTask(TasksResult tasks);

        //used for offline syncing for the timesheet periods
        Task<List<TimesheetPeriodsResult>> GetTimesheetPeriods();
        Task<bool> AddTimesheetPeriods(TimesheetPeriodsResult period);
        Task<bool> RemoveTimesheetPeriods(TimesheetPeriodsResult period);
    }
}
