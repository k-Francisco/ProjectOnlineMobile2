using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            if(projects == null)
            {
                ProjectList = new ObservableCollection<Result>();
                GetAllProjects();
                Debug.WriteLine("ProjectPageViewModel", "getting projects");
            }
        }

        private async void GetAllProjects()
        {
            try {
                var projects = await PSapi.GetAllProjects();
                MessagingCenter.Instance.Send<ProjectServerProjectList>(projects, "SetProjects");
                foreach (var project in projects.D.Results)
                {
                    ProjectList.Add(project);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ProjectPage-GetAllProjects", e.Message);
            }

        }
    }
}
