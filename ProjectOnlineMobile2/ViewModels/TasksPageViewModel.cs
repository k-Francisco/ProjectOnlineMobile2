using Result = ProjectOnlineMobile2.Models.TaskModel.Result;
using ProjectOnlineMobile2.Models.ResourceAssignmentModel;
using ProjectOnlineMobile2.Models.TaskModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

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

        public TasksPageViewModel()
        {
            Tasks = new ObservableCollection<Result>();
            if(Tasks.Count == 0)
            {
                MessagingCenter.Instance.Subscribe<String>(this, "GetTasks", (s) =>
                {
                    GetTasks();
                });
            }
        }

        private async void GetTasks()
        {
            List<TaskModel> projectTasks = new List<TaskModel>();
            List<string> taskIds = new List<string>();
            try
            {
                foreach (var project in projects.D.Results)
                {
                    var tempProjectTasks = await PSapi.GetTasksByProject(project.ProjectId);
                    projectTasks.Add(tempProjectTasks);
                    var resourceAssignments = await PSapi.GetResourceAssignment(project.ProjectId, userName);
                    foreach (var item in resourceAssignments.D.Results)
                    {
                        taskIds.Add(item.TaskId);
                    }
                }

                foreach (var item in taskIds) {
                    Debug.WriteLine("taskIds", item);
                }

                foreach (var item in projectTasks)
                {
                    foreach (var item2 in item.D.Results)
                    {
                        Debug.WriteLine("projectTasks", item2.Name);
                    }
                }

                //foreach (var task in Tasks)
                //{
                //    foreach (var projectTask in task.D.Results)
                //    {
                //        Debug.WriteLine("GetTasks", projectTask.TaskName);
                //    }
                //}
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetTasks", e.Message);
            }
        }
    }
}
