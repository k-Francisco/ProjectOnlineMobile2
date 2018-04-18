using Foundation;
using ProjectOnlineMobile2.Pages;
using ProjectOnlineMobile2.Services;
using System;
using UIKit;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.iOS
{
    public partial class HomeController : UIViewController
    {
        public HomeController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.NavigationBarHidden = true;

            btnLogout.TouchUpInside += ClearSettings;
        }

        private void ClearSettings(object sender, EventArgs e)
        {
            Settings.ClearAll();
        }
    }
}