using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using ProjectOnlineMobile2.Android.Activities;

namespace ProjectOnlineMobile2.Android.Helpers
{
    public class CustomWebView : WebViewClient
    {
        private LoginActivity _activity;
        public CustomWebView(LoginActivity activity) {
            this._activity = activity;
        }
        bool doneNavigating = false;
        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);

            bool doneSavingTokens;

            CookieManager cookieManager = CookieManager.Instance;
            cookieManager.SetAcceptCookie(true);

            doneSavingTokens = Singleton.Instance.TokenServices.SaveCookies(cookieManager.GetCookie("https://sharepointevo.sharepoint.com/SitePages/home.aspx?AjaxDelta=1"));

            if (doneSavingTokens && !doneNavigating)
            {
                _activity.GoToLandingPage();
                doneNavigating = true;
            }
        }
    }
}