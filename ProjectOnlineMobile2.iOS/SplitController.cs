using Foundation;
using ProjectOnlineMobile2.Pages;
using System;
using UIKit;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.iOS
{
    public partial class SplitController : UISplitViewController
    {
        UIViewController _hompageController, _projectPageController, _tasksPageController, _timesheetPageController;
        UINavigationController masterView, detailView;
        public SplitController (IntPtr handle) : base (handle)
        {
            MessagingCenter.Instance.Subscribe<String>(this, "Navigate", (where) => {
                ExecuteNavigate(where);
            });

            MessagingCenter.Instance.Subscribe<String[]>(this, "DisplayAlert", (s) => {

                //s[0] = message
                //s[1] = affirm button message
                //s[3] = identifier
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
                detailView.PresentViewController(alertController, true, null);
            });
        }

        private void ExecuteNavigate(string where)
        {
            detailView = new UINavigationController();
            if (where.Equals("Projects"))
            {
                detailView.PushViewController(_projectPageController, true);
            }
            else if (where.Equals("Tasks"))
            {
                detailView.PushViewController(_tasksPageController, true);
            }
            else if (where.Equals("Timesheets"))
            {
                detailView.PushViewController(_timesheetPageController, true);
            }

            this.ViewControllers = new UIViewController[] { masterView, detailView };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;

            masterView = new UINavigationController();
            detailView = new UINavigationController();

            _hompageController = new HomePage().CreateViewController();
            _hompageController.Title = "Menu";
            masterView.NavigationBar.PrefersLargeTitles = true;
            
            _projectPageController = new ProjectPage().CreateViewController();
            _projectPageController.Title = "Projects";

            _tasksPageController = new TasksPage().CreateViewController();
            _tasksPageController.Title = "Tasks";

            _timesheetPageController = new TimesheetPage().CreateViewController();
            _timesheetPageController.Title = "Timesheet";

            masterView.PushViewController(_hompageController, false);
            detailView.PushViewController(_projectPageController, false);

            this.ViewControllers = new UIViewController[] { masterView, detailView};
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
    }
}