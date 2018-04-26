using Foundation;
using Newtonsoft.Json;
using ProjectOnlineMobile2.Pages;
using ProjectOnlineMobile2.Services;
using System;
using System.Diagnostics;
using UIKit;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.iOS
{
    public partial class ViewController : UIViewController
    {
        HomeController _homeController;
        public ViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
            //var storyboard = AppDelegate.Storyboard;
            try
            {
                _homeController = this.Storyboard.InstantiateViewController("HomeController") as HomeController;
            }
            catch(Exception e)
            {
                Debug.WriteLine("controller", e.Message);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            this.NavigationController.NavigationBarHidden = true;


            var url = "https://sharepointevo.sharepoint.com/_forms/default.aspx?wa=wsignin1.0";
            var request = new NSMutableUrlRequest(new NSUrl(url));
            webView.Frame = View.Frame;
            webView.LoadRequest(request);
            webView.LoadStarted += webViewLoading;
          
        }

        string rtFa = string.Empty;
        string FedAuth = string.Empty;
        private void webViewLoading(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(rtFa) && string.IsNullOrEmpty(FedAuth))
            {
                foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
                {
                    if (cookie.Name.Equals("rtFa"))
                    {
                        rtFa = cookie.Name + "=" + cookie.Value;
                    }

                    if (cookie.Name.Equals("FedAuth"))
                    {
                        FedAuth = cookie.Name + "=" + cookie.Value;
                    }
                }
                
                if (!string.IsNullOrEmpty(rtFa) && !string.IsNullOrEmpty(FedAuth))
                {
                    try
                    {
                        //save the cookies locally
                        var authCookie = rtFa + "; " + FedAuth;
                        Settings.CookieString = JsonConvert.SerializeObject(authCookie);

                        //get user information through api
                        //AppDelegate.shared.GetUserInfo();

                        //navigate to the projects page
                        var controller = new HomePage().CreateViewController();
                        controller.Title = "Home";
                        this.NavigationController.PushViewController(controller, true);
                        AppDelegate.shared.navigationController = new UINavigationController(controller);
                        AppDelegate.shared.Window.RootViewController = AppDelegate.shared.navigationController;
                    }
                    catch (Exception ez)
                    {
                        Debug.WriteLine("webViewLoading", ez.Message);
                    }
                }
                
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}