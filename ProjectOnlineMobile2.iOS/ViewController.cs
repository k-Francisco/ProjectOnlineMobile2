using Foundation;
using Newtonsoft.Json;
using ProjectOnlineMobile2.Pages;
using ProjectOnlineMobile2.Services;
using System;
using System.Diagnostics;
using UIKit;

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


            var url = "https://sharepointevo.sharepoint.com";
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

                if(!string.IsNullOrEmpty(rtFa) && !string.IsNullOrEmpty(FedAuth))
                {
                    var authCookie = rtFa + "; " + FedAuth;
                    
                    try
                    {
                        Settings.CookieString = JsonConvert.SerializeObject(authCookie);
                        this.NavigationController.PushViewController(_homeController, true);
                        AppDelegate.shared.navigationController = new UINavigationController(_homeController);
                        AppDelegate.shared.Window.RootViewController = AppDelegate.shared.navigationController;
                    }
                    catch (Exception ez)
                    {
                        Debug.WriteLine("waaa", ez.Message);
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