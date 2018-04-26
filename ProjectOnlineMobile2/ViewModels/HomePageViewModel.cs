using System;
using System.Collections.Generic;
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
            GetUserInfo();
        }

        private void ExecuteGoToTimesheetPage(object obj)
        {
            MessagingCenter.Instance.Send<String>("TimesheetPage", "NavigateToPage");
        }

        private void ExecuteGoToTasksPage(object obj)
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
