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

            GetProjects();
        }

        private async void GetProjects()
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    var projects = await PSapi.GetAllProjects();

                    if (projects.D.Results.Any())
                    {
                        //TODO: check the collections form the database and server if it matches

                        foreach (var item in projects.D.Results)
                        {
                            ProjectList.Add(item);
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("GetProjects-online", e.Message);
                }
            }
            else
            {
                try
                {
                    //Retrieve items from the db
                }
                catch (Exception e)
                {
                    Debug.WriteLine("GetProjects-offline", e.Message);
                }
            }
        }
    }
}
