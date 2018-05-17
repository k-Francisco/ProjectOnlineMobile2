using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Services;

namespace ProjectOnlineMobile2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
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

        private HomePageModel _selectedItem;
        public HomePageModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public ICommand SelectedItemCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public List<HomePageModel> MasterList { get; set; }

        private HomePageModel _projects, _tasks, _timesheets;

        public HomePageViewModel()
        {
            GetUserInfo();

            SelectedItemCommand = new Command(ExecuteSelectedItemCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);

            Debug.WriteLine("HomePageViewModel", "here");

            MasterList = new List<HomePageModel>();

            _projects = new HomePageModel() {
                Title = "Projects",
                Image = ""
            };
            _tasks = new HomePageModel()
            {
                Title = "Tasks",
                Image = ""
            };
            _timesheets = new HomePageModel()
            {
                Title = "Timesheets",
                Image = ""
            };

            MasterList.Add(_projects);
            MasterList.Add(_tasks);
            MasterList.Add(_timesheets);

            SelectedItem = _projects;

            MessagingCenter.Instance.Subscribe<String>(this, "ClearAll", (s)=> {
                realm.Write(()=> {
                    realm.RemoveAll();
                });
                Settings.ClearAll();
            });

        }

        private void ExecuteLogoutCommand()
        {
            string[]  strings = { "Are you sure you want to log out?", "Logout", "Logout"};
            MessagingCenter.Instance.Send<String[]>(strings, "DisplayAlert");
        }

        private void ExecuteSelectedItemCommand()
        {
            MessagingCenter.Instance.Send<String>(SelectedItem.Title, "Navigate");
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

    }
}
