using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace ProjectOnlineMobile2.ViewModels
{
    public class ProjectPageViewModel : BaseViewModel
    {
        
        public ProjectPageViewModel()
        {
            GetAllProjects();
        }

        private async void GetAllProjects()
        {
            try {
                var projects = await PSapi.GetAllProjects();
                foreach (var project in projects.D.Results)
                {
                    Debug.WriteLine("projects", project.ProjectName);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("projects", e.Message);
            }

        }
    }
}
