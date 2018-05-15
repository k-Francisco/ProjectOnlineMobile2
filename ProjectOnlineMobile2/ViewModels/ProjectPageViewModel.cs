using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public ProjectPageViewModel()
        {
            ProjectList = new ObservableCollection<Result>();

            var savedProjects = realm.All<Result>().ToList();

            foreach (var item in savedProjects)
            {
                ProjectList.Add(item);
            }

            MessagingCenter.Instance.Subscribe<String>(this, "ProjectPageInit", (s)=> {
                SyncProjects(savedProjects);
            });
        }

        private async void SyncProjects(List<Result> savedProjects)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var projects = await PSapi.GetAllProjects();

                    syncDataService.SyncProjects(projects, savedProjects, ProjectList);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncProjects", e.Message);
            }
        }
    }
}
