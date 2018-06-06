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
using System.Windows.Input;

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

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ICommand RefreshTasksCommand { get; set; }

        public TasksPageViewModel()
        {
            Tasks = new ObservableCollection<Result>();

            RefreshTasksCommand = new Command(ExecuteRefreshTasksCommand);

            var savedTasks = realm.All<Result>().ToList();

            MessagingCenter.Instance.Subscribe<String>(this, "SortTasks", (s) => {
                ExecuteSortTasks(s, savedTasks);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SyncUserTasks", (s)=> {
                ExecuteSynceUserTasks(savedTasks);
            });

            foreach (var item in savedTasks)
            {
                Tasks.Add(item);
            }
        }

        private void ExecuteSortTasks(string sort, List<Result> savedTasks)
        {
            Tasks.Clear();
            if (sort.Equals("All"))
            {
                foreach (var item in savedTasks)
                {
                    Tasks.Add(item);
                }
            }
            else if (sort.Equals("In Progress"))
            {
                foreach (var item in savedTasks)
                {
                    if(item.AssignmentPercentWorkCompleted != 100)
                    {
                        Tasks.Add(item);
                    }
                }
            }
            else if (sort.Equals("Completed"))
            {
                foreach (var item in savedTasks)
                {
                    if (item.AssignmentPercentWorkCompleted == 100)
                    {
                        Tasks.Add(item);
                    }
                }
            }
        }

        private void ExecuteRefreshTasksCommand()
        {
            try
            {
                IsRefreshing = true;
                if (IsConnectedToInternet())
                {
                    realm.Write(() =>
                    {
                        realm.RemoveAll<Result>();
                    });
                    Tasks.Clear();

                    var savedTasks = realm.All<Result>().ToList();
                    ExecuteSynceUserTasks(savedTasks);
                }
                else
                    IsRefreshing = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteRefreshTasksCommand", e.Message);
                IsRefreshing = false;
            }
            
        }

        private async void ExecuteSynceUserTasks(List<Result> savedTasks)
        {
            try
            {
                var savedProjects = realm.All<ProjectOnlineMobile2.Models.PSPL.Result>().ToList();
                var userInfo = realm.All<ProjectOnlineMobile2.Models.D_User>().FirstOrDefault();
                List<Result> tempCollection = new List<Result>();

                if (IsConnectedToInternet())
                {
                    IsRefreshing = true;

                    foreach (var item in savedProjects)
                    {
                        var assignment = await PSapi.GetResourceAssignment(item.ProjectId, userInfo.Title);
                        foreach (var item2 in assignment.D.Results)
                        {
                            tempCollection.Add(item2);
                        }
                    }

                    syncDataService.SyncUserTasks(savedTasks, tempCollection, Tasks);

                    IsRefreshing = false;
                }
                else
                {
                    string[] alertStrings = { "Your device is not connected to the internet", "Close" };
                    MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncUserTasks", e.Message);
                IsRefreshing = false;

                string[] alertStrings = { "There was a problem syncing your tasks. Please try again", "Close" };
                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
            }
        }
        
    }
}
