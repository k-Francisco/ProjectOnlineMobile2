using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectOnlineMobile2.Models;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Models.ResourceAssignmentModel;
using ProjectOnlineMobile2.Models.TSPL;

namespace ProjectOnlineMobile2.Database
{
    public class SavedChangesRepository : ISavedChangesRepository
    {
        private DatabaseContext _databaseContext;

        public SavedChangesRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }

        public async Task<bool> AddEntryAsync(LineWorkChangesModel savedChanges)
        {
            try
            {
                var tracking = await _databaseContext.Changes.AddAsync(savedChanges);

                await _databaseContext.SaveChangesAsync();

                var isAdded = tracking.State == EntityState.Added;

                return isAdded;
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddEntryAsync", e.Message);
                Debug.WriteLine("ExecuteSaveWorkChanges", e.InnerException.Message);
                return false;
            }
        }

        public async Task<bool> AddProjects(ProjectResult projects)
        {
            try
            {
                var tracking = await _databaseContext.Projects.AddAsync(projects);

                await _databaseContext.SaveChangesAsync();

                var isAdded = tracking.State == EntityState.Added;

                return isAdded;
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddProjects", e.Message);
                return false;
            }
        }

        public async Task<bool> AddTimesheetPeriods(Models.TSPL.Result period)
        {
            try
            {
                var tracking = await _databaseContext.Periods.AddAsync(period);

                await _databaseContext.SaveChangesAsync();

                var isAdded = tracking.State == EntityState.Added;

                return isAdded;
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddTimesheetPeriods", e.Message);
                return false;
            }
        }

        public async Task<bool> AddUserTask(Models.ResourceAssignmentModel.Result task)
        {
            try
            {
                var tracking = await _databaseContext.UserTasks.AddAsync(task);

                await _databaseContext.SaveChangesAsync();

                var isAdded = tracking.State == EntityState.Added;

                return isAdded;
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddUserTask", e.Message);
                return false;
            }
        }

        public async Task<List<LineWorkChangesModel>> GetChangesAsync()
        {
            try
            {
                var changes = await _databaseContext.Changes.ToListAsync();

                return changes;
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetChangesAsync", e.Message);
                return null;
            }
        }

        public async Task<List<ProjectResult>> GetProjects()
        {
            try
            {
                var projects = await _databaseContext.Projects.ToListAsync();

                return projects;
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetProjectsDatabase", e.Message);
                return null;
            }
        }

        public async Task<List<Models.TSPL.Result>> GetTimesheetPeriods()
        {
            try
            {
                var periods = await _databaseContext.Periods.ToListAsync();

                return periods;
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetTimesheetPeriodsDatabase", e.Message);
                return null;
            }
        }

        public async Task<List<Models.ResourceAssignmentModel.Result>> GetUserTasks()
        {
            try
            {
                var tasks = await _databaseContext.UserTasks.ToListAsync();

                return tasks;
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetUserTasksDatabase", e.Message);
                return null;
            }
        }

        public async Task<bool> RemoveEntryAsync(DateTime startDate)
        {
            try
            {
                var entry = await _databaseContext.Changes.FindAsync(startDate);

                var tracking = _databaseContext.Remove(entry);

                await _databaseContext.SaveChangesAsync();

                var isRemoved = tracking.State == EntityState.Deleted;

                return isRemoved;
            }
            catch(Exception e)
            {
                Debug.WriteLine("RemoveEntryAsync", e.Message);
                return false;
            }
        }

        public async Task<bool> RemoveProjects(ProjectResult projects)
        {
            try
            {
                var project = await _databaseContext.Projects.FindAsync(projects);

                var tracking = _databaseContext.Remove(project);

                await _databaseContext.SaveChangesAsync();

                var isRemoved = tracking.State == EntityState.Deleted;

                return isRemoved;
            }
            catch (Exception e)
            {
                Debug.WriteLine("RemoveProjects", e.Message);
                return false;
            }
        }

        public async Task<bool> RemoveTask(Models.ResourceAssignmentModel.Result tasks)
        {
            try
            {
                var task = await _databaseContext.Projects.FindAsync(tasks);

                var tracking = _databaseContext.Remove(task);

                await _databaseContext.SaveChangesAsync();

                var isRemoved = tracking.State == EntityState.Deleted;

                return isRemoved;
            }
            catch (Exception e)
            {
                Debug.WriteLine("RemoveTask", e.Message);
                return false;
            }
        }

        public async Task<bool> RemoveTimesheetPeriods(Models.TSPL.Result period)
        {
            try
            {
                var periodToBeDeleted = await _databaseContext.Projects.FindAsync(period);

                var tracking = _databaseContext.Remove(periodToBeDeleted);

                await _databaseContext.SaveChangesAsync();

                var isRemoved = tracking.State == EntityState.Deleted;

                return isRemoved;
            }
            catch (Exception e)
            {
                Debug.WriteLine("RemoveTimesheetPeriods", e.Message);
                return false;
            }
        }
    }
}
