using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models;


namespace ProjectOnlineMobile2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public ICommand GoToProjectsPage { get; set; }
        public ICommand GoToTasksPage { get; set; }
        public ICommand GoToTimesheetPage { get; set; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { SetProperty(ref _userEmail, value); }
        }

        public HomePageViewModel()
        {
            GoToProjectsPage = new Command(ExecuteGoToProjectsPage);
            GoToTasksPage = new Command(ExecuteGoToTasksPage);
            GoToTimesheetPage = new Command(ExecuteGoToTimesheetPage);

            GetUserInfo();

            MessagingCenter.Instance.Subscribe<String>(this, "Logout", (s)=> {
                realm.RemoveAll();
            });

        }

        private async void GetUserInfo()
        {
            try
            {
                var userInfo = realm.All<ProjectOnlineMobile2.Models.D_User>();

                if (IsConnectedToInternet())
                {
                    if(!userInfo.Any())
                    {
                        var user = await SPapi.GetCurrentUser();
                        UserName = user.D.Title;
                        UserEmail = user.D.Email;
                        realm.Write(()=> {
                            realm.Add<ProjectOnlineMobile2.Models.D_User>(user.D);
                        });
                        MessagingCenter.Instance.Send<ProjectOnlineMobile2.Models.D_User>(user.D, "UserInfo");
                    }
                    else
                    {
                        UserName = userInfo.First().Title;
                        UserEmail = userInfo.First().Email;
                        MessagingCenter.Instance.Send<ProjectOnlineMobile2.Models.D_User>(userInfo.First(), "UserInfo");
                    }
                }
                else
                {
                    if(userInfo != null)
                    {
                        UserName = userInfo.First().Title;
                        UserEmail = userInfo.First().Email;
                        MessagingCenter.Instance.Send<ProjectOnlineMobile2.Models.D_User>(userInfo.First(), "UserInfo");
                    }
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine("GetUserInfo", e.Message);
            }
        }

        private void ExecuteGoToTimesheetPage()
        {
            MessagingCenter.Instance.Send<String>("TimesheetPage", "NavigateToPage");
        }

        private void ExecuteGoToTasksPage()
        {
            MessagingCenter.Instance.Send<String>("TasksPage", "NavigateToPage");
        }

        private void ExecuteGoToProjectsPage()
        {
            MessagingCenter.Instance.Send<String>("ProjectPage", "NavigateToPage");
        }
    }
}
