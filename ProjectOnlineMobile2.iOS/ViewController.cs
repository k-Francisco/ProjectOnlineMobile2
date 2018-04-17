using Foundation;
using System;
using System.Diagnostics;
using UIKit;

namespace ProjectOnlineMobile2.iOS
{
    public partial class ViewController : UIViewController
    {
        LandingPageController LandingPageController;
        public ViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
            
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

                var authCookie = rtFa + ";" + FedAuth;
                this.NavigationController.PushViewController(new LandingPageController(), animated: true);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}