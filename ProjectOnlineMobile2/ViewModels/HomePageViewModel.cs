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
using System.Collections.ObjectModel;

namespace ProjectOnlineMobile2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public ICommand GoToProjectsPage { get; set; }
        public ICommand GoToTasksPage { get; set; }
        public ICommand GoToTimesheetPage { get; set; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { SetProperty(ref _userEmail, value); }
        }

        public HomePageViewModel()
        {
            GoToProjectsPage = new Command(ExecuteGoToProjectsPage);
            GoToTasksPage = new Command(ExecuteGoToTasksPage);
            GoToTimesheetPage = new Command(ExecuteGoToTimesheetPage);

            GetUserInfo();

        }

        //private async void CheckWorkChanges(List<LineWorkChangesModel> savedWork)
        //{
        //    if(savedWork != null && savedWork.Any())
        //    {
        //        MessagingCenter.Instance.Send<String>("Uploading timesheet changes", "Toast");
        //        try
        //        {
        //            var formDigest = await SPapi.GetFormDigest();

        //            foreach (var item in savedWork)
        //            {
        //                var body = "{'parameters':{'ActualWork':'" + item.ActualHours + "', " +
        //                    "'PlannedWork':'" + item.PlannedHours + "', " +
        //                    "'Start':'" + item.StartDate + "', " +
        //                    "'NonBillableOvertimeWork':'0h', " +
        //                    "'NonBillableWork':'0h', " +
        //                    "'OvertimeWork':'0h'}}";
        //                var response = await PSapi.AddTimesheetLineWork(item.PeriodId, item.LineId, body, formDigest.D.GetContextWebInformation.FormDigestValue);
        //                if (response)
        //                    await savedChangesRepo.RemoveEntryAsync(item.StartDate);
        //                else
        //                    MessagingCenter.Instance.Send<String>("There was an error uploading the changes", "Toast");
        //            }
        //        }
        //        catch(Exception e)
        //        {
        //            Debug.WriteLine("CheckWorkChanges", e.Message);
        //        }
        //    }
        //}

        //private async void CheckPeriodChanges(List<Models.TSPL.Result> savedPeriods)
        //{
        //    try
        //    {
        //        var periods = await PSapi.GetAllTimesheetPeriods();

        //        if(savedPeriods != null || savedPeriods.Any())
        //        {
        //            var isTheSame = savedPeriods.SequenceEqual(periods.D.Results);

        //            if (isTheSame)
        //            {
        //                MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(periods.D.Results, "DisplayTimesheetPeriods");
        //            }
        //            else
        //            {
        //                foreach (var item in savedPeriods)
        //                {
        //                    await savedChangesRepo.RemoveTimesheetPeriods(item);
        //                }

        //                foreach (var item in periods.D.Results)
        //                {
        //                    await savedChangesRepo.AddTimesheetPeriods(item);
        //                }

        //                MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(periods.D.Results, "DisplayTimesheetPeriods");
        //            }
        //        }
        //        else
        //        {
        //            foreach (var item in periods.D.Results)
        //            {
        //                await savedChangesRepo.AddTimesheetPeriods(item);
        //            }
        //            MessagingCenter.Instance.Send<List<Models.TSPL.Result>>(periods.D.Results, "DisplayTimesheetPeriods");
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("CheckPeriodChanges", e.Message);
        //    }
        //}

        //private async void CheckUserTaskChanges(List<TasksResult> savedTasks, List<ProjectResult> savedProjects, bool projectsResult)
        //{
        //    List<TasksResult> userTasks = new List<TasksResult>();
        //    try
        //    {
        //        if (projectsResult)
        //        {
        //            if(savedProjects.Any() && savedProjects != null)
        //            {
        //                foreach (var item in savedProjects)
        //                {
        //                    var tasks = await PSapi.GetResourceAssignment(item.ProjectId, UserName);
        //                    foreach (var assignments in tasks.D.Results)
        //                    {
        //                        userTasks.Add(assignments);
        //                    }
        //                }

        //                if (userTasks.Any())
        //                {
        //                    var isTheSame = savedTasks.SequenceEqual(userTasks);

        //                    if (isTheSame)
        //                    {
        //                        MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
        //                    }
        //                    else
        //                    {
        //                        foreach (var item in savedTasks)
        //                        {
        //                            await savedChangesRepo.RemoveTask(item);
        //                        }

        //                        foreach (var item in userTasks)
        //                        {
        //                            await savedChangesRepo.AddUserTask(item);
        //                        }

        //                        MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var projects = await PSapi.GetAllProjects();

        //                foreach (var item in projects.D.Results)
        //                {
        //                    var tasks = await PSapi.GetResourceAssignment(item.ProjectId, UserName);
        //                    foreach (var assignments in tasks.D.Results)
        //                    {
        //                        userTasks.Add(assignments);
        //                    }
        //                }

        //                if (userTasks.Any())
        //                {
        //                    foreach (var item in userTasks)
        //                    {
        //                        await savedChangesRepo.AddUserTask(item);
        //                    }

        //                    MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
        //                }

        //            }
        //        }
        //        else
        //        {
        //            var newSavedProjects = await savedChangesRepo.GetProjects();

        //            if(newSavedProjects.Any() && newSavedProjects != null)
        //            {
        //                foreach (var item in newSavedProjects)
        //                {
        //                    var tasks = await PSapi.GetResourceAssignment(item.ProjectId, UserName);
        //                    foreach (var assignments in tasks.D.Results)
        //                    {
        //                        userTasks.Add(assignments);
        //                    }
        //                }

        //                if (userTasks.Any())
        //                {
        //                    var isTheSame = savedTasks.SequenceEqual(userTasks);

        //                    if (isTheSame)
        //                    {
        //                        MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
        //                    }
        //                    else
        //                    {
        //                        foreach (var item in savedTasks)
        //                        {
        //                            await savedChangesRepo.RemoveTask(item);
        //                        }

        //                        foreach (var item in userTasks)
        //                        {
        //                            await savedChangesRepo.AddUserTask(item);
        //                        }

        //                        MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var projects = await PSapi.GetAllProjects();

        //                foreach (var item in projects.D.Results)
        //                {
        //                    var tasks = await PSapi.GetResourceAssignment(item.ProjectId, UserName);
        //                    foreach (var assignments in tasks.D.Results)
        //                    {
        //                        userTasks.Add(assignments);
        //                    }
        //                }

        //                if (userTasks.Any())
        //                {
        //                    foreach (var item in userTasks)
        //                    {
        //                        await savedChangesRepo.AddUserTask(item);
        //                    }

        //                    MessagingCenter.Instance.Send<List<TasksResult>>(userTasks, "DisplayUserTasks");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("CheckUserTaskChanges", e.Message);
        //    }
        //}

        //private async Task<bool> CheckProjectChanges(List<ProjectResult> savedProjects)
        //{
        //    try
        //    {
        //        var projects = await PSapi.GetAllProjects();

        //        if(savedProjects != null && savedProjects.Any())
        //        {
        //            var isTheSame = savedProjects.SequenceEqual(projects.D.Results);

        //            if (isTheSame)
        //            {
        //                MessagingCenter.Instance.Send<List<ProjectResult>>(savedProjects, "DisplayProjects");
        //                return isTheSame;
        //            }
        //            else
        //            {
        //                foreach (var item in savedProjects)
        //                {
        //                    var isRemoved = await savedChangesRepo.RemoveProjects(item);
        //                    if (isRemoved)
        //                        Debug.WriteLine("CheckProjectChanges-null", isRemoved + "ang result");
        //                    else
        //                        Debug.WriteLine("CheckProjectChanges-null", isRemoved + "ang result");
        //                }

        //                foreach (var item in projects.D.Results)
        //                {
        //                    await savedChangesRepo.AddProjects(item);
        //                }
        //                MessagingCenter.Instance.Send<List<ProjectResult>>(projects.D.Results, "DisplayProjects");
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            foreach (var item in projects.D.Results)
        //            {
        //                var isAdded = await savedChangesRepo.AddProjects(item);
        //            }
        //            MessagingCenter.Instance.Send<List<ProjectResult>>(projects.D.Results, "DisplayProjects");
        //            return false;
        //        }
                
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("CheckProjectChanges", e.Message);
        //        return true;
        //    }
        //}

        private async void GetUserInfo()
        {
            try
            {
                var user = await SPapi.GetCurrentUser();
                UserName = user.D.Title;
                UserEmail = user.D.Email;
                MessagingCenter.Instance.Send<UserModel>(user, "UserInfo");
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetUserInfo", e.Message);
            }
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
