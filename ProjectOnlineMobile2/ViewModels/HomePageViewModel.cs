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
using Plugin.Connectivity;

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

        public HomePageViewModel()
        {
            GetUserInfo();

            MessagingCenter.Instance.Subscribe<String>(this, "ClearAll", (s)=> {
                realm.Write(()=> {
                    realm.RemoveAll();
                });
                Settings.ClearAll();
            });

            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) => {
                if(IsConnectedToInternet())
                    MessagingCenter.Instance.Send<String>("", "SaveOfflineWorkChanges");
            };

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
                        var user = userInfo.First();
                        MessagingCenter.Instance.Send<ProjectOnlineMobile2.Models.D_User>(user, "UserInfo");

                        UserName = userInfo.First().Title;
                        UserEmail = userInfo.First().Email;
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
