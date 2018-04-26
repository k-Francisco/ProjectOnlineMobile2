using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using ProjectOnlineMobile2.Android.Activities;
using ProjectOnlineMobile2.Android.Helpers;

namespace ProjectOnlineMobile2.Android.Fragments
{
    public class LoginFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static LoginFragment NewInstance()
        {
            var frag1 = new LoginFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_login, container, false);

            WebView webView = rootView.FindViewById<WebView>(Resource.Id.wbLogin);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.ClearCache(true);
            CookieManager.Instance.RemoveSessionCookie();
            webView.LoadUrl("https://sharepointevo.sharepoint.com/_forms/default.aspx?wa=wsignin1.0");
            webView.SetWebViewClient(new CustomWebView((Activity as LoginActivity)));

            return rootView;
        }

    }
}