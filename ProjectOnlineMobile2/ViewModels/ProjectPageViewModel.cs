using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.ViewModels
{
    public class ProjectPageViewModel : BaseViewModel
    {
        private ObservableCollection<Result> _projectList;
        public ObservableCollection<Result> ProjectList
        {
            get { return _projectList; }
            set { SetProperty(ref _projectList, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ICommand RefreshProjects { get; set; }

        public ProjectPageViewModel()
        {
            ProjectList = new ObservableCollection<Result>();
            RefreshProjects = new Command(ExecuteRefreshProjects);

            var savedProjects = realm.All<Result>().ToList();

            foreach (var item in savedProjects)
            {
                ProjectList.Add(item);
            }

            SyncProjects(savedProjects);
        }

        private void ExecuteRefreshProjects()
        {
            try
            {
                IsRefreshing = true;
                if (IsConnectedToInternet())
                {
                    realm.Write(() => {
                        realm.RemoveAll<Result>();
                    });
                    ProjectList.Clear();

                    var savedProjects = realm.All<Result>().ToList();
                    SyncProjects(savedProjects);
                }
                else
                    IsRefreshing = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteRefreshProjects", e.Message);
                IsRefreshing = false;
            }
        }

        private async void SyncProjects(List<Result> savedProjects)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var projects = await PSapi.GetAllProjects();

                    syncDataService.SyncProjects(projects, savedProjects, ProjectList);

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
                Debug.WriteLine("SyncProjects", e.Message);
                IsRefreshing = false;

                string[] alertStrings = { "There was a problem syncing the projects. Please try again", "Close" };
                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
            }
        }
    }
}
