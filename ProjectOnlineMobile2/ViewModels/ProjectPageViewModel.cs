using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

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
            GetAllProjects();
        }

        private async void GetAllProjects()
        {
            try {
                var projects = await PSapi.GetAllProjects();
                foreach (var project in projects.D.Results)
                {
                    ProjectList.Add(project);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("projects", e.Message);
            }

        }
    }
}
