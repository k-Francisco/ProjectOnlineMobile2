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
            MessagingCenter.Instance.Subscribe<String>(this, "GetTasks", (s) =>
            {
                if(Tasks.Count == 0)
                    GetTasks();
            });
        }

        private async void GetTasks()
        {
            try
            {
                foreach (var project in projects.D.Results)
                {
                    var resourceAssignments = await PSapi.GetResourceAssignment(project.ProjectId, userName);
                    foreach (var item in resourceAssignments.D.Results)
                    {
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
