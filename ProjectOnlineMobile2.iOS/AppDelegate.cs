using System;
using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using System.Diagnostics;
using Foundation;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Pages;
using ProjectOnlineMobile2.Services;
using UIKit;
using Xamarin.Forms;
using System.IO;

[assembly: Preserve(typeof(System.Linq.Queryable), AllMembers = true)]
namespace ProjectOnlineMobile2.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public static AppDelegate shared;
        public static UIStoryboard Storyboard = UIStoryboard.FromName("Main", null);

        public UINavigationController navigationController;
        UIViewController controller;
        private UIViewController _projectPage, _tasksPage, _timesheetPage;
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            Forms.Init();
            
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                TextColor = UIColor.White
            });
            UINavigationBar.Appearance.LargeTitleTextAttributes = new UIStringAttributes() {
                ForegroundColor = UIColor.FromRGB(255, 255, 255)
            };
            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGBA(49, 117, 47,1);
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(49, 117, 47, 1);

            shared = this;
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            if (String.IsNullOrWhiteSpace(Settings.CookieString))
            {
                controller = Storyboard.InstantiateViewController("ViewController") as ViewController;
            }
            else
            {
                controller = new HomePage().CreateViewController();
                controller.Title = "Home";
            }

            MessagingCenter.Instance.Subscribe<String>(this,"NavigateToPage", (s) => {
                NavigatePage(s);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "DoCreateTimesheet", (periodId) => {
                DisplayAlertDialog(periodId);
            });

            MessagingCenter.Instance.Subscribe<LineResult>(this, "PushTimesheetWorkPage", (timesheetLine) => {
                PushTimesheetWorkPage(timesheetLine);
            });

            navigationController = new UINavigationController();
            navigationController.NavigationBar.PrefersLargeTitles = true;


            //var searchController = new UISearchController(searchResultsController: null)
            //{
            //    HidesNavigationBarDuringPresentation = true,
            //    DimsBackgroundDuringPresentation = true,
            //};
            //searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
            //searchController.SearchBar.BackgroundColor = UIColor.White;
            //searchController.SearchBar.Placeholder = "search";

            //controller.NavigationItem.SearchController = searchController;

            Window.RootViewController = navigationController;
            navigationController.PushViewController(controller, false);
            Window.MakeKeyAndVisible();

            return true;
        }

        private void PushTimesheetWorkPage(LineResult timesheetLine)
        {
            var controller = new TimesheetWorkPage().CreateViewController();
            controller.Title = timesheetLine.TaskName;
            controller.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain, (sender, e) => {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }),false);
            navigationController.PushViewController(controller, true);
        }

        private void DisplayAlertDialog(String periodId)
        {
            var doCreateTimesheet = UIAlertController.Create("", 
                "The timesheet for this period has not been created. Do you want to create this timesheet?",
                UIAlertControllerStyle.Alert);

            doCreateTimesheet.AddAction(UIAlertAction.Create("Create", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<string>(periodId, "CreateTimesheet");
            }));

            doCreateTimesheet.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {
                
            }));

            navigationController.PresentViewController(doCreateTimesheet, true, null);
        }

        private void NavigatePage(String page)
        {
            if (page.Equals("ProjectPage"))
            {
                if(_projectPage == null)
                    _projectPage = new ProjectPage().CreateViewController();
                    _projectPage.Title = "Projects";

                navigationController.PushViewController(_projectPage, true);
            }
            else if (page.Equals("TasksPage"))
            {
                if(_tasksPage == null)
                    _tasksPage = new TasksPage().CreateViewController();
                    _tasksPage.Title = "Tasks";

                navigationController.PushViewController(_tasksPage, true);
            }
            else if (page.Equals("TimesheetPage"))
            {
                if(_timesheetPage == null)
                    _timesheetPage = new TimesheetPage().CreateViewController();
                    _timesheetPage.Title = "Timesheet";

                navigationController.PushViewController(_timesheetPage, true);
            }
            else if (page.Equals("Logout"))
            {
                var logoutDialog = UIAlertController.Create("",
                "Are you sure you want to log out?",
                UIAlertControllerStyle.Alert);

                logoutDialog.AddAction(UIAlertAction.Create("Logout", UIAlertActionStyle.Default, alert => {
                    MessagingCenter.Instance.Send<String>("", "ClearAll");

                    var controller = Storyboard.InstantiateViewController("ViewController") as ViewController;
                    navigationController.PushViewController(controller, true);
                    AppDelegate.shared.navigationController = new UINavigationController(controller);
                    AppDelegate.shared.Window.RootViewController = AppDelegate.shared.navigationController;
                }));

                logoutDialog.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {

                }));

                navigationController.PresentViewController(logoutDialog, true, null);
            }
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}