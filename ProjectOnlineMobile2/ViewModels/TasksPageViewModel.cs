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

        public TasksPageViewModel()
        {
            Tasks = new ObservableCollection<Result>();
            GetUserTasks();
        }

        private void GetUserTasks()
        {
            if (IsConnectedToInternet())
            {
                //TODO: Get the projects from the db and then retrieve the user's tasks by project
                //TODO: Check if the collections in the db and in the cloud are equal
            }
            else
            {
                //retrieve from the db
            }
        }

        
    }
}
