using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        protected SharepointApiWrapper SPapi { get; private set; }
        protected ProjectOnlineApiWrapper PSapi { get; private set; }

        //used for project references of other viewmodels
        public ProjectServerProjectList projects;

        //used to identify the user
        public string userName = "";

        public BaseViewModel() {
            if (SPapi == null)
                SPapi = new SharepointApiWrapper();

            if (PSapi == null)
                PSapi = new ProjectOnlineApiWrapper();

            MessagingCenter.Instance.Subscribe<ProjectServerProjectList>(this, "SetProjects", (projects)=> {
                this.projects = projects;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "UserName", (user) => {
                this.userName = user;
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
