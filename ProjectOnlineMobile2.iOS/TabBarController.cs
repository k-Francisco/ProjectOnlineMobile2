using Foundation;
using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using ProjectOnlineMobile2.Pages;
using System;
using UIKit;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models.TLL;

namespace ProjectOnlineMobile2.iOS
{
    public partial class TabBarController : UITabBarController
    {
        private UIViewController _projectPageController, _tasksPageController, _timesheetPageController;
        private UINavigationController _projectNavController, _tasksNavController, _timesheetNavController;

        public TabBarController (IntPtr handle) : base (handle)
        {
            MessagingCenter.Instance.Subscribe<LineResult>(this, "PushTimesheetWorkPage", (line)=> {
                ExecutePushTimesheetWorkPage(line);
            });

            MessagingCenter.Instance.Subscribe<String[]>(this, "DisplayAlert", (s) => {

                //s[0] = message
                //s[1] = affirm button message
                //s[2] = identifier
                //further strings are for necessary parameters like periodId, lineId, etc
                var alertController = UIAlertController.Create("",
                s[0],
                UIAlertControllerStyle.Alert);

                alertController.AddAction(UIAlertAction.Create(s[1], UIAlertActionStyle.Default, alert => {
                    if (s[2].Equals("Logout"))
                    {
                        MessagingCenter.Instance.Send<String>("", "ClearAll");

                        var controller = Storyboard.InstantiateViewController("LoginController") as LoginController;
                        AppDelegate.appDelegate.Window.RootViewController = controller;
                    }
                }));

                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {

                }));
                SelectedViewController.PresentViewController(alertController, true, null);
            });
        }

        private void ExecutePushTimesheetWorkPage(LineResult line)
        {
            var controller = new TimesheetWorkPage().CreateViewController();
            controller.Title = line.TaskName;
            controller.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain, (sender, e) => {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }), false);
            _timesheetNavController.PushViewController(controller, true);
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