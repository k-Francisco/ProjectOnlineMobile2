using Result = ProjectOnlineMobile2.Models.ResourceAssignmentModel.AssignmentResult;
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
using System.Threading.Tasks;

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

            var savedTasks = realm.All<Result>().ToList();

            foreach (var item in savedTasks)
            {
                Tasks.Add(item);
            }

            MessagingCenter.Instance.Subscribe<String>(this, "TasksPageInit", (s) => {
                SyncUserTasks(savedTasks);
            });
        }

        private async void SyncUserTasks(List<Result> savedTasks)
        {
            try
            {
                var savedProjects = realm.All<ProjectOnlineMobile2.Models.PSPL.Result>().ToList();
                var userInfo = realm.All<ProjectOnlineMobile2.Models.D_User>().FirstOrDefault();
                List<Result> tempCollection = new List<Result>();

                if (IsConnectedToInternet())
                {
                    foreach (var item in savedProjects)
                    {
                        var assignment = await PSapi.GetResourceAssignment(item.ProjectId, userInfo.Title);
                        foreach (var item2 in assignment.D.Results)
                        {
                            tempCollection.Add(item2);
                        }
                    }

                    syncDataService.SyncUserTasks(savedTasks, tempCollection, Tasks);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncUserTasks", e.Message);
            }
        }
        
    }
}
