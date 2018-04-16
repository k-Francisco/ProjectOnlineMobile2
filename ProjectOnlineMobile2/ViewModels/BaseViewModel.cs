using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectOnlineMobile2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        protected SharepointApiWrapper SPapi { get; private set; }
        protected ProjectOnlineApiWrapper PSapi { get; private set; }

        public BaseViewModel() {
            if (SPapi == null)
                SPapi = new SharepointApiWrapper();

            if (PSapi == null)
                PSapi = new ProjectOnlineApiWrapper();
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
