using Result = ProjectOnlineMobile2.Models.ResourceAssignmentModel.Result;
using ProjectOnlineMobile2.Models.ResourceAssignmentModel;
using ProjectOnlineMobile2.Models.TaskModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using ProjectOnlineMobile2.Models.PSPL;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TasksPageViewModel : BaseViewModel
    {
        private ObservableCollection<Result> _tasks;
        public ObservableCollection<Result> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        //
        //constructor
        //
        public TasksPageViewModel()
        {
            Tasks = new ObservableCollection<Result>();

            if(NetStandardSingleton.Instance.projects is null)
            {
                GetAllProjectsAndUserTasks();
            }
            else
            {
                VerifyTaskCount();
            }
        }

        //
        //functions
        //
        private void VerifyTaskCount()
        {
            if (NetStandardSingleton.Instance.userTasks.Count is 0)
            {
                GetTasks();
            }
            else
            {
                foreach (var item in NetStandardSingleton.Instance.userTasks)
                {
                    Tasks.Add(item);
                }
            }
        }

        private async void GetAllProjectsAndUserTasks()
        {
            var projects = await PSapi.GetAllProjects();
            MessagingCenter.Instance.Send<ProjectServerProjectList>(projects, "SetProjects");
            VerifyTaskCount();
        }

        private async void GetTasks()
        {
            try
            {
                foreach (var project in NetStandardSingleton.Instance.projects.D.Results)
                {
                    var resourceAssignments = await PSapi.GetResourceAssignment(project.ProjectId, userName);
                    //Debug.WriteLine("GetAllTasks", resourceAssignments.D.Results.Count + "");
                    foreach (var item in resourceAssignments.D.Results)
                    {
                        NetStandardSingleton.Instance.userTasks.Add(item);
                        Tasks.Add(item);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetTasks", e.Message);
            }
        }
    }
}
