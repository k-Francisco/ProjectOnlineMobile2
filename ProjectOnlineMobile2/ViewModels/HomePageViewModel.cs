using ProjectOnlineMobile2.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using TasksResult = ProjectOnlineMobile2.Models.ResourceAssignmentModel.Result;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Models.ResourceAssignmentModel;
using System.Threading.Tasks;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.TSPL;

namespace ProjectOnlineMobile2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public ICommand GoToProjectsPage { get; set; }
        public ICommand GoToTasksPage { get; set; }
        public ICommand GoToTimesheetPage { get; set; }
        
        public string userName { get; set; }

        public HomePageViewModel()
        {
            GoToProjectsPage = new Command(ExecuteGoToProjectsPage);
            GoToTasksPage = new Command(ExecuteGoToTasksPage);
            GoToTimesheetPage = new Command(ExecuteGoToTimesheetPage);

            MessagingCenter.Instance.Subscribe<SavedChangesRepository>(this, "database", (repo) => {
                this.savedChangesRepo = repo;
                OfflineSync();
            });
        }

        private async void OfflineSync()
        {
            try
            {
                var savedProjects = await savedChangesRepo.GetProjects();
                var savedTasks = await savedChangesRepo.GetUserTasks();
                var savedPeriods = await savedChangesRepo.GetTimesheetPeriods();

                if (IsConnectedToInternet())
                {
                    GetUserInfo();
                    var projectsResult = await CheckProjectChanges(savedProjects);
                    CheckUserTaskChanges(savedTasks,savedProjects, projectsResult);
                    CheckPeriodChanges(savedPeriods);
                }
                else
                {
                    MessagingCenter.Instance.Send<List<ProjectResult>>(savedProjects, "DisplayProjects");
                    MessagingCenter.Instance.Send<List<TasksResult>>(savedTasks, "DisplayUserTasks");
                    MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(savedPeriods, "DisplayTimesheetPeriods");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("OfflineSync", e.Message);
            }
            
        }

        private async void CheckPeriodChanges(List<Models.TSPL.Result> savedPeriods)
        {
            try
            {
                var periods = await PSapi.GetAllTimesheetPeriods();

                if(savedPeriods != null || savedPeriods.Any())
                {
                    var isTheSame = savedPeriods.SequenceEqual(periods.D.Results);

                    if (isTheSame)
                    {
                        MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(periods.D.Results, "DisplayTimesheetPeriods");
                    }
                    else
                    {
                        foreach (var item in savedPeriods)
                        {
                            await savedChangesRepo.RemoveTimesheetPeriods(item);
                        }

                        foreach (var item in periods.D.Results)
                        {
                            await savedChangesRepo.AddTimesheetPeriods(item);
                        }

                        MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(periods.D.Results, "DisplayTimesheetPeriods");
                    }
                }
                else
                {
                    foreach (var item in periods.D.Results)
                    {
                        await savedChangesRepo.AddTimesheetPeriods(item);
                    }
                    MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(periods.D.Results, "DisplayTimesheetPeriods");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("CheckPeriodChanges", e.Message);
            }
        }

        private async void CheckUserTaskChanges(List<TasksResult> savedTasks, List<ProjectResult> savedProjects, bool projectsResult)
        {
            List<TasksResult> userTasks = new List<TasksResult>();
            try
            {
                if (projectsResult)
                {
                    Debug.WriteLine("CheckUserTaskChanges", "true siya");
                    foreach (var item in savedProjects)
                    {
                        Debug.WriteLine("CheckUserTaskChanges", userName + " user");
                        var tasks = await PSapi.GetResourceAssignment(item.ProjectId, userName);
                        foreach (var assignments in tasks.D.Results)
                        {
                            userTasks.Add(assignments);
                        }
                    }

                    if (userTasks.Any())
                    {
                        var isTheSame = savedTasks.SequenceEqual(userTasks);

                        if (isTheSame)
                        {
                            MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
                        }
                        else
                        {
                            foreach (var item in savedTasks)
                            {
                                await savedChangesRepo.RemoveTask(item);
                            }

                            foreach (var item in userTasks)
                            {
                                await savedChangesRepo.AddUserTask(item);
                            }

                            MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
                        }
                    }

                }
                else
                {
                    var newSavedProjects = await savedChangesRepo.GetProjects();

                    foreach (var item in newSavedProjects)
                    {
                        var tasks = await PSapi.GetResourceAssignment(item.ProjectId, userName);
                        foreach (var assignments in tasks.D.Results)
                        {
                            userTasks.Add(assignments);
                        }
                    }

                    if (userTasks.Any())
                    {
                        var isTheSame = savedTasks.SequenceEqual(userTasks);

                        if (isTheSame)
                        {
                            MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
                        }
                        else
                        {
                            foreach (var item in savedTasks)
                            {
                                await savedChangesRepo.RemoveTask(item);
                            }

                            foreach (var item in userTasks)
                            {
                                await savedChangesRepo.AddUserTask(item);
                            }

                            MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("CheckUserTaskChanges", e.Message);
            }
        }

        private async Task<bool> CheckProjectChanges(List<ProjectResult> savedProjects)
        {
            try
            {
                var projects = await PSapi.GetAllProjects();

                if(savedProjects != null || savedProjects.Any())
                {
                    var isTheSame = savedProjects.SequenceEqual(savedProjects);

                    if (isTheSame)
                    {
                        MessagingCenter.Instance.Send<List<ProjectResult>>(projects.D.Results, "DisplayProjects");
                        return isTheSame;
                    }
                    else
                    {
                        foreach (var item in savedProjects)
                        {
                            await savedChangesRepo.RemoveProjects(item);
                        }

                        foreach (var item in projects.D.Results)
                        {
                            await savedChangesRepo.AddProjects(item);
                        }
                        MessagingCenter.Instance.Send<List<ProjectResult>>(projects.D.Results, "DisplayProjects");
                        return false;
                    }
                }
                else
                {
                    foreach (var item in projects.D.Results)
                    {
                        await savedChangesRepo.AddProjects(item);
                    }
                    MessagingCenter.Instance.Send<List<ProjectResult>>(projects.D.Results, "DisplayProjects");
                    return false;
                }
                
            }
            catch(Exception e)
            {
                Debug.WriteLine("CheckProjectChanges", e.Message);
                return true;
            }
        }

        
        private async void GetUserInfo()
        {
            var user = await SPapi.GetCurrentUser();
            userName = user.D.Title;
            MessagingCenter.Instance.Send<UserModel>(user, "UserInfo");
        }

        private void ExecuteGoToTimesheetPage()
        {
            MessagingCenter.Instance.Send<String>("TimesheetPage", "NavigateToPage");
        }

        private void ExecuteGoToTasksPage()
        {
            MessagingCenter.Instance.Send<String>("TasksPage", "NavigateToPage");
        }

        private void ExecuteGoToProjectsPage()
        {
            MessagingCenter.Instance.Send<String>("ProjectPage", "NavigateToPage");
        }
    }
}
