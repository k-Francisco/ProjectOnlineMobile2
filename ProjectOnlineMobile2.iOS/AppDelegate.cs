using System;
using System.Diagnostics;
using Foundation;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Pages;
using ProjectOnlineMobile2.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.SideMenu;

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
        SideMenuManager _sideMenuManager;
        UIViewController controller;
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
            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGBA(49, 117, 47,1);
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(49, 117, 47, 1);

            shared = this;
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            _sideMenuManager = new SideMenuManager();
            
            if (String.IsNullOrWhiteSpace(Settings.CookieString))
            {
                controller = Storyboard.InstantiateViewController("ViewController") as ViewController;
            }
            else
            {
                controller = new HomePage().CreateViewController();
                controller.Title = "Home";
                //SetupSideMenu(controller);

                //GetUserInfo();
            }

            MessagingCenter.Instance.Subscribe<String>(this,"NavigateToPage", (s) => {
                NavigatePage(s);
            });
            
            navigationController = new UINavigationController();
            Window.RootViewController = navigationController;
            navigationController.PushViewController(controller, false);
            Window.MakeKeyAndVisible();

            return true;
        }

        private void NavigatePage(String page)
        {
            if (page.Equals("ProjectPage"))
            {
                var controller = new ProjectPage().CreateViewController();
                controller.Title = "Projects";
                navigationController.PushViewController(controller, true);
            }
            else if (page.Equals("TasksPage"))
            {
                var controller = new TasksPage().CreateViewController();
                controller.Title = "Tasks";
                navigationController.PushViewController(controller, true);
            }
            else if (page.Equals("TimesheetPage"))
            {
                var controller = new TimesheetPage().CreateViewController();
                controller.Title = "Timesheet";
                navigationController.PushViewController(controller, true);
            }
        }

        //public async void GetUserInfo()
        //{
        //    try
        //    {
        //        var sharepointApi = new SharepointApiWrapper();
        //        UserModel user = await sharepointApi.GetCurrentUser();
        //        MessagingCenter.Instance.Send<String>(user.D.Title, "UserName");
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("GetUserInfo", e.Message);
        //    }
        //}

        //public void SetupSideMenu(UIViewController controller)
        //{
        //    //if(_sideMenuManager.LeftNavigationController != null)
        //    //{
        //    //    _sideMenuManager.LeftNavigationController.DismissViewController(true, null);
        //    //}
        //    controller.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Menu", UIBarButtonItemStyle.Plain, (sender,e) => {
        //        controller.PresentViewController(_sideMenuManager.LeftNavigationController, true, null);
        //    }), false);
            
        //    _sideMenuManager.LeftNavigationController = new UISideMenuNavigationController(_sideMenuManager, Storyboard.InstantiateViewController("HomeController"), true);
        //    //_sideMenuManager.AddScreenEdgePanGesturesToPresent(toView: navigationController?.View);

        //    _sideMenuManager.PresentMode = SideMenuManager.MenuPresentMode.MenuSlideIn;
        //    _sideMenuManager.BlurEffectStyle = null;
        //    _sideMenuManager.AnimationFadeStrength = .25;
        //    _sideMenuManager.ShadowOpacity = .50;
        //    _sideMenuManager.FadeStatusBar = false;

        //}

        //public void SwitchControllers(UIViewController controller)
        //{
        //    navigationController = new UINavigationController();
        //    navigationController.PushViewController(controller, false);
        //    Window.RootViewController = navigationController;
        //    SetupSideMenu(controller);
        //}

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