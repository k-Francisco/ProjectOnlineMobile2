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

            GetUserTasks();
        }

        private async void GetUserTasks()
        {
            try
            {
                await Task.Delay(2000);
                var savedTasks = realm.All<Result>();
                var savedProjects = realm.All<ProjectOnlineMobile2.Models.PSPL.Result>();
                var userInfo = realm.All<ProjectOnlineMobile2.Models.D_User>();

                List<Result> tempCollection = new List<Result>();

                if (IsConnectedToInternet())
                {
                    if (savedProjects.Any())
                    {
                        foreach (var item in savedProjects)
                        {
                            var assignment = await PSapi.GetResourceAssignment(item.ProjectId, userInfo.First().Title);
                            foreach (var item2 in assignment.D.Results)
                            {
                                tempCollection.Add(item2);
                            }
                        }

                        var isTheSame = savedTasks.SequenceEqual(tempCollection);

                        if (isTheSame)
                        {
                            foreach (var item in savedTasks)
                            {
                                Tasks.Add(item);
                            }
                        }
                        else
                        {
                            realm.Write(()=> {
                                realm.RemoveAll<Result>();
                            });

                            foreach (var item in tempCollection)
                            {
                                realm.Write(()=> {
                                    realm.Add(item);
                                });
                            }

                            realm.Refresh();

                            foreach (var item in savedTasks)
                            {
                                Tasks.Add(item);
                            }
                        }

                    }
                    else
                    {
                        var projects = await PSapi.GetAllProjects();

                        foreach (var item in projects.D.Results)
                        {
                            realm.Write(() => {
                                realm.Add(item);
                            });

                            var assignment = await PSapi.GetResourceAssignment(item.ProjectId, userInfo.First().Title);
                            foreach (var item2 in assignment.D.Results)
                            {
                                tempCollection.Add(item2);
                            }
                        }

                        var isTheSame = savedTasks.SequenceEqual(tempCollection);

                        if (isTheSame)
                        {
                            foreach (var item in savedTasks)
                            {
                                Tasks.Add(item);
                            }
                        }
                        else
                        {
                            realm.Write(() => {
                                realm.RemoveAll<Result>();
                            });

                            foreach (var item in tempCollection)
                            {
                                realm.Write(() => {
                                    realm.Add(item);
                                });
                            }

                            realm.Refresh();

                            foreach (var item in savedTasks)
                            {
                                Tasks.Add(item);
                            }
                        }

                    }
                }
                else
                {
                    foreach (var item in savedTasks)
                    {
                        Tasks.Add(item);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetUserTasks", e.Message);
            }
        }

        
    }
}
