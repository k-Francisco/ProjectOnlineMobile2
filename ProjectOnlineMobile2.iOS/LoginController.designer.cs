// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ProjectOnlineMobile2.iOS
{
    [Register ("LoginController")]
    partial class LoginController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        WebKit.WKWebView loginWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (loginWebView != null) {
                loginWebView.Dispose ();
                loginWebView = null;
            }
        }
    }
}