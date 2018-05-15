using Foundation;
using Newtonsoft.Json;
using ProjectOnlineMobile2.Services;
using System;
using System.Diagnostics;
using System.Linq;
using UIKit;
using WebKit;

namespace ProjectOnlineMobile2.iOS
{
    public partial class LoginController : UIViewController, IWKNavigationDelegate, IWKUIDelegate
    {

        [Export("webView:didStartProvisionalNavigation:")]
        public async void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            var cookies = await webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookiesAsync();
            foreach (var item in cookies)
            {
                loginWebView.Configuration.WebsiteDataStore.HttpCookieStore.DeleteCookie(item, null);
            }

            activityIndicator.StartAnimating();
        }

        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            activityIndicator.StopAnimating();
        }

        string rtFa = string.Empty;
        string FedAuth = string.Empty;
        [Export("webView:didCommitNavigation:")]
        public async void DidCommitNavigation(WKWebView webView, WKNavigation navigation)
        {
            var cookies = await webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookiesAsync();

            if (string.IsNullOrEmpty(rtFa) && string.IsNullOrEmpty(FedAuth))
            {
                foreach (var cookie in cookies)
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
                        activityIndicator.StopAnimating();

                        var authCookie = rtFa + "; " + FedAuth;
                        Settings.CookieString = JsonConvert.SerializeObject(authCookie);

                        var controller = Storyboard.InstantiateViewController("TabBarController") as TabBarController;
                        AppDelegate.appDelegate.Window.RootViewController = controller;
                    }
                    catch (Exception ez)
                    {
                        Debug.WriteLine("webViewLoading", ez.Message);
                    }
                }

            }
        }

        WKWebView loginWebView;
        UIActivityIndicatorView activityIndicator;

        public LoginController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            activityIndicator = new UIActivityIndicatorView();
            activityIndicator.HidesWhenStopped = true;
            activityIndicator.StopAnimating();
            activityIndicator.Color = UIColor.FromRGBA(49, 117, 47, 1);

            var url = "https://sharepointevo.sharepoint.com";
            var request = new NSMutableUrlRequest(new NSUrl(url));

            loginWebView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            loginWebView.Bounds = View.Bounds;
            loginWebView.NavigationDelegate = this;
            loginWebView.UIDelegate = this;
            loginWebView.LoadRequest(request);

            View.AddSubview(loginWebView);
            this.loginWebView.AddSubview(activityIndicator);

        }

    }
}