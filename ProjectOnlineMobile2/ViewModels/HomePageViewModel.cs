using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public ICommand GoToProjectsPage { get; set; }
        public ICommand GoToTasksPage { get; set; }
        public ICommand GoToTimesheetPage { get; set; }

        public HomePageViewModel()
        {
            GoToProjectsPage = new Command(ExecuteGoToProjectsPage);
            GoToTasksPage = new Command(ExecuteGoToTasksPage);
            GoToTimesheetPage = new Command(ExecuteGoToTimesheetPage);
            if(Device.RuntimePlatform == Device.iOS)
            {
                GetUserInfo();
            }
            
            GetTimesheetPeriod();
        }

        private async void GetTimesheetPeriod()
        {
            var timesheetperiod = await PSapi.GetAllTimesheetPeriods();
            foreach (var item in timesheetperiod.D.Results)
            {
                NetStandardSingleton.Instance.periods.Add(item);
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

        private async void GetUserInfo()
        {
            var user = await SPapi.GetCurrentUser();
            MessagingCenter.Instance.Send<String>(user.D.Title, "UserName");
        }

        private void ExecuteGoToProjectsPage()
        {
            MessagingCenter.Instance.Send<String>("ProjectPage", "NavigateToPage");
        }
    }
}
