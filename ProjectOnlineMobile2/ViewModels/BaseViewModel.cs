using ProjectOnlineMobile2.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.ViewModels
{
    public class BaseViewModel
    {

        protected SharepointApiWrapper SPapi { get; private set; }
        protected ProjectOnlineApiWrapper PSapi { get; private set; }

        public BaseViewModel() {
            if (SPapi == null)
                SPapi = new SharepointApiWrapper();

            if (PSapi == null)
                PSapi = new ProjectOnlineApiWrapper();
        }
    }
}
