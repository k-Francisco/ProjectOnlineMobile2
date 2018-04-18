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
    [Register ("HomeController")]
    partial class HomeController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnGo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnGo != null) {
                btnGo.Dispose ();
                btnGo = null;
            }
        }
    }
}