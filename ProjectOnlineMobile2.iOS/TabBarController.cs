using Foundation;
using ProjectOnlineMobile2.Pages;
using System;
using UIKit;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.iOS
{
    public partial class TabBarController : UITabBarController
    {
        private UIViewController _projectPageController, _tasksPageController, _timesheetPageController;
        private UINavigationController _projectNavController, _tasksNavController, _timesheetNavController;

        public TabBarController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            this.TabBar.Translucent = false;

            _projectPageController = new ProjectPage().CreateViewController();
            _projectPageController.Title = "Projects";
            _projectNavController = new UINavigationController();
            _projectNavController.NavigationBar.PrefersLargeTitles = true;
            _projectNavController.PushViewController(_projectPageController, false);

            _tasksPageController = new TasksPage().CreateViewController();
            _tasksPageController.Title = "Tasks";
            _tasksNavController = new UINavigationController();
            _tasksNavController.NavigationBar.PrefersLargeTitles = true;
            _tasksNavController.PushViewController(_tasksPageController, false);

            _timesheetPageController = new TimesheetPage().CreateViewController();
            _timesheetPageController.Title = "Timesheet";
            _timesheetNavController = new UINavigationController();
            _timesheetNavController.NavigationBar.PrefersLargeTitles = true;
            _timesheetNavController.PushViewController(_timesheetPageController, false);

            var tabs = new UIViewController[] { _projectNavController, _tasksNavController, _timesheetNavController };
            ViewControllers = tabs;
            SelectedViewController = _projectNavController;

        }
    }
}