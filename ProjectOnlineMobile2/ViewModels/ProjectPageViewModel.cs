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
            var savedProjects = realm.All<Result>().ToList();

            if (IsConnectedToInternet())
            {
                try
                {
                    var projects = await PSapi.GetAllProjects();

                    if (projects.D.Results.Any())
                    {
                        if (savedProjects.Any())
                        {
                            var isTheSame = savedProjects.SequenceEqual(projects.D.Results);
                            if (isTheSame)
                            {
                                foreach (var item in savedProjects)
                                {
                                    ProjectList.Add(item);
                                }
                            }
                            else
                            {
                                realm.Write(()=> {
                                    realm.RemoveAll<Result>();
                                });
                                
                                foreach (var item in projects.D.Results)
                                {
                                    realm.Write(()=> {
                                        realm.Add(item);
                                    });
                                    ProjectList.Add(item);
                                }

                            }
                        }
                        else
                        {
                            realm.Write(() => {
                                realm.RemoveAll<Result>();
                            });

                            foreach (var item in projects.D.Results)
                            {
                                realm.Write(() => {
                                    realm.Add(item);
                                });
                                ProjectList.Add(item);
                            }

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
                    foreach (var item in savedProjects)
                    {
                        ProjectList.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("GetProjects-offline", e.Message);
                }
            }
        }
    }
}
